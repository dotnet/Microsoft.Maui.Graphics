using System;
using SharpDX;
using SharpDX.Direct2D1;
using Xamarin.Graphics;
using Geometry = Xamarin.Graphics.Geometry;

namespace Elevenworks.Graphics.SharpDX
{
    public static class DXExtensions
    {
        public static Color4 AsDxColor(this EWColor aTarget)
        {
            if (aTarget == null)
            {
                return Color.Black;
            }

            return new Color4(aTarget.Components);
        }

        public static Color4 AsDxColor(this EWColor aTarget, float aAlphaMultiplier)
        {
            if (aTarget == null)
            {
                return Color.Black;
            }

            return new Color4(aTarget.Red, aTarget.Green, aTarget.Blue, aTarget.Alpha * aAlphaMultiplier);
        }

        public static PathGeometry AsDxPath(this EWPath target, Factory factory, FillMode fillMode = FillMode.Winding)
        {
            return AsDxPath(target, 1, factory, fillMode);
        }

        public static PathGeometry AsDxPath(this EWPath path, float ppu, Factory factory, FillMode fillMode = FillMode.Winding)
        {
            return AsDxPath(path, ppu, 0, 0, 1, 1, factory, fillMode);
        }

        public static PathGeometry AsDxPath(this EWPath path, float ppu, float ox, float oy, float fx, float fy, Factory factory, FillMode fillMode = FillMode.Winding)
        {
            var geometry = new PathGeometry(factory);

#if DEBUG
            try
            {
#endif

                var sink = geometry.Open();
                sink.SetFillMode(fillMode);

                var ppux = ppu * fx;
                var ppuy = ppu * fy;

                var pointIndex = 0;
                var arcAngleIndex = 0;
                var arcClockwiseIndex = 0;
                var figureOpen = false;
                var segmentIndex = -1;

                var lastOperation = PathOperation.MOVE_TO;

                foreach (var type in path.SegmentTypes)
                {
                    segmentIndex++;

                    if (type == PathOperation.MOVE_TO)
                    {
                        if (lastOperation != PathOperation.CLOSE && lastOperation != PathOperation.MOVE_TO)
                        {
                            sink.EndFigure(FigureEnd.Open);
                            //vPath = vPathGeometry.Open();
                        }

                        var point = path[pointIndex++];
                        /*var vBegin = FigureBegin.Hollow;
                        if (path.IsSubPathClosed(vSegmentIndex))
                        {
                            vBegin = FigureBegin.Filled;
                        }*/
                        var begin = FigureBegin.Filled;
                        sink.BeginFigure(ox + point.X * ppux, oy + point.Y * ppuy, begin);
                        figureOpen = true;
                    }
                    else if (type == PathOperation.LINE)
                    {
                        var point = path[pointIndex++];
                        sink.LineTo(ox + point.X * ppux, oy + point.Y * ppuy);
                    }

                    else if (type == PathOperation.QUAD)
                    {
                        var controlPoint = path[pointIndex++];
                        var endPoint = path[pointIndex++];

                        sink.QuadTo(
                            ox + controlPoint.X * ppux,
                            oy + controlPoint.Y * ppuy,
                            ox + endPoint.X * ppux,
                            oy + endPoint.Y * ppuy);
                    }
                    else if (type == PathOperation.CUBIC)
                    {
                        var controlPoint1 = path[pointIndex++];
                        var controlPoint2 = path[pointIndex++];
                        var endPoint = path[pointIndex++];
                        sink.CubicTo(
                            ox + controlPoint1.X * ppux,
                            oy + controlPoint1.Y * ppuy,
                            ox + controlPoint2.X * ppux,
                            oy + controlPoint2.Y * ppuy,
                            ox + endPoint.X * ppux,
                            oy + endPoint.Y * ppuy);
                    }
                    else if (type == PathOperation.ARC)
                    {
                        var topLeft = path[pointIndex++];
                        var bottomRight = path[pointIndex++];
                        var startAngle = path.GetArcAngle(arcAngleIndex++);
                        var endAngle = path.GetArcAngle(arcAngleIndex++);
                        var clockwise = path.GetArcClockwise(arcClockwiseIndex++);

                        while (startAngle < 0)
                        {
                            startAngle += 360;
                        }

                        while (endAngle < 0)
                        {
                            endAngle += 360;
                        }

                        var rotation = Geometry.GetSweep(startAngle, endAngle, clockwise);
                        var absRotation = Math.Abs(rotation);

                        var rectX = ox + topLeft.X * ppux;
                        var rectY = oy + topLeft.Y * ppuy;
                        var rectWidth = (ox + bottomRight.X * ppux) - rectX;
                        var rectHeight = (oy + bottomRight.Y * ppuy) - rectY;

                        var startPoint = Geometry.OvalAngleToPoint(rectX, rectY, rectWidth, rectHeight, -startAngle);
                        var endPoint = Geometry.OvalAngleToPoint(rectX, rectY, rectWidth, rectHeight, -endAngle);


                        if (!figureOpen)
                        {
                            /*var vBegin = FigureBegin.Hollow;
                            if (path.IsSubPathClosed(vSegmentIndex))
                            {
                                vBegin = FigureBegin.Filled;
                            }*/
                            var begin = FigureBegin.Filled;
                            sink.BeginFigure(startPoint.X, startPoint.Y, begin);
                            figureOpen = true;
                        }
                        else
                        {
                            sink.LineTo(startPoint.X, startPoint.Y);
                        }

                        var arcSegment = new ArcSegment
                        {
                            Point = new Vector2(endPoint.X, endPoint.Y),
                            Size = new Size2F(rectWidth / 2, rectHeight / 2),
                            SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.CounterClockwise,
                            ArcSize = absRotation >= 180 ? ArcSize.Large : ArcSize.Small
                        };
                        sink.AddArc(arcSegment);
                    }
                    else if (type == PathOperation.CLOSE)
                    {
                        sink.EndFigure(FigureEnd.Closed);
                    }

                    lastOperation = type;
                }

                if (segmentIndex >= 0 && lastOperation != PathOperation.CLOSE)
                {
                    sink.EndFigure(FigureEnd.Open);
                }

                sink.Close();

                return geometry;
#if DEBUG
            }
            catch (Exception exc)
            {
                geometry.Dispose();

                var definition = path.ToDefinitionString();
                Logger.Debug(string.Format("Unable to convert the path to a DXPath: {0}", definition), exc);
                return null;
            }
#endif
        }

        public static PathGeometry AsDxPath(this EWPath path, float ppu, float zoom, Factory factory)
        {
            return AsDxPath(path, ppu * zoom, factory);
        }

        public static PathGeometry AsDxPathFromSegment(this EWPath path, int segmentIndex, float ppu, float zoom, Factory factory)
        {
            float scale = ppu / zoom;

            var geometry = new PathGeometry(factory);
            var sink = geometry.Open();

            var type = path.GetSegmentType(segmentIndex);
            if (type == PathOperation.LINE)
            {
                int segmentStartingPointIndex = path.GetSegmentPointIndex(segmentIndex);
                var startPoint = path[segmentStartingPointIndex - 1];
                sink.BeginFigure(startPoint.X * scale, startPoint.Y * scale, FigureBegin.Hollow);

                var point = path[segmentStartingPointIndex];
                sink.LineTo(point.X * scale, point.Y * scale);
            }
            else if (type == PathOperation.QUAD)
            {
                int segmentStartingPointIndex = path.GetSegmentPointIndex(segmentIndex);
                var startPoint = path[segmentStartingPointIndex - 1];
                sink.BeginFigure(startPoint.X * scale, startPoint.Y * scale, FigureBegin.Hollow);

                var controlPoint = path[segmentStartingPointIndex++];
                var endPoint = path[segmentStartingPointIndex];
                sink.QuadTo(
                    controlPoint.X * scale, controlPoint.Y * scale,
                    endPoint.X * scale, endPoint.Y * scale);
            }
            else if (type == PathOperation.CUBIC)
            {
                int segmentStartingPointIndex = path.GetSegmentPointIndex(segmentIndex);
                var startPoint = path[segmentStartingPointIndex - 1];
                sink.BeginFigure(startPoint.X * scale, startPoint.Y * scale, FigureBegin.Hollow);

                var controlPoint1 = path[segmentStartingPointIndex++];
                var controlPoint2 = path[segmentStartingPointIndex++];
                var endPoint = path[segmentStartingPointIndex];
                sink.CubicTo(
                    controlPoint1.X * scale, controlPoint1.Y * scale,
                    controlPoint2.X * scale, controlPoint2.Y * scale,
                    endPoint.X * scale, endPoint.Y * scale);
            }
            else if (type == PathOperation.ARC)
            {
                path.GetSegmentInfo(segmentIndex, out var pointIndex, out var arcAngleIndex, out var arcClockwiseIndex);

                var topLeft = path[pointIndex++];
                var bottomRight = path[pointIndex];
                var startAngle = path.GetArcAngle(arcAngleIndex++);
                var endAngle = path.GetArcAngle(arcAngleIndex);
                var clockwise = path.GetArcClockwise(arcClockwiseIndex);

                while (startAngle < 0)
                {
                    startAngle += 360;
                }

                while (endAngle < 0)
                {
                    endAngle += 360;
                }

                var rotation = Geometry.GetSweep(startAngle, endAngle, clockwise);
                var absRotation = Math.Abs(rotation);

                var rectX = topLeft.X * scale;
                var rectY = topLeft.Y * scale;
                var rectWidth = (bottomRight.X * scale) - rectX;
                var rectHeight = (bottomRight.Y * scale) - rectY;

                var startPoint = Geometry.OvalAngleToPoint(rectX, rectY, rectWidth, rectHeight, -startAngle);
                var endPoint = Geometry.OvalAngleToPoint(rectX, rectY, rectWidth, rectHeight, -endAngle);

                sink.BeginFigure(startPoint.X * scale, startPoint.Y * scale, FigureBegin.Hollow);

                var arcSegment = new ArcSegment
                {
                    Point = new Vector2(endPoint.X, endPoint.Y),
                    Size = new Size2F(rectWidth / 2, rectHeight / 2),
                    SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.CounterClockwise,
                    ArcSize = absRotation >= 180 ? ArcSize.Large : ArcSize.Small
                };
                sink.AddArc(arcSegment);
            }

            sink.EndFigure(FigureEnd.Open);
            sink.Close();

            return geometry;
        }
    }
}