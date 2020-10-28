using System;

namespace Elevenworks.Graphics
{
    public static class BezierLengthUtil
    {
        public static readonly int Steps = 10;

        private static float CalcParametericValue(float t, float start, float control1, float control2, float end)
        {
            /* Formula from Wikipedia article on Bezier curves. */
            return start * (1.0f - t) * (1.0f - t) * (1.0f - t) + 3.0f * control1 * (1.0f - t) * (1.0f - t) * t + 3.0f * control2 * (1.0f - t) * t * t + end * t * t * t;
        }

        public static float ApproximateCubicLength(EWImmutablePoint aStartPoint, EWImmutablePoint aControlPoint1, EWImmutablePoint aControlPoint2, EWImmutablePoint aEndPoint)
        {
            var vPoint = new EWPoint();
            var vPreviewPoint = new EWPoint();
            float vLength = 0.0f;
            for (int i = 0; i <= Steps; i++)
            {
                float t = i / (float) Steps;
                vPoint.X = CalcParametericValue(t, aStartPoint.X, aControlPoint1.X, aControlPoint2.X, aEndPoint.X);
                vPoint.Y = CalcParametericValue(t, aStartPoint.Y, aControlPoint1.Y, aControlPoint2.Y, aEndPoint.Y);
                if (i > 0)
                {
                    float xDiff = vPoint.X - vPreviewPoint.X;
                    float yDiff = vPoint.Y - vPreviewPoint.Y;
                    vLength += (float) Math.Sqrt(xDiff * xDiff + yDiff * yDiff);
                }

                vPreviewPoint.X = vPoint.X;
                vPreviewPoint.Y = vPoint.Y;
            }

            return vLength;
        }

        public static float ApproximateQuadraticLength(EWImmutablePoint p0, EWImmutablePoint p1, EWImmutablePoint p2)
        {
            float aX = p0.X - 2 * p1.X + p2.X;
            float aY = p0.Y - 2 * p1.Y + p2.Y;
            float bX = 2 * p1.X - 2 * p0.X;
            float bY = 2 * p1.Y - 2 * p0.Y;

            float a = 4 * (aX * aX + aY * aY);
            float b = 4 * (aX * bX + aY * bY);
            float c = bX * bX + bY * bY;

            float sabc = 2 * (float) Math.Sqrt(a + b + c);
            float a2 = (float) Math.Sqrt(a);
            float a32 = 2 * a * a2;
            float c2 = 2 * (float) Math.Sqrt(c);
            float ba = b / a2;

            return (a32 * sabc + a2 * b * (sabc - c2) + (4 * c * a - b * b) * (float) Math.Log((2 * a2 + ba + sabc) / (ba + c2))) / (4 * a32);
        }

        public static float GetLength(EWPath aPath)
        {
            if (aPath == null || aPath.Count < 2)
            {
                return 0;
            }

            float vLength = 0;

            var vStartPoint = aPath[0];

            for (int i = 0; i < aPath.SegmentCount; i++)
            {
                PathOperation vSegmentType = aPath.GetSegmentType(i);
                var vPoints = aPath.GetPointsForSegment(i);

                if (vPoints != null && vPoints.Length > 0)
                {
                    var vEndPoint = vPoints[vPoints.Length - 1];
                    switch (vSegmentType)
                    {
                        case PathOperation.ARC:
                            EWPoint vControlPoint1 = Geometry.GetPointAlongLine(vStartPoint, vPoints[0], .55f);
                            EWPoint vControlPoint2 = Geometry.GetPointAlongLine(vEndPoint, vPoints[0], .55f);
                            vLength += ApproximateCubicLength(vStartPoint, vControlPoint1, vControlPoint2, vEndPoint);
                            break;
                        case PathOperation.LINE:
                            vLength += Geometry.GetDistance(vStartPoint, vEndPoint);
                            break;
                        case PathOperation.QUAD:
                            vLength += ApproximateQuadraticLength(vStartPoint, vPoints[0], vEndPoint);
                            break;
                        case PathOperation.CUBIC:
                            vLength += ApproximateCubicLength(vStartPoint, vPoints[0], vPoints[1], vEndPoint);
                            break;
                    }

                    vStartPoint = vEndPoint;
                }
            }

            return vLength;
        }

        public static EWPoint GetPointAlongCurve(EWPath aPath, float t)
        {
            float vApproxLength = GetLength(aPath);
            float vLengthToPoint = vApproxLength * t;
            float vLength = 0;

            var vStartPoint = aPath[0];

            for (int i = 0; i < aPath.SegmentCount; i++)
            {
                PathOperation vSegmentType = aPath.GetSegmentType(i);
                EWImmutablePoint[] vPoints;
                EWImmutablePoint vEndPoint;
                if (vSegmentType == PathOperation.CLOSE)
                {
                    vPoints = new[] {aPath[aPath.Count - 1], aPath[0]};
                    vEndPoint = aPath[0];
                }
                else
                {
                    vPoints = aPath.GetPointsForSegment(i);
                    vEndPoint = vPoints[vPoints.Length - 1];
                }

                float vSegmentLength = 0;

                switch (vSegmentType)
                {
                    case PathOperation.ARC:
                        EWPoint vControlPoint1 = Geometry.GetPointAlongLine(vStartPoint, vPoints[0], .55f);
                        EWPoint vControlPoint2 = Geometry.GetPointAlongLine(vEndPoint, vPoints[0], .55f);
                        vSegmentLength = ApproximateCubicLength(vStartPoint, vControlPoint1, vControlPoint2, vEndPoint);
                        break;
                    case PathOperation.LINE:
                        vSegmentLength = Geometry.GetDistance(vStartPoint, vEndPoint);
                        break;
                    case PathOperation.QUAD:
                        vSegmentLength = ApproximateQuadraticLength(vStartPoint, vPoints[0], vEndPoint);
                        break;
                    case PathOperation.CUBIC:
                        vSegmentLength = ApproximateCubicLength(vStartPoint, vPoints[0], vPoints[1], vEndPoint);
                        break;
                }

                vLength += vSegmentLength;

                if (vLength > vLengthToPoint)
                {
                    float vSegmentStartPos = (vLength - vSegmentLength) / vApproxLength;
                    float vSegmentEndPos = vLength / vApproxLength;
                    float vPositionInSegment = (vSegmentEndPos - t) / (vSegmentEndPos - vSegmentStartPos);
                    vPositionInSegment = 1 - vPositionInSegment;
                    switch (vSegmentType)
                    {
                        case PathOperation.ARC:
                            EWPoint vControlPoint1 = Geometry.GetPointAlongLine(vStartPoint, vPoints[0], .55f);
                            EWPoint vControlPoint2 = Geometry.GetPointAlongLine(vEndPoint, vPoints[0], .55f);
                            return BezierUtil.GetPointOnCubicCurveAt(vPositionInSegment, vStartPoint, vControlPoint1, vControlPoint2, vEndPoint);
                        case PathOperation.LINE:
                            return Geometry.GetPointAlongLine(vStartPoint, vEndPoint, vPositionInSegment);
                        case PathOperation.QUAD:
                            return BezierUtil.GetPointOnQuadCurveAt(vPositionInSegment, vStartPoint, vPoints[0], vEndPoint);
                        case PathOperation.CUBIC:
                            return BezierUtil.GetPointOnCubicCurveAt(vPositionInSegment, vStartPoint, vPoints[0], vPoints[1], vEndPoint);
                    }
                }

                vStartPoint = vEndPoint;
            }

            return Geometry.GetCenter(aPath[0], aPath.LastPoint);
        }

        public static float GetLengthAtPointOnSegment(EWPath aPath, EWPoint aPoint, int aSegment)
        {
            if (aPath == null || aPath.Count < 2)
            {
                return 0;
            }

            float vLength = 0;

            var vStartPoint = aPath[0];

            for (int i = 0; i < aPath.SegmentCount && i <= aSegment; i++)
            {
                PathOperation vSegmentType = aPath.GetSegmentType(i);
                var vPoints = aPath.GetPointsForSegment(i);

                if (i == aSegment)
                {
                    if (vPoints != null && vPoints.Length > 0)
                    {
                        var vEndPoint = vPoints[vPoints.Length - 1];
                        switch (vSegmentType)
                        {
                            case PathOperation.ARC:
                                EWPoint vControlPoint1 = Geometry.GetPointAlongLine(vStartPoint, vPoints[0], .55f);
                                EWPoint vControlPoint2 = Geometry.GetPointAlongLine(vEndPoint, vPoints[0], .55f);
                                float at = BezierUtil.ClosestPointToBezier(new[] {vStartPoint, vControlPoint1, vControlPoint2, vEndPoint}, aPoint);
                                float al = ApproximateCubicLength(vStartPoint, vControlPoint1, vControlPoint2, vEndPoint);
                                vLength += (at * al);
                                break;
                            case PathOperation.LINE:
                                vLength += Geometry.GetDistance(vStartPoint, aPoint);
                                break;
                            case PathOperation.QUAD:
                                float qt = BezierUtil.ClosestPointToBezier(new[] {vStartPoint, vPoints[0], vEndPoint}, aPoint);
                                float ql = ApproximateQuadraticLength(vStartPoint, vPoints[0], vEndPoint);
                                vLength += (qt * ql);
                                break;
                            case PathOperation.CUBIC:
                                float ct = BezierUtil.ClosestPointToBezier(new[] {vStartPoint, vPoints[0], vPoints[0], vEndPoint}, aPoint);
                                float cl = ApproximateCubicLength(vStartPoint, vPoints[0], vPoints[1], vEndPoint);
                                vLength += (ct * cl);
                                break;
                        }

                        vStartPoint = vEndPoint;
                    }
                }
                else
                {
                    if (vPoints != null && vPoints.Length > 0)
                    {
                        var vEndPoint = vPoints[vPoints.Length - 1];
                        switch (vSegmentType)
                        {
                            case PathOperation.ARC:
                                EWPoint vControlPoint1 = Geometry.GetPointAlongLine(vStartPoint, vPoints[0], .55f);
                                EWPoint vControlPoint2 = Geometry.GetPointAlongLine(vEndPoint, vPoints[0], .55f);
                                vLength += ApproximateCubicLength(vStartPoint, vControlPoint1, vControlPoint2, vEndPoint);
                                break;
                            case PathOperation.LINE:
                                vLength += Geometry.GetDistance(vStartPoint, vEndPoint);
                                break;
                            case PathOperation.QUAD:
                                vLength += ApproximateQuadraticLength(vStartPoint, vPoints[0], vEndPoint);
                                break;
                            case PathOperation.CUBIC:
                                vLength += ApproximateCubicLength(vStartPoint, vPoints[0], vPoints[1], vEndPoint);
                                break;
                        }

                        vStartPoint = vEndPoint;
                    }
                }
            }

            return vLength;
        }
    }
}