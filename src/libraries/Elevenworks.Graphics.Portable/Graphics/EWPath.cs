using System;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public enum PathOperation
    {
        MOVE_TO,
        LINE,
        QUAD,
        CUBIC,
        ARC,
        CLOSE
    }

    public class EWPath : IDisposable
    {
        private readonly List<float> _arcAngles;
        private readonly List<bool> _arcClockwise;
        private readonly List<EWPoint> _points;
        private readonly List<PathOperation> _segmentTypes;
        private int _subPathCount;
        private readonly List<bool> _subPathsClosed;

        private EWRectangle _cachedBounds;
        private object _nativePath;
        
        private EWPath(List<EWPoint> aPoints, List<float> aArcSizes, List<bool> arcClockwise, List<PathOperation> aOperations, int aSubPathCount)
        {
            _points = aPoints;
            _arcAngles = aArcSizes;
            _arcClockwise = arcClockwise;
            _segmentTypes = aOperations;
            _subPathCount = aSubPathCount;
            _subPathsClosed = new List<bool>();

            int vSubpathIndex = 0;
            foreach (PathOperation vSegmentType in _segmentTypes)
            {
                if (vSegmentType == PathOperation.MOVE_TO)
                {
                    vSubpathIndex++;
                    _subPathsClosed.Add(false);
                }
                else if (vSegmentType == PathOperation.CLOSE)
                {
                    _subPathsClosed.RemoveAt(vSubpathIndex - 1);
                    _subPathsClosed.Add(true);
                }
            }
        }

        public EWPath(EWPath aPath) : this()
        {
            _segmentTypes.AddRange(aPath._segmentTypes);
            foreach (var vPoint in aPath.Points)
            {
                _points.Add(new EWPoint(vPoint));
            }

            _arcAngles.AddRange(aPath._arcAngles);
            _arcClockwise.AddRange(aPath._arcClockwise);

            foreach (PathOperation vSegmentType in _segmentTypes)
            {
                if (vSegmentType == PathOperation.MOVE_TO)
                {
                    _subPathCount++;
                    _subPathsClosed.Add(false);
                }
                else if (vSegmentType == PathOperation.CLOSE)
                {
                    _subPathsClosed.RemoveAt(_subPathCount - 1);
                    _subPathsClosed.Add(true);
                }
            }
        }

        public EWPath(EWImmutablePoint aPoint) : this()
        {
            MoveTo(aPoint.X, aPoint.Y);
        }

        public EWPath(float x, float y) : this(new EWPoint(x, y))
        {
        }

        public EWPath()
        {
            _subPathCount = 0;
            _arcAngles = new List<float>();
            _arcClockwise = new List<bool>();
            _points = new List<EWPoint>();
            _segmentTypes = new List<PathOperation>();
            _subPathsClosed = new List<bool>();
        }

        public int SubPathCount => _subPathCount;

        public bool Closed
        {
            get
            {
                if (_segmentTypes.Count > 0)
                {
                    return _segmentTypes[_segmentTypes.Count - 1] == PathOperation.CLOSE;
                }

                return false;
            }
        }

        public EWPoint FirstPoint
        {
            get
            {
                if (_points != null && _points.Count > 0)
                {
                    return _points[0];
                }

                return null;
            }
        }

        public IEnumerable<PathOperation> SegmentTypes
        {
            get
            {
                for (int i = 0; i < _segmentTypes.Count; i++)
                {
                    yield return _segmentTypes[i];
                }
            }
        }

        public IEnumerable<EWImmutablePoint> Points
        {
            get
            {
                for (int i = 0; i < _points.Count; i++)
                {
                    yield return _points[i];
                }
            }
        }

        public IEnumerable<EWImmutablePoint> SegmentIntersectionPoints
        {
            get
            {
                int vIndex = 0;
                //int vArcIndex = 0;
                int vSegmentCount = _segmentTypes.Count;

                for (int s = 0; s < vSegmentCount; s++)
                {
                    var vType = _segmentTypes[s];

                    if (vType == PathOperation.MOVE_TO)
                    {
                        yield return _points[vIndex++];
                    }
                    else if (vType == PathOperation.LINE)
                    {
                        yield return _points[vIndex++];
                    }
                    else if (vType == PathOperation.QUAD)
                    {
                        vIndex++;
                        yield return _points[vIndex++];
                    }
                    else if (vType == PathOperation.CUBIC)
                    {
                        vIndex += 2;
                        yield return _points[vIndex++];
                    }
                    else if (vType == PathOperation.ARC)
                    {
                        vIndex++;
                        yield return _points[vIndex++];
                    }
                }
            }
        }
        
        public EWImmutablePoint LastPoint
        {
            get
            {
                if (_points != null && _points.Count > 0)
                    return _points[_points.Count - 1];

                return null;
            }
        }

        public int LastPointIndex
        {
            get
            {
                if (_points != null && _points.Count > 0)
                    return _points.Count - 1;

                return -1;
            }
        }

        public EWImmutablePoint this[int aIndex]
        {
            get
            {
                if (aIndex < 0 || aIndex >= _points.Count)
                {
                    return null;
                }

                return _points[aIndex];
            }
            //set { points[index] = value; }
        }

        public void SetPoint(int index, float x, float y)
        {
            _points[index].X = x;
            _points[index].Y = y;
            Invalidate();
        }

        public void SetPoint(int index, EWImmutablePoint point)
        {
            _points[index].X = point.X;
            _points[index].Y = point.Y;
            Invalidate();
        }

        public int Count => _points.Count;

        public int SegmentCount => _segmentTypes.Count;

        public int SegmentCountExcludingOpenAndClose
        {
            get
            {
                if (_segmentTypes != null)
                {
                    int vCount = _segmentTypes.Count;
                    if (vCount > 0)
                    {
                        if (_segmentTypes[0] == PathOperation.MOVE_TO)
                        {
                            vCount--;
                        }

                        if (_segmentTypes[_segmentTypes.Count - 1] == PathOperation.CLOSE)
                        {
                            vCount--;
                        }
                    }

                    return vCount;
                }

                return 0;
            }
        }
        
        public PathOperation GetSegmentType(int aIndex)
        {
            return _segmentTypes[aIndex];
        }

        public float GetArcAngle(int aIndex)
        {
            if (_arcAngles.Count > aIndex)
            {
                return _arcAngles[aIndex];
            }

            return 0;
        }

        public void SetArcAngle(int aIndex, float aValue)
        {
            if (_arcAngles.Count > aIndex)
            {
                _arcAngles[aIndex] = aValue;
            }

            Invalidate();
        }

        public bool GetArcClockwise(int aIndex)
        {
            if (_arcClockwise.Count > aIndex)
            {
                return _arcClockwise[aIndex];
            }

            return false;
        }

        public void SetArcClockwise(int aIndex, bool aValue)
        {
            if (_arcClockwise.Count > aIndex)
            {
                _arcClockwise[aIndex] = aValue;
            }

            Invalidate();
        }

        public EWPath MoveTo(float x, float y)
        {
            return MoveTo(new EWPoint(x, y));
        }

        public EWPath MoveTo(EWPoint aPoint)
        {
            _subPathCount++;
            _subPathsClosed.Add(false);
            _points.Add(aPoint);
            _segmentTypes.Add(PathOperation.MOVE_TO);
            Invalidate();
            return this;
        }

        public void Close()
        {
            if (!Closed)
            {
                _subPathsClosed.RemoveAt(_subPathCount - 1);
                _subPathsClosed.Add(true);
                _segmentTypes.Add(PathOperation.CLOSE);
            }

            Invalidate();
        }

        public void Open()
        {
            if (_segmentTypes[_segmentTypes.Count - 1] == PathOperation.CLOSE)
            {
                _subPathsClosed.RemoveAt(_subPathCount - 1);
                _subPathsClosed.Add(false);
                _segmentTypes.RemoveAt(_segmentTypes.Count - 1);
            }

            Invalidate();
        }

        public EWPath LineTo(float x, float y)
        {
            return LineTo(new EWPoint(x, y));
        }

        public EWPath LineTo(EWPoint aPoint)
        {
            if (_points.Count == 0)
            {
                _points.Add(aPoint);
                _subPathCount++;
                _subPathsClosed.Add(false);
                _segmentTypes.Add(PathOperation.MOVE_TO);
            }
            else
            {
                _points.Add(aPoint);
                _segmentTypes.Add(PathOperation.LINE);
            }

            Invalidate();

            return this;
        }

        public EWPath InsertLineTo(EWPoint aPoint, int aIndex)
        {
            if (aIndex == 0)
            {
                aIndex = 1;
            }

            if (aIndex == SegmentCount)
            {
                LineTo(aPoint);
            }
            else
            {
                int vPointIndex = GetSegmentPointIndex(aIndex);
                _points.Insert(vPointIndex, aPoint);
                _segmentTypes.Insert(aIndex, PathOperation.LINE);
                Invalidate();
            }

            return this;
        }

        public EWPath AddArc(float x1, float y1, float x2, float y2, float startAngle, float endAngle, bool clockwise)
        {
            return AddArc(new EWPoint(x1, y1), new EWPoint(x2, y2), startAngle, endAngle, clockwise);
        }

        public EWPath AddArc(EWPoint topLeft, EWPoint bottomRight, float startAngle, float endAngle, bool clockwise)
        {
            if (Count == 0 || SegmentCount == 0 || GetSegmentType(SegmentCount - 1) == PathOperation.CLOSE)
            {
                _subPathCount++;
                _subPathsClosed.Add(false);
            }

            _points.Add(topLeft);
            _points.Add(bottomRight);
            _arcAngles.Add(startAngle);
            _arcAngles.Add(endAngle);
            _arcClockwise.Add(clockwise);
            _segmentTypes.Add(PathOperation.ARC);
            Invalidate();
            return this;
        }

        public EWPath QuadTo(float cx, float cy, float x, float y)
        {
            return QuadTo(new EWPoint(cx, cy), new EWPoint(x, y));
        }

        public EWPath QuadTo(EWPoint aControlPoint, EWPoint aPoint)
        {
            _points.Add(aControlPoint);
            _points.Add(aPoint);
            _segmentTypes.Add(PathOperation.QUAD);
            Invalidate();
            return this;
        }

        public EWPath InsertQuadTo(EWPoint aControlPoint, EWPoint aPoint, int aIndex)
        {
            if (aIndex == 0)
            {
                aIndex = 1;
            }

            if (aIndex == SegmentCount)
            {
                QuadTo(aControlPoint, aPoint);
            }
            else
            {
                int vPointIndex = GetSegmentPointIndex(aIndex);
                _points.Insert(vPointIndex, aPoint);
                _points.Insert(vPointIndex, aControlPoint);
                _segmentTypes.Insert(aIndex, PathOperation.QUAD);
                Invalidate();
            }

            return this;
        }

        public EWPath CurveTo(float c1X, float c1Y, float c2X, float c2Y, float x, float y)
        {
            return CurveTo(new EWPoint(c1X, c1Y), new EWPoint(c2X, c2Y), new EWPoint(x, y));
        }

        public EWPath CurveTo(EWPoint aControlPoint1, EWPoint aControlPoint2, EWPoint aPoint)
        {
            _points.Add(aControlPoint1);
            _points.Add(aControlPoint2);
            _points.Add(aPoint);
            _segmentTypes.Add(PathOperation.CUBIC);
            Invalidate();
            return this;
        }

        public EWPath InsertCurveTo(EWPoint aControlPoint1, EWPoint aControlPoint2, EWPoint aPoint, int aIndex)
        {
            if (aIndex == 0)
            {
                aIndex = 1;
            }

            if (aIndex == SegmentCount)
            {
                CurveTo(aControlPoint1, aControlPoint2, aPoint);
            }
            else
            {
                int vPointIndex = GetSegmentPointIndex(aIndex);
                _points.Insert(vPointIndex, aPoint);
                _points.Insert(vPointIndex, aControlPoint2);
                _points.Insert(vPointIndex, aControlPoint1);
                _segmentTypes.Insert(aIndex, PathOperation.CUBIC);
                Invalidate();
            }

            return this;
        }

        public int GetSegmentPointIndex(int aIndex)
        {
            if (aIndex <= SegmentCount)
            {
                int vPointIndex = 0;
                for (int vSegment = 0; vSegment < _segmentTypes.Count; vSegment++)
                {
                    PathOperation vType = _segmentTypes[vSegment];
                    if (vType == PathOperation.MOVE_TO)
                    {
                        if (vSegment == aIndex)
                        {
                            return vPointIndex;
                        }

                        vPointIndex++;
                    }
                    else if (vType == PathOperation.LINE)
                    {
                        if (vSegment == aIndex)
                        {
                            return vPointIndex;
                        }

                        vPointIndex++;
                    }
                    else if (vType == PathOperation.QUAD)
                    {
                        if (vSegment == aIndex)
                        {
                            return vPointIndex;
                        }

                        vPointIndex += 2;
                    }
                    else if (vType == PathOperation.CUBIC)
                    {
                        if (vSegment == aIndex)
                        {
                            return vPointIndex;
                        }

                        vPointIndex += 3;
                    }
                    else if (vType == PathOperation.ARC)
                    {
                        if (vSegment == aIndex)
                        {
                            return vPointIndex;
                        }

                        vPointIndex += 2;
                    }
                    else if (vType == PathOperation.CLOSE)
                    {
                        if (vSegment == aIndex)
                        {
                            return vPointIndex;
                        }
                    }
                }
            }

            return -1;
        }

        public PathOperation GetSegmentInfo(int segmentIndex, out int pointIndex, out int arcAngleIndex, out int arcClockwiseIndex)
        {
            pointIndex = 0;
            arcAngleIndex = 0;
            arcClockwiseIndex = 0;

            if (segmentIndex <= SegmentCount)
            {
                for (int s = 0; s < _segmentTypes.Count; s++)
                {
                    var type = _segmentTypes[s];
                    if (type == PathOperation.MOVE_TO)
                    {
                        if (s == segmentIndex) return type;

                        pointIndex++;
                    }
                    else if (type == PathOperation.LINE)
                    {
                        if (s == segmentIndex) return type;
                        pointIndex++;
                    }
                    else if (type == PathOperation.QUAD)
                    {
                        if (s == segmentIndex) return type;
                        pointIndex += 2;
                    }
                    else if (type == PathOperation.CUBIC)
                    {
                        if (s == segmentIndex) return type;
                        pointIndex += 3;
                    }
                    else if (type == PathOperation.ARC)
                    {
                        if (s == segmentIndex) return type;
                        pointIndex += 2;
                        arcAngleIndex += 2;
                        arcClockwiseIndex++;
                    }
                    else if (type == PathOperation.CLOSE)
                    {
                        if (s == segmentIndex) return type;
                    }
                }
            }

            return PathOperation.CLOSE;
        }

        public int GetSegmentForPoint(int pointIndex)
        {
            if (pointIndex < _points.Count)
            {
                int index = 0;
                for (int segment = 0; segment < _segmentTypes.Count; segment++)
                {
                    var segmentType = _segmentTypes[segment];
                    if (segmentType == PathOperation.MOVE_TO)
                    {
                        if (pointIndex == index++)
                        {
                            return segment;
                        }
                    }
                    else if (segmentType == PathOperation.LINE)
                    {
                        if (pointIndex == index++)
                        {
                            return segment;
                        }
                    }
                    else if (segmentType == PathOperation.QUAD)
                    {
                        if (pointIndex == index++)
                        {
                            return segment;
                        }

                        if (pointIndex == index++)
                        {
                            return segment;
                        }
                    }
                    else if (segmentType == PathOperation.CUBIC)
                    {
                        if (pointIndex == index++)
                        {
                            return segment;
                        }

                        if (pointIndex == index++)
                        {
                            return segment;
                        }

                        if (pointIndex == index++)
                        {
                            return segment;
                        }
                    }
                    else if (segmentType == PathOperation.ARC)
                    {
                        if (pointIndex == index++)
                        {
                            return segment;
                        }

                        if (pointIndex == index++)
                        {
                            return segment;
                        }
                    }
                }
            }

            return -1;
        }

        public EWImmutablePoint[] GetPointsForSegment(int segmentIndex)
        {
            if (segmentIndex <= SegmentCount)
            {
                int pointIndex = 0;
                for (int segment = 0; segment < _segmentTypes.Count; segment++)
                {
                    var segmentType = _segmentTypes[segment];
                    if (segmentType == PathOperation.MOVE_TO)
                    {
                        if (segment == segmentIndex)
                        {
                            var vPoints = new EWImmutablePoint[] {_points[pointIndex]};
                            return vPoints;
                        }

                        pointIndex++;
                    }
                    else if (segmentType == PathOperation.LINE)
                    {
                        if (segment == segmentIndex)
                        {
                            var vPoints = new EWImmutablePoint[] {_points[pointIndex]};
                            return vPoints;
                        }

                        pointIndex++;
                    }

                    else if (segmentType == PathOperation.QUAD)
                    {
                        if (segment == segmentIndex)
                        {
                            var vPoints = new EWImmutablePoint[] {_points[pointIndex++], _points[pointIndex]};
                            return vPoints;
                        }

                        pointIndex += 2;
                    }
                    else if (segmentType == PathOperation.CUBIC)
                    {
                        if (segment == segmentIndex)
                        {
                            var vPoints = new EWImmutablePoint[] {_points[pointIndex++], _points[pointIndex++], _points[pointIndex]};
                            return vPoints;
                        }

                        pointIndex += 3;
                    }
                    else if (segmentType == PathOperation.ARC)
                    {
                        if (segment == segmentIndex)
                        {
                            var vPoints = new EWImmutablePoint[] {_points[pointIndex++], _points[pointIndex]};
                            return vPoints;
                        }

                        pointIndex += 2;
                    }
                    else if (segmentType == PathOperation.CLOSE)
                    {
                        if (segment == segmentIndex)
                        {
                            return new EWImmutablePoint[] { };
                        }
                    }
                }
            }

            return null;
        }

        private void RemoveAllAfter(int pointIndex, int segmentIndex, int arcIndex, int arcClockwiseIndex)
        {
            _points.RemoveRange(pointIndex, _points.Count - pointIndex);
            _segmentTypes.RemoveRange(segmentIndex, _segmentTypes.Count - segmentIndex);
            _arcAngles.RemoveRange(arcIndex, _arcAngles.Count - arcIndex);
            _arcClockwise.RemoveRange(arcClockwiseIndex, _arcClockwise.Count - arcClockwiseIndex);

            _subPathCount = 0;
            _subPathsClosed.Clear();

            foreach (PathOperation vCommand in _segmentTypes)
            {
                if (vCommand == PathOperation.MOVE_TO)
                {
                    _subPathCount++;
                    _subPathsClosed.Add(false);
                }
                else if (vCommand == PathOperation.CLOSE)
                {
                    _subPathsClosed.RemoveAt(_subPathCount);
                    _subPathsClosed.Add(true);
                }
            }

            if (_subPathCount > 0)
            {
                _subPathCount--;
            }

            Invalidate();
        }

        public void RemoveAllSegmentsAfter(int segmentIndex)
        {
            if (segmentIndex <= SegmentCount)
            {
                int pointIndex = 0;
                int arcIndex = 0;
                int arcClockwiseIndex = 0;
                for (var segment = 0; segment < _segmentTypes.Count; segment++)
                {
                    var segmentType = _segmentTypes[segment];

                    if (segment == segmentIndex)
                    {
                        RemoveAllAfter(pointIndex, segment, arcIndex, arcClockwiseIndex);
                        return;
                    }

                    if (segmentType == PathOperation.MOVE_TO)
                    {
                        pointIndex++;
                    }
                    else if (segmentType == PathOperation.LINE)
                    {
                        pointIndex++;
                    }
                    else if (segmentType == PathOperation.QUAD)
                    {
                        pointIndex += 2;
                    }
                    else if (segmentType == PathOperation.CUBIC)
                    {
                        pointIndex += 3;
                    }
                    else if (segmentType == PathOperation.ARC)
                    {
                        pointIndex += 2;
                        arcIndex += 2;
                        arcClockwiseIndex += 1;
                    }
                }
            }

            Invalidate();
        }

        public void RemoveSegment(int segmentIndex)
        {
            if (segmentIndex <= SegmentCount)
            {
                Invalidate();

                int pointIndex = 0;
                int arcIndex = 0;
                int arcClockwiseIndex = 0;

                for (int segment = 0; segment < _segmentTypes.Count; segment++)
                {
                    var segmentType = _segmentTypes[segment];
                    if (segmentType == PathOperation.MOVE_TO)
                    {
                        if (segment == segmentIndex)
                        {
                            if (segmentIndex == _segmentTypes.Count - 1)
                            {
                                var points = GetPointsForSegment(segmentIndex);
                                if (points != null)
                                {
                                    for (int i = 0; i < points.Length; i++)
                                    {
                                        _points.RemoveAt(pointIndex);
                                    }
                                }

                                _segmentTypes.RemoveAt(segmentIndex);
                            }
                            else
                            {
                                var points = GetPointsForSegment(segmentIndex + 1);
                                if (points != null)
                                {
                                    if (points.Length > 0)
                                    {
                                        _points[pointIndex] = (EWPoint) points[points.Length - 1];
                                        for (int i = 0; i < points.Length; i++)
                                        {
                                            _points.RemoveAt(pointIndex + 1);
                                        }
                                    }

                                    _segmentTypes.RemoveAt(segmentIndex + 1);
                                }
                            }

                            return;
                        }

                        pointIndex++;
                    }
                    else if (segmentType == PathOperation.LINE)
                    {
                        if (segment == segmentIndex)
                        {
                            _points.RemoveAt(pointIndex);
                            _segmentTypes.RemoveAt(segmentIndex);
                            return;
                        }

                        pointIndex++;
                    }
                    else if (segmentType == PathOperation.QUAD)
                    {
                        if (segment == segmentIndex)
                        {
                            _points.RemoveAt(pointIndex);
                            _points.RemoveAt(pointIndex);
                            _segmentTypes.RemoveAt(segmentIndex);
                            return;
                        }

                        pointIndex += 2;
                    }
                    else if (segmentType == PathOperation.CUBIC)
                    {
                        if (segment == segmentIndex)
                        {
                            _points.RemoveAt(pointIndex);
                            _points.RemoveAt(pointIndex);
                            _points.RemoveAt(pointIndex);
                            _segmentTypes.RemoveAt(segmentIndex);
                            return;
                        }

                        pointIndex += 3;
                    }
                    else if (segmentType == PathOperation.ARC)
                    {
                        if (segment == segmentIndex)
                        {
                            _points.RemoveAt(pointIndex);
                            _points.RemoveAt(pointIndex);
                            _segmentTypes.RemoveAt(segmentIndex);
                            _arcAngles.RemoveAt(arcIndex);
                            _arcAngles.RemoveAt(arcIndex);
                            _arcClockwise.RemoveAt(arcClockwiseIndex);
                            return;
                        }

                        pointIndex += 2;
                        arcIndex += 2;
                        arcClockwiseIndex += 1;
                    }
                    else if (segmentType == PathOperation.CLOSE)
                    {
                        if (segment == segmentIndex)
                        {
                            _segmentTypes.RemoveAt(segmentIndex);
                            return;
                        }
                    }
                }
            }
        }
        
        public EWPath Rotate(float angleAsDegrees, EWImmutablePoint pivot)
        {
            var path = new EWPath();

            int index = 0;
            int arcIndex = 0;
            int clockwiseIndex = 0;

            foreach (var segmentType in _segmentTypes)
            {
                if (segmentType == PathOperation.MOVE_TO)
                {
                    var rotatedPoint = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    path.MoveTo(rotatedPoint);
                }
                else if (segmentType == PathOperation.LINE)
                {
                    var rotatedPoint = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    path.LineTo(rotatedPoint.X, rotatedPoint.Y);
                }
                else if (segmentType == PathOperation.QUAD)
                {
                    var rotatedControlPoint = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    var rotatedEndPoint = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    path.QuadTo(rotatedControlPoint.X, rotatedControlPoint.Y, rotatedEndPoint.X, rotatedEndPoint.Y);
                }
                else if (segmentType == PathOperation.CUBIC)
                {
                    var rotatedControlPoint1 = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    var rotatedControlPoint2 = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    var rotatedEndPoint = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    path.CurveTo(rotatedControlPoint1.X, rotatedControlPoint1.Y, rotatedControlPoint2.X, rotatedControlPoint2.Y, rotatedEndPoint.X, rotatedEndPoint.Y);
                }
                else if (segmentType == PathOperation.ARC)
                {
                    var topLeft = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    var bottomRight = GetRotatedPoint(index++, pivot, angleAsDegrees);
                    var startAngle = _arcAngles[arcIndex++];
                    var endAngle = _arcAngles[arcIndex++];
                    var clockwise = _arcClockwise[clockwiseIndex++];

                    path.AddArc(new EWPoint(topLeft), new EWPoint(bottomRight), startAngle, endAngle, clockwise);
                }
                else if (segmentType == PathOperation.CLOSE)
                {
                    path.Close();
                }
            }

            return path;
        }

        public EWPoint GetRotatedPoint(int pointIndex, EWImmutablePoint pivotPoint, float angle)
        {
            var point = _points[pointIndex];
            return Geometry.RotatePoint(pivotPoint, point, angle);
        }

        /*
       * 
       * 
       * float cx = vRect.X + vRect.Width / 2;
         float cy = vRect.Y + vRect.Height / 2;
         
         CGAffineTransform vTransform = CGAffineTransform.MakeTranslation(cx,cy);			
         vTransform.Rotate ((float)Geometry.DegreesToRadians (aDegrees));
         vTransform.Translate(-cx,-cy);
         
         vPath.
       */

        public void Transform(EWAffineTransform aTransform)
        {
            foreach (EWPoint vPoint in _points)
            {
                EWPoint vNewPoint = aTransform.Transform(vPoint);
                vPoint.X = vNewPoint.X;
                vPoint.Y = vNewPoint.Y;
            }

            Invalidate();
        }
        
        public List<EWPath> Separate()
        {
            var vPaths = new List<EWPath>();
            if (_points == null || _segmentTypes == null)
                return vPaths;

            EWPath vPath = null;

            // ReSharper disable PossibleNullReferenceException
            int i = 0;
            int a = 0;
            int c = 0;

            foreach (PathOperation vType in _segmentTypes)
            {
                if (vType == PathOperation.MOVE_TO)
                {
                    vPath = new EWPath();
                    vPaths.Add(vPath);
                    vPath.MoveTo(_points[i++]);
                }
                else if (vType == PathOperation.LINE)
                {
                    vPath.LineTo(_points[i++]);
                }
                else if (vType == PathOperation.QUAD)
                {
                    vPath.QuadTo(_points[i++], _points[i++]);
                }
                else if (vType == PathOperation.CUBIC)
                {
                    vPath.CurveTo(_points[i++], _points[i++], _points[i++]);
                }
                else if (vType == PathOperation.ARC)
                {
                    vPath.AddArc(_points[i++], _points[i++], _arcAngles[a++], _arcAngles[a++], _arcClockwise[c++]);
                }
                else if (vType == PathOperation.CLOSE)
                {
                    vPath.Close();
                    vPath = null;
                }
            }
            // ReSharper restore PossibleNullReferenceException

            return vPaths;
        }

        public EWPath Reverse()
        {
            var points = new List<EWPoint>(_points);
            points.Reverse();

            var arcSizes = new List<float>(_arcAngles);
            arcSizes.Reverse();

            var arcClockwise = new List<bool>(_arcClockwise);
            arcClockwise.Reverse();

            var operations = new List<PathOperation>(_segmentTypes);
            operations.Reverse();

            bool segmentClosed = false;
            int segmentStart = -1;

            for (int i = 0; i < operations.Count; i++)
            {
                if (operations[i] == PathOperation.MOVE_TO)
                {
                    if (segmentStart == -1)
                    {
                        operations.RemoveAt(i);
                        operations.Insert(0, PathOperation.MOVE_TO);
                    }
                    else if (segmentClosed)
                    {
                        operations[segmentStart] = PathOperation.MOVE_TO;
                        operations[i] = PathOperation.CLOSE;
                    }

                    segmentStart = i + 1;
                }
                else if (operations[i] == PathOperation.CLOSE)
                {
                    segmentStart = i;
                    segmentClosed = true;
                }
            }

            return new EWPath(points, arcSizes, arcClockwise, operations, _subPathCount);
        }

        public void AppendOval(float x, float y, float w, float h)
        {
            float minx = x;
            float miny = y;
            float maxx = minx + w;
            float maxy = miny + h;
            float midx = minx + (w / 2);
            float midy = miny + (h / 2);
            float offsetY = h / 2 * .55f;
            float offsetX = w / 2 * .55f;

            MoveTo(new EWPoint(minx, midy));
            CurveTo(new EWPoint(minx, midy - offsetY), new EWPoint(midx - offsetX, miny), new EWPoint(midx, miny));
            CurveTo(new EWPoint(midx + offsetX, miny), new EWPoint(maxx, midy - offsetY), new EWPoint(maxx, midy));
            CurveTo(new EWPoint(maxx, midy + offsetY), new EWPoint(midx + offsetX, maxy), new EWPoint(midx, maxy));
            CurveTo(new EWPoint(midx - offsetX, maxy), new EWPoint(minx, midy + offsetY), new EWPoint(minx, midy));
            Close();
        }

        public void AppendCircle(float cx, float cy, float r)
        {
            float minx = cx - r;
            float miny = cy - r;
            float maxx = cx + r;
            float maxy = cy + r;
            float midx = cx;
            float midy = cy;
            float offsetY = r * .55f;
            float offsetX = r * .55f;

            MoveTo(new EWPoint(minx, midy));
            CurveTo(new EWPoint(minx, midy - offsetY), new EWPoint(midx - offsetX, miny), new EWPoint(midx, miny));
            CurveTo(new EWPoint(midx + offsetX, miny), new EWPoint(maxx, midy - offsetY), new EWPoint(maxx, midy));
            CurveTo(new EWPoint(maxx, midy + offsetY), new EWPoint(midx + offsetX, maxy), new EWPoint(midx, maxy));
            CurveTo(new EWPoint(midx - offsetX, maxy), new EWPoint(minx, midy + offsetY), new EWPoint(minx, midy));
            Close();
        }

        public void AppendRectangle(float x, float y, float w, float h, bool includeLast = false)
        {
            float minx = x;
            float miny = y;
            float maxx = minx + w;
            float maxy = miny + h;

            MoveTo(new EWPoint(minx, miny));
            LineTo(new EWPoint(maxx, miny));
            LineTo(new EWPoint(maxx, maxy));
            LineTo(new EWPoint(minx, maxy));

            if (includeLast)
            {
                LineTo(new EWPoint(minx, miny));
            }

            Close();
        }

        public void AppendRoundedRectangle(float x, float y, float w, float h, float cornerRadius, bool includeLast = false)
        {
            if (cornerRadius > h / 2)
            {
                cornerRadius = h / 2;
            }

            if (cornerRadius > w / 2)
            {
                cornerRadius = w / 2;
            }

            float minx = x;
            float miny = y;
            float maxx = minx + w;
            float maxy = miny + h;

            float handleOffset = cornerRadius * .55f;
            float cornerOffset = cornerRadius - handleOffset;

            MoveTo(new EWPoint(minx, miny + cornerRadius));
            CurveTo(new EWPoint(minx, miny + cornerOffset), new EWPoint(minx + cornerOffset, miny), new EWPoint(minx + cornerRadius, miny));
            LineTo(new EWPoint(maxx - cornerRadius, miny));
            CurveTo(new EWPoint(maxx - cornerOffset, miny), new EWPoint(maxx, miny + cornerOffset), new EWPoint(maxx, miny + cornerRadius));
            LineTo(new EWPoint(maxx, maxy - cornerRadius));
            CurveTo(new EWPoint(maxx, maxy - cornerOffset), new EWPoint(maxx - cornerOffset, maxy), new EWPoint(maxx - cornerRadius, maxy));
            LineTo(new EWPoint(minx + cornerRadius, maxy));
            CurveTo(new EWPoint(minx + cornerOffset, maxy), new EWPoint(minx, maxy - cornerOffset), new EWPoint(minx, maxy - cornerRadius));

            if (includeLast)
            {
                LineTo(new EWPoint(minx, miny + cornerRadius));
            }

            Close();
        }
        
        public bool IsSubPathClosed(int aSubPathIndex)
        {
            if (aSubPathIndex >= 0 && aSubPathIndex < SubPathCount)
            {
                return _subPathsClosed[aSubPathIndex];
            }

            return false;
        }

        public object NativePath
        {
            get => _nativePath;
            set
            {
                if (_nativePath is IDisposable disposable)
                    disposable.Dispose();

                _nativePath = value;
            }
        }

        public void Invalidate()
        {
            _cachedBounds = null;
            ReleaseNative();
        }

        public void Dispose()
        {
            ReleaseNative();
        }

        private void ReleaseNative()
        {
            if (_nativePath is IDisposable disposable)
                disposable.Dispose();

            _nativePath = null;
        }

        public void Move(float x, float y)
        {
            foreach (var point in _points)
            {
                point.X += x;
                point.Y += y;
            }

            Invalidate();
        }

        public void MovePoint(int index, float dx, float dy)
        {
            _points[index].X += dx;
            _points[index].Y += dy;
            Invalidate();
        }
        
        public override bool Equals(object obj)
        {
            if (obj is EWPath compareTo)
            {
                if (SegmentCount != compareTo.SegmentCount)
                    return false;
                
                for (var i = 0; i < _segmentTypes.Count; i++)
                {
                    var segmentType = _segmentTypes[i];
                    if (segmentType != compareTo.GetSegmentType(i))
                        return false;
                }
                
                for (var i = 0; i < _points.Count; i++)
                {
                    var point = _points[i];
                    if (!point.Equals(compareTo[i], Geometry.Epsilon))
                        return false;
                }

                if (_arcAngles != null)
                {
                    for (var i = 0; i < _arcAngles.Count; i++)
                    {
                        var arcAngle = _arcAngles[i];
                        if (Math.Abs(arcAngle - compareTo.GetArcAngle(i)) > Geometry.Epsilon)
                            return false;
                    }
                }

                if (_arcClockwise != null)
                {
                    for (var i = 0; i < _arcClockwise.Count; i++)
                    {
                        var arcClockwise = _arcClockwise[i];
                        if (arcClockwise != compareTo.GetArcClockwise(i))
                            return false;
                    }
                }
            }

            return true;
        }
        
        public bool Equals(object obj, float epsilon)
        {
            if (obj is EWPath compareTo)
            {
                if (SegmentCount != compareTo.SegmentCount)
                    return false;
                
                for (var i = 0; i < _segmentTypes.Count; i++)
                {
                    var segmentType = _segmentTypes[i];
                    if (segmentType != compareTo.GetSegmentType(i))
                        return false;
                }
                
                for (var i = 0; i < _points.Count; i++)
                {
                    var point = _points[i];
                    if (!point.Equals(compareTo[i], epsilon))
                        return false;
                }

                if (_arcAngles != null)
                {
                    for (var i = 0; i < _arcAngles.Count; i++)
                    {
                        var arcAngle = _arcAngles[i];
                        if (Math.Abs(arcAngle - compareTo.GetArcAngle(i)) > epsilon)
                            return false;
                    }
                }

                if (_arcClockwise != null)
                {
                    for (var i = 0; i < _arcClockwise.Count; i++)
                    {
                        var arcClockwise = _arcClockwise[i];
                        if (arcClockwise != compareTo.GetArcClockwise(i))
                            return false;
                    }
                }
            }

            return true;
        }
    }
}