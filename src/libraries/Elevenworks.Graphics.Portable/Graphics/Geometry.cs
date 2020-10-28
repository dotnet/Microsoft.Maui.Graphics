using System;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public static class Geometry
    {
        public const float Epsilon = 0.0000000001f;

        public static EWPoint GetPointOnArc(
            float x,
            float y,
            float width,
            float height,
            float startAngle,
            float endAngle,
            bool clockwise,
            float percentage)
        {
            var cx = x + (width / 2);
            var cy = y + (height / 2);

            var d = Math.Max(width, height);
            var d2 = d / 2;
            var fx = width / d;
            var fy = height / d;

            var sweep = Math.Abs(endAngle - startAngle);
            if (clockwise)
                sweep *= -1;

            var angle = startAngle + (sweep * percentage);

            while (angle >= 360)
                angle -= 360;

            angle *= -1;

            var radians = (float) DegreesToRadians(angle);
            var point = GetPointAtAngle(0, 0, d2, radians);
            point.X = cx + (point.X * fx);
            point.Y = cy + (point.Y * fy);

            return point;
        }

        public static EWPoint GetPointOnOval(
            float x,
            float y,
            float width,
            float height,
            float angle)
        {
            var cx = x + (width / 2);
            var cy = y + (height / 2);

            var d = Math.Max(width, height);
            var d2 = d / 2;
            var fx = width / d;
            var fy = height / d;

            while (angle >= 360)
                angle -= 360;

            angle *= -1;

            var radians = (float) DegreesToRadians(angle);
            var point = GetPointAtAngle(0, 0, d2, radians);
            point.X = cx + (point.X * fx);
            point.Y = cy + (point.Y * fy);

            return point;
        }

        public static EWRectangle GetBoundsOfArc(
            float x,
            float y,
            float width,
            float height,
            float angle1,
            float angle2,
            bool clockwise)
        {
            var x1 = x;
            var y1 = y;
            var x2 = x + width;
            var y2 = y + height;

            var point1 = GetPointOnOval(x, y, width, height, angle1);
            var point2 = GetPointOnOval(x, y, width, height, angle2);
            var center = new EWPoint(x + width / 2, y + height / 2);

            var startAngle = GetAngleAsDegrees(center, point1);
            var endAngle = GetAngleAsDegrees(center, point2);

            var startAngleRadians = DegreesToRadians(startAngle);
            var endAngleRadians = DegreesToRadians(endAngle);

            var quadrant1 = GetQuadrant(startAngleRadians);
            var quadrant2 = GetQuadrant(endAngleRadians);

            if (quadrant1 == quadrant2)
            {
                if (clockwise)
                {
                    if (((quadrant1 == 1 || quadrant1 == 2) && (point1.X < point2.X)) || ((quadrant1 == 3 || quadrant1 == 4) && (point1.X > point2.X)))
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y1 = Math.Min(point1.Y, point2.Y);
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                }
                else
                {
                    if (((quadrant1 == 1 || quadrant1 == 2) && (point1.X > point2.X)) || ((quadrant1 == 3 || quadrant1 == 4) && (point1.X < point2.X)))
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y1 = Math.Min(point1.Y, point2.Y);
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                }
            }
            else if (quadrant1 == 1)
            {
                if (clockwise)
                {
                    if (quadrant2 == 4)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 3)
                    {
                        y1 = Math.Min(point1.Y, point2.Y);
                        x1 = Math.Min(point1.X, point2.X);
                    }
                    else if (quadrant2 == 2)
                    {
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                }
                else
                {
                    if (quadrant2 == 2)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 3)
                    {
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 4)
                    {
                        x2 = Math.Max(point1.X, point2.X);
                    }
                }
            }
            else if (quadrant1 == 2)
            {
                if (clockwise)
                {
                    if (quadrant2 == 1)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 4)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 3)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                    }
                }
                else
                {
                    if (quadrant2 == 3)
                    {
                        y1 = Math.Min(point1.Y, point2.Y);
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 4)
                    {
                        y1 = Math.Min(point1.Y, point2.Y);
                        x2 = Math.Max(point1.X, point2.X);
                    }
                    else if (quadrant2 == 1)
                    {
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                }
            }
            else if (quadrant1 == 3)
            {
                if (clockwise)
                {
                    if (quadrant2 == 2)
                    {
                        y1 = Math.Min(point1.Y, point2.Y);
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 1)
                    {
                        x2 = Math.Max(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 4)
                    {
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                }
                else
                {
                    if (quadrant2 == 4)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        x2 = Math.Max(point1.X, point2.X);
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 1)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 2)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                    }
                }
            }
            else if (quadrant1 == 4)
            {
                if (clockwise)
                {
                    if (quadrant2 == 3)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        x2 = Math.Max(point1.X, point2.X);
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 2)
                    {
                        x2 = Math.Max(point1.X, point2.X);
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 1)
                    {
                        x2 = Math.Max(point1.X, point2.X);
                    }
                }
                else
                {
                    if (quadrant2 == 1)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                        y1 = Math.Min(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 2)
                    {
                        x1 = Math.Min(point1.X, point2.X);
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                    else if (quadrant2 == 3)
                    {
                        y2 = Math.Max(point1.Y, point2.Y);
                    }
                }
            }

            return new EWRectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public static byte GetQuadrant(double radians)
        {
            var trueAngle = radians % (2 * Math.PI);
            if (trueAngle >= 0.0 && trueAngle < Math.PI / 2.0)
                return 1;
            if (trueAngle >= Math.PI / 2.0 && trueAngle < Math.PI)
                return 2;
            if (trueAngle >= Math.PI && trueAngle < Math.PI * 3.0 / 2.0)
                return 3;
            if (trueAngle >= Math.PI * 3.0 / 2.0 && trueAngle < Math.PI * 2)
                return 4;
            return 0;
        }
        
        public static float GetDistance(EWImmutablePoint point1, EWImmutablePoint point2)
        {
            if (point1 == null || point2 == null)
            {
                return 0;
            }

            var a = point2.X - point1.X;
            var b = point2.Y - point1.Y;

            return (float) Math.Sqrt(a * a + b * b);
        }

        public static float GetDistance(float x1, float y1, float x2, float y2)
        {
            var a = x2 - x1;
            var b = y2 - y1;

            return (float) Math.Sqrt(a * a + b * b);
        }

        public static float GetAngleAsDegrees(EWImmutablePoint point1, EWImmutablePoint point2)
        {
            try
            {
                var dx = point1.X - point2.X;
                var dy = point1.Y - point2.Y;

                var radians = (float) Math.Atan2(dy, dx);
                var degrees = radians * 180.0f / (float) Math.PI;

                return 180 - degrees;
            }
            catch (Exception exc)
            {
                Logger.Warn(exc);
                throw new Exception("Exception in GetAngleAsDegrees", exc);
            }
        }

        public static float GetAngleAsDegrees(float x1, float y1, float x2, float y2)
        {
            try
            {
                var dx = x1 - x2;
                var dy = y1 - y2;

                var radians = (float) Math.Atan2(dy, dx);
                var degrees = radians * 180.0f / (float) Math.PI;

                return 180 - degrees;
            }
            catch (Exception exc)
            {
                Logger.Warn(exc);
                throw new Exception("Exception in GetAngleAsDegrees", exc);
            }
        }

        public static bool AngleIsBetweenTwoAnglesInclusive(float startAngle, float endAngle, float angle, bool clockwise)
        {
            if (clockwise)
            {
                if (endAngle > startAngle)
                {
                    if (angle < startAngle)
                    {
                        return true;
                    }
                    else if (angle > endAngle)
                    {
                        return true;
                    }
                }
                else if (angle <= startAngle && angle >= endAngle)
                {
                    return true;
                }
            }
            else
            {
                if (startAngle > endAngle)
                {
                    if (angle > startAngle)
                    {
                        return true;
                    }
                    else if (angle < endAngle)
                    {
                        return true;
                    }
                }
                else if (angle >= startAngle && angle <= endAngle)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool AngleIsBetweenTwoAnglesExclusive(float startAngle, float endAngle, float angle, bool clockwise)
        {
            if (clockwise)
            {
                if (endAngle > startAngle)
                {
                    if (angle < startAngle)
                    {
                        return true;
                    }
                    else if (angle > endAngle)
                    {
                        return true;
                    }
                }
                else if (angle < startAngle && angle > endAngle)
                {
                    return true;
                }
            }
            else
            {
                if (startAngle > endAngle)
                {
                    if (angle > startAngle)
                    {
                        return true;
                    }
                    else if (angle < endAngle)
                    {
                        return true;
                    }
                }
                else if (angle > startAngle && angle < endAngle)
                {
                    return true;
                }
            }

            return false;
        }

        public static float DegreesToRadians(float aAngle)
        {
            return (float) Math.PI * aAngle / 180;
        }

        public static double DegreesToRadians(double aAngle)
        {
            return Math.PI * aAngle / 180;
        }

        public static float RadiansToDegrees(float aAngle)
        {
            return aAngle * (180 / (float) Math.PI);
        }

        public static double RadiansToDegrees(double aAngle)
        {
            return aAngle * (180 / Math.PI);
        }

        public static int GetPointNearest(EWPoint aReference, EWPoint[] aPoints)
        {
            if (aReference == null)
            {
                return 0;
            }

            var vIndex = 0;
            var vDistance = GetDistance(aReference, aPoints[0]);

            for (var i = 1; i < aPoints.Length; i++)
            {
                if (aPoints[i] != null)
                {
                    var vDistance2 = GetDistance(aReference, aPoints[i]);
                    if (vDistance2 < vDistance)
                    {
                        vIndex = i;
                        vDistance = vDistance2;
                    }
                }
            }

            return vIndex;
        }

        public static int GetPointNearestButNotEqualTo(EWPoint aReference, EWPoint[] aPoints)
        {
            var vIndex = -1;

            var vDistance = float.MaxValue;
            for (var i = 0; i < aPoints.Length; i++)
            {
                if (!aPoints[i].Equals(aReference))

                {
                    var vDistance2 = GetDistance(aReference, aPoints[i]);
                    if (vDistance2 < vDistance)
                    {
                        vIndex = i;
                        vDistance = vDistance2;
                    }
                }
            }

            return vIndex;
        }

        public static EWPoint NearestPointOnLine(
            float ax,
            float ay,
            float bx,
            float by,
            float px,
            float py,
            bool clampToSegment)
        {
            var apx = px - ax;
            var apy = py - ay;
            var abx = bx - ax;
            var aby = by - ay;

            var ab2 = abx * abx + aby * aby;
            var ap_ab = apx * abx + apy * aby;
            var t = ap_ab / ab2;
            if (clampToSegment)
            {
                if (t < 0)
                {
                    t = 0;
                }
                else if (t > 1)
                {
                    t = 1;
                }
            }

            return new EWPoint(ax + abx * t, ay + aby * t);
        }

        public static void TranslatePoint(EWPoint aPoint, TranslationData aData)
        {
            var px = aPoint.X - aData.xOrigin;
            var py = aPoint.Y - aData.yOrigin;
            var dx = px / aData.xFactor - px;
            var dy = py / aData.yFactor - py;

            aPoint.X = aPoint.X + dx + aData.xOffset;
            aPoint.Y = aPoint.Y + dy + aData.yOffset;
        }

        public static EWPoint RotatePoint(EWImmutablePoint point, float angle)
        {
            var radians = DegreesToRadians(angle);

            var x = (float) (Math.Cos(radians) * point.X - Math.Sin(radians) * point.Y);
            var y = (float) (Math.Sin(radians) * point.X + Math.Cos(radians) * point.Y);

            return new EWPoint(x, y);
        }

        public static EWPoint RotatePoint(EWImmutablePoint center, EWImmutablePoint point, float angle)
        {
            var radians = DegreesToRadians(angle);
            var x = center.X + (float) (Math.Cos(radians) * (point.X - center.X) - Math.Sin(radians) * (point.Y - center.Y));
            var y = center.Y + (float) (Math.Sin(radians) * (point.X - center.X) + Math.Cos(radians) * (point.Y - center.Y));
            return new EWPoint(x, y);
        }

        public static float GetSweep(float angle1, float angle2, bool clockwise)
        {
            if (clockwise)
            {
                if (angle2 > angle1)
                {
                    return angle1 + (360 - angle2);
                }
                else
                {
                    return angle1 - angle2;
                }
            }
            else
            {
                if (angle1 > angle2)
                {
                    return angle2 + (360 - angle1);
                }
                else
                {
                    return angle2 - angle1;
                }
            }
        }

        public static EWPoint GetCenter(EWImmutablePoint aPoint1, EWImmutablePoint aPoint2)
        {
            var x = (aPoint1.X + aPoint2.X) / 2;
            var y = (aPoint1.Y + aPoint2.Y) / 2;
            return new EWPoint(x, y);
        }

        public static EWPoint GetCenter(float x1, float y1, float x2, float y2)
        {
            var x = (x1 + x2) / 2;
            var y = (y1 + y2) / 2;
            return new EWPoint(x, y);
        }

        public static EWPoint GetPointAlongLine(EWImmutablePoint aPoint1, EWImmutablePoint aPoint2, float aFactor)
        {
            var dx = aPoint2.X - aPoint1.X;
            var dy = aPoint2.Y - aPoint1.Y;

            dx *= aFactor;
            dy *= aFactor;

            return new EWPoint(aPoint1.X + dx, aPoint1.Y + dy);
        }

        public static EWPoint GetPointAlongLine(float x1, float y1, float x2, float y2, float factor)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;

            dx *= factor;
            dy *= factor;

            return new EWPoint(x1 + dx, y1 + dy);
        }

        public static TranslationData CreateTranslationData(
            EWRectangle currentBounds,
            EWRectangle newBounds)
        {
            var data = new TranslationData();

            var newWidth = Math.Abs(newBounds.Width);
            var newHeight = Math.Abs(newBounds.Height);

            data.xFactor = 1;
            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (newWidth != 0)
            {
                data.xFactor = Math.Abs(currentBounds.Width) / newWidth;
            }

            data.yFactor = 1;
            if (newHeight != 0)
            {
                data.yFactor = Math.Abs(currentBounds.Height) / newHeight;
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            data.xOrigin = currentBounds.MinX;
            data.yOrigin = currentBounds.MinY;
            data.xOffset = newBounds.MinX - currentBounds.MinX;
            data.yOffset = newBounds.MinY - currentBounds.MinY;

            return data;
        }

        public static EWPoint GetPointAtAngleInDegrees(float x, float y, float distance, float degrees)
        {
            var radians = DegreesToRadians(degrees);
            return GetPointAtAngle(x, y, distance, radians);
        }

        public static EWPoint GetPointAtAngle(float x, float y, float distance, float radians)
        {
            var x2 = x + (Math.Cos(radians) * distance);
            var y2 = y + (Math.Sin(radians) * distance);
            return new EWPoint((float) x2, (float) y2);
        }

        /* todo: this is not calculating correctly for rectangles.  Only squares */

        public static EWPoint PointAtAngle(float aX, float aY, float aWidth, float aHeight, float aAngle)
        {
            var vAngle = DegreesToRadians(aAngle);

            var sin = (float) Math.Sin(vAngle);
            var cos = (float) Math.Cos(vAngle);
            const float e = 0.0001f;

            float x = 0, y = 0;
            if (Math.Abs(sin) > e)
            {
                x = ((1.0f + cos / Math.Abs(sin)) / 2.0f * aWidth);
                x = Range(0, aWidth, x);
            }

            else if (cos >= 0.0)
            {
                x = aWidth;
            }

            if (Math.Abs(cos) > e)
            {
                y = ((1.0f + sin / Math.Abs(cos)) / 2.0f * aHeight);
                y = Range(0, aHeight, y);
            }

            else if (sin >= 0.0)
            {
                y = aHeight;
            }

            return new EWPoint(aX + x, aY + y);
        }

        public static EWPoint GetPointWhereLineIntersectsLine(EWLine aLine1, EWLine aLine2)
        {
            return GetPointWhereLineIntersectsLine(aLine1.Point1, aLine1.Point2, aLine2.Point1, aLine2.Point2);
        }

        public static EWPoint GetPointWhereLineIntersectsLine(
            EWImmutablePoint aLine1Point1,
            EWImmutablePoint aLine1Point2,
            EWImmutablePoint aLine2Point1,
            EWImmutablePoint aLine2Point2)
        {
            return GetPointWhereLineIntersectsLine(
                aLine1Point1.X,
                aLine1Point1.Y,
                aLine1Point2.X,
                aLine1Point2.Y,
                aLine2Point1.X,
                aLine2Point1.Y,
                aLine2Point2.X,
                aLine2Point2.Y);
        }

        public static EWPoint GetPointWhereLineIntersectsLineOrEndpoints(
            EWImmutablePoint aLine1Point1,
            EWImmutablePoint aLine1Point2,
            EWImmutablePoint aLine2Point1,
            EWImmutablePoint aLine2Point2)
        {
            return GetPointWhereLineIntersectsLineOrEndpoints(
                aLine1Point1.X,
                aLine1Point1.Y,
                aLine1Point2.X,
                aLine1Point2.Y,
                aLine2Point1.X,
                aLine2Point1.Y,
                aLine2Point2.X,
                aLine2Point2.Y);
        }

        public static EWPoint GetPointWhereLineIntersectsLine(float l1x1, float l1y1, float l1x2, float l1y2, float l2x1, float l2y1, float l2x2, float l2y2)
        {
            if (!IsLineIntersectingLine(l1x1, l1y1, l1x2, l1y2, l2x1, l2y1, l2x2, l2y2))
            {
                return null;
            }

            float px = l1x1, py = l1y1, rx = l1x2 - px, ry = l1y2 - py;
            float qx = l2x1, qy = l2y1, sx = l2x2 - qx, sy = l2y2 - qy;

            var det = sx * ry - sy * rx;
            if (Math.Abs(det - 0) < Epsilon)
            {
                return null;
            }

            var z = (sx * (qy - py) + sy * (px - qx)) / det;
            if (Math.Abs(z - 0) < Epsilon || Math.Abs(z - 1) < Epsilon)
            {
                return null;
            }

            // intersection at end point!
            return new EWPoint(px + z * rx, py + z * ry);
        }

        public static EWPoint GetPointWhereLineIntersectsLineOrEndpoints(
            float l1x1,
            float l1y1,
            float l1x2,
            float l1y2,
            float l2x1,
            float l2y1,
            float l2x2,
            float l2y2)
        {
            if (!IsLineIntersectingLine(l1x1, l1y1, l1x2, l1y2, l2x1, l2y1, l2x2, l2y2))
            {
                return null;
            }

            float px = l1x1, py = l1y1, rx = l1x2 - px, ry = l1y2 - py;
            float qx = l2x1, qy = l2y1, sx = l2x2 - qx, sy = l2y2 - qy;

            var det = sx * ry - sy * rx;
            if (Math.Abs(det - 0) < Epsilon)
            {
                return null;
            }

            var z = (sx * (qy - py) + sy * (px - qx)) / det;
            if (Math.Abs(z - 0) < Epsilon || Math.Abs(z - 1) < Epsilon)
            {
                if (Math.Abs(z - 0) < Epsilon)
                {
                    return new EWPoint(l1x1, l1y1);
                }

                return new EWPoint(l1x2, l1y2);
            }

            // intersection at end point!
            return new EWPoint(px + z * rx, py + z * ry);
        }

        public static EWPoint GetPointWhereLineIntersectsPath(EWLine vLine, EWPath vPath, out int aSegment)
        {
            if (vLine == null)
            {
                aSegment = -1;
                return null;
            }

            var vPoint1 = vLine.Point1;
            var vPoint2 = vLine.Point2;

            if (vPath != null)
            {
                //int vArcIndex = 0;
                var vIndex = 0;
                EWImmutablePoint vSegmentStart = null;
                EWImmutablePoint vSegmentEnd = null;

                var vDiscoveredPoints = new List<EWPoint>(4);
                var vDiscoveredSegments = new List<int>(4);

                for (var s = 0; s < vPath.SegmentCount; s++)
                {
                    var vSegmentType = vPath.GetSegmentType(s);

                    if (vSegmentType == PathOperation.MOVE_TO)
                    {
                        vSegmentEnd = vPath[vIndex++];
                    }
                    else if (vSegmentType == PathOperation.LINE)
                    {
                        vSegmentEnd = vPath[vIndex++];
                        var vLineIntersectionPoint = GetPointWhereLineIntersectsLine(
                            vPoint1,
                            vPoint2,
                            vSegmentStart,
                            vSegmentEnd);
                        if (vLineIntersectionPoint != null)
                        {
                            vDiscoveredPoints.Add(vLineIntersectionPoint);
                            vDiscoveredSegments.Add(s);
                        }
                    }
                    else if (vSegmentType == PathOperation.QUAD)
                    {
                        var vQuadControlPoint = vPath[vIndex++];
                        vSegmentEnd = vPath[vIndex++];
                        var vIntersectionPoints = GetPointsWhereLineIntersectsQuadCurve(
                            vPoint1,
                            vPoint2,
                            vSegmentStart,
                            vQuadControlPoint,
                            vSegmentEnd);

                        if (vIntersectionPoints != null)
                        {
                            foreach (var vPoint in vIntersectionPoints)
                            {
                                vDiscoveredPoints.Add(vPoint);
                                vDiscoveredSegments.Add(s);
                            }
                        }
                    }
                    else if (vSegmentType == PathOperation.CUBIC)
                    {
                        var vBezierControlPoint1 = vPath[vIndex++];
                        var vBezierControlPoint2 = vPath[vIndex++];
                        vSegmentEnd = vPath[vIndex++];
                        var vIntersectionPoints = GetPointsWhereLineIntersectsCubicCurve(
                            vPoint1,
                            vPoint2,
                            vSegmentStart,
                            vBezierControlPoint1,
                            vBezierControlPoint2,
                            vSegmentEnd);

                        if (vIntersectionPoints != null)
                        {
                            foreach (var vPoint in vIntersectionPoints)
                            {
                                vDiscoveredPoints.Add(vPoint);
                                vDiscoveredSegments.Add(s);
                            }
                        }
                    }
                    else if (vSegmentType == PathOperation.ARC)
                    {
                        vIndex += 2;
                        //EWPoint vArcControlPoint = vPath[vIndex++];
                        //vSegmentEnd = vPath[vIndex++];
                        //float vArcSize = vPath.GetArcSize (vArcIndex++);
                    }

                    vSegmentStart = vSegmentEnd;
                }

                if (vPath.Closed)
                {
                    // Check the "closing" line
                    vSegmentStart = vPath[0];
                    vSegmentEnd = vPath[vPath.Count - 1];

                    if (!vSegmentStart.Equals(vSegmentEnd))
                    {
                        var vClosingLineIntersection = GetPointWhereLineIntersectsLine(
                            vPoint1,
                            vPoint2,
                            vSegmentStart,
                            vSegmentEnd);
                        if (vClosingLineIntersection != null)
                        {
                            vDiscoveredPoints.Add(vClosingLineIntersection);
                            vDiscoveredSegments.Add(vPath.SegmentCount);
                        }
                    }
                }

                if (vDiscoveredPoints.Count > 0)
                {
                    var vIntersectionPoint = vDiscoveredPoints[0];
                    var vSegment = vDiscoveredSegments[0];

                    for (var i = 1; i < vDiscoveredPoints.Count; i++)
                    {
                        var vCurLength = GetDistance(vIntersectionPoint, vPoint2);
                        var vThisLength = GetDistance(vDiscoveredPoints[i], vPoint2);

                        if (vThisLength < vCurLength)
                        {
                            vIntersectionPoint = vDiscoveredPoints[i];
                            vSegment = vDiscoveredSegments[i];
                        }
                    }

                    aSegment = vSegment;
                    return vIntersectionPoint;
                }
            }

            aSegment = -1;
            return null;
        }

        public static List<EWPoint> GetPointsWhereLineIntersectsPath(EWImmutablePoint point1, EWImmutablePoint point2, EWPath path)
        {
            return GetPointsWhereLineIntersectsPath(point1, point2, path, out _);
        }

        public static List<EWPoint> GetPointsWhereLineIntersectsPath(
            EWImmutablePoint point1,
            EWImmutablePoint point2,
            EWPath path,
            out List<int> segments)
        {
            if (point1 == null || point2 == null || path == null)
            {
                segments = null;
                return null;
            }

            var index = 0;
            EWImmutablePoint segmentStart = null;
            EWImmutablePoint segmentEnd = null;

            var discoveredPoints = new List<EWPoint>();
            segments = new List<int>();

            for (var s = 0; s < path.SegmentCount; s++)
            {
                var segmentType = path.GetSegmentType(s);

                if (segmentType == PathOperation.MOVE_TO)
                {
                    segmentEnd = path[index++];
                }
                else if (segmentType == PathOperation.LINE)
                {
                    segmentEnd = path[index++];
                    var vLineIntersectionPoint = GetPointWhereLineIntersectsLineOrEndpoints(
                        point1,
                        point2,
                        segmentStart,
                        segmentEnd);
                    if (vLineIntersectionPoint != null)
                    {
                        discoveredPoints.Add(vLineIntersectionPoint);
                        segments.Add(s);
                    }
                }
                else if (segmentType == PathOperation.QUAD)
                {
                    var vQuadControlPoint = path[index++];
                    segmentEnd = path[index++];
                    var vIntersectionPoints = GetPointsWhereLineIntersectsQuadCurve(
                        point1,
                        point2,
                        segmentStart,
                        vQuadControlPoint,
                        segmentEnd);

                    if (vIntersectionPoints != null)
                    {
                        foreach (var vIntersectionPoint in vIntersectionPoints)
                        {
                            discoveredPoints.Add(vIntersectionPoint);
                            segments.Add(s);
                        }
                    }
                }
                else if (segmentType == PathOperation.CUBIC)
                {
                    var vBezierControlPoint1 = path[index++];
                    var vBezierControlPoint2 = path[index++];
                    segmentEnd = path[index++];
                    var vIntersectionPoints = GetPointsWhereLineIntersectsCubicCurve(
                        point1,
                        point2,
                        segmentStart,
                        vBezierControlPoint1,
                        vBezierControlPoint2,
                        segmentEnd);

                    if (vIntersectionPoints != null)
                    {
                        foreach (var vIntersectionPoint in vIntersectionPoints)
                        {
                            discoveredPoints.Add(vIntersectionPoint);
                            segments.Add(s);
                        }
                    }
                }
                else if (segmentType == PathOperation.ARC)
                {
                    index += 2;
                    //EWPoint vArcControlPoint = vPath[vIndex++];
                    //vSegmentEnd = vPath[vIndex++];
                    //float vArcSize = vPath.GetArcSize (vArcIndex++);
                }

                segmentStart = segmentEnd;
            }

            if (path.Closed)
            {
                // Check the "closing" line
                segmentStart = path[0];
                segmentEnd = path[path.Count - 1];

                if (!segmentStart.Equals(segmentEnd))
                {
                    var vClosingLineIntersection = GetPointWhereLineIntersectsLine(
                        point1,
                        point2,
                        segmentStart,
                        segmentEnd);
                    if (vClosingLineIntersection != null)
                    {
                        discoveredPoints.Add(vClosingLineIntersection);
                        segments.Add(path.SegmentCount);
                    }
                }
            }

            RemoveDuplicates(discoveredPoints, segments, null);

            return discoveredPoints;
        }

        private static void RemoveDuplicates(
            List<EWPoint> vDiscoveredPoints,
            List<int> vDiscoveredSegments1,
            List<int> vDiscoveredSegments2)
        {
            for (var i = 0; i < vDiscoveredPoints.Count - 1; i++)
            {
                var vPoint = vDiscoveredPoints[i];
                for (var j = 1; j < vDiscoveredPoints.Count; j++)
                {
                    var vCompareTo = vDiscoveredPoints[j];
                    if (vPoint.Equals(vCompareTo) || GetDistance(vPoint, vCompareTo) < .01)
                    {
                        vDiscoveredPoints.RemoveAt(j);
                        if (vDiscoveredSegments1 != null)
                        {
                            vDiscoveredSegments1.RemoveAt(j);
                        }

                        if (vDiscoveredSegments2 != null)
                        {
                            vDiscoveredSegments2.RemoveAt(j);
                        }

                        j--;
                    }
                }
            }
        }

        /**
        * Constains a value to the given range.
        * @return the constrained value
        */

        private static float Range(float min, float max, float value)
        {
            if (value < min)
            {
                value = min;
            }

            if (value > max)
            {
                value = max;
            }

            return value;
        }

        /**
     * Converts a polar to a point
     */

        public static EWPoint PolarToPoint(float angleInRadians, float fx, float fy)
        {
            var sin = (float) Math.Sin(angleInRadians);
            var cos = (float) Math.Cos(angleInRadians);
            return new EWPoint(fx * cos, fy * sin);
        }


        /// <summary>
        /// Gets the point on an oval that corresponds to the given angle.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="x">The x position of the bounding rectangle.</param>
        /// <param name="y">The y position of the bounding rectangle.</param>
        /// <param name="width">The width of the bounding rectangle.</param>
        /// <param name="height">The height of the bounding rectangle.</param>
        /// <param name="angleInDegrees">Angle in degrees.</param>
        public static EWPoint OvalAngleToPoint(float x, float y, float width, float height, float angleInDegrees)
        {
            var radians = DegreesToRadians(angleInDegrees);

            var cx = x + width / 2;
            var cy = y + height / 2;

            var point = PolarToPoint(radians, width / 2, height / 2);

            point.X += cx;
            point.Y += cy;
            return point;
        }

        public static bool IsOppositeWithinTolerance(
            EWImmutablePoint aCenter,
            EWImmutablePoint aPoint,
            EWImmutablePoint aOppositePoint,
            float aTolerance)
        {
            var vCurrentAngleOfOppositePoint = GetAngleAsDegrees(aCenter, aOppositePoint);
            var vCurrentOppositeAngle = GetOppositeAngle(aCenter, aPoint);
            if (Math.Abs(vCurrentOppositeAngle - vCurrentAngleOfOppositePoint) < aTolerance)
            {
                return true;
            }

            return false;
        }

        public static EWImmutablePoint GetOppositePointMaintainingLengthIfWithinTolerance(
            EWImmutablePoint aCenter,
            EWImmutablePoint aPoint,
            EWImmutablePoint aOppositePoint,
            float aTolerance)
        {
            var vCurrentAngleOfOppositePoint = GetAngleAsDegrees(aCenter, aOppositePoint);
            var vCurrentOppositeAngle = GetOppositeAngle(aCenter, aPoint);
            if (Math.Abs(vCurrentOppositeAngle - vCurrentAngleOfOppositePoint) < aTolerance)
            {
                return GetOppositePointMaintainingLength(aCenter, aPoint, aOppositePoint);
            }

            return aOppositePoint;
        }

        public static EWPoint GetOppositePointMaintainingLength(EWImmutablePoint aCenter, EWImmutablePoint aPoint, EWImmutablePoint aOppositePoint)
        {
            var vDistance = GetDistance(aCenter, aOppositePoint);
            var vAngle = GetOppositeAngle(aCenter, aPoint);
            var vNewPoint = new EWPoint(aCenter.X + vDistance, aCenter.Y);
            vNewPoint = RotatePoint(aCenter, vNewPoint, -vAngle);

            return vNewPoint;
        }

        public static EWPoint GetOppositePoint(EWImmutablePoint aPivot, EWImmutablePoint aOppositePoint)
        {
            var dx = aOppositePoint.X - aPivot.X;
            var dy = aOppositePoint.Y - aPivot.Y;
            return new EWPoint(aPivot.X - dx, aPivot.Y - dy);
        }

        public static float GetOppositeAngle(EWImmutablePoint aCenter, EWImmutablePoint aPoint)
        {
            var vAngle = GetAngleAsDegrees(aCenter, aPoint);
            vAngle += 180;
            if (vAngle >= 360)
            {
                vAngle -= 360;
            }

            return vAngle;
        }

        public static EWPoint GetEquadistantOppositePoint(EWImmutablePoint aCenter, EWImmutablePoint aPoint)
        {
            var dx = aPoint.X - aCenter.X;
            var dy = aPoint.Y - aCenter.Y;
            return new EWPoint(aCenter.X - dx, aCenter.Y - dy);
        }

        /**
       * Return true if c is between a and b.
        */

        private static bool IsBetween(float a, float b, float c)
        {
            return b > a ? c >= a && c <= b : c >= b && c <= a;
        }

        /**
         * Check if two points are on the same side of a given line.
         * Algorithm from Sedgewick page 350.
         * 
         * @param x0, y0, x1, y1  The line.
         * @param px0, py0        First point.
         * @param px1, py1        Second point.
         * @return                <0 if points on opposite sides.
         *                        =0 if one of the points is exactly on the line
         *                        >0 if points on same side.
         */

        private static int SameSide(float x0, float y0, float x1, float y1, float px0, float py0, float px1, float py1)
        {
            var sameSide = 0;

            var dx = x1 - x0;
            var dy = y1 - y0;
            var dx1 = px0 - x0;
            var dy1 = py0 - y0;
            var dx2 = px1 - x1;
            var dy2 = py1 - y1;

            // Cross product of the vector from the endpoint of the line to the point
            var c1 = dx * dy1 - dy * dx1;
            var c2 = dx * dy2 - dy * dx2;

            // ReSharper disable CompareOfFloatsByEqualityOperator
            if (c1 != 0 && c2 != 0)
            {
                sameSide = c1 < 0 != c2 < 0 ? -1 : 1;
            }
            else if (dx == 0 && dx1 == 0 && dx2 == 0)
            {
                sameSide = !IsBetween(y0, y1, py0) && !IsBetween(y0, y1, py1) ? 1 : 0;
            }
            else if (dy == 0 && dy1 == 0 && dy2 == 0)
            {
                sameSide = !IsBetween(x0, x1, px0) && !IsBetween(x0, x1, px1) ? 1 : 0;
            }
            // ReSharper restore CompareOfFloatsByEqualityOperator

            return sameSide;
        }

        /**
         * Check if two line segments intersects. Integer domain.
         * 
         * @param x0, y0, x1, y1  End points of first line to check.
         * @param x2, yy, x3, y3  End points of second line to check.
         * @return                True if the two lines intersects.
         */

        public static bool IsLineIntersectingLine(
            float x0,
            float y0,
            float x1,
            float y1,
            float x2,
            float y2,
            float x3,
            float y3)
        {
            var s1 = SameSide(x0, y0, x1, y1, x2, y2, x3, y3);
            var s2 = SameSide(x2, y2, x3, y3, x0, y0, x1, y1);

            return s1 <= 0 && s2 <= 0;
        }

        /**
        * Caps the line defined by p1 and p2 by the number of units
        * specified by radius.
        * @return A new end point for the line.
        */

        public static EWPoint AdjustLineLength(EWImmutablePoint aPoint1, EWImmutablePoint aPoint2, float aLength)
        {
            var vAngleInRadians = (float) Math.PI / 2 - (float) Math.Atan2(aPoint2.X - aPoint1.X, aPoint2.Y - aPoint1.Y);
            return new EWPoint(
                aPoint2.X + aLength * (float) Math.Cos(vAngleInRadians),
                aPoint2.Y + aLength * (float) Math.Sin(vAngleInRadians));
        }

        public static EWPoint[] GetPointsWhereLineIntersectsCubicCurve(
            EWImmutablePoint aLinePoint1,
            EWImmutablePoint aLinePoint2,
            EWImmutablePoint aCurveStartPoint,
            EWImmutablePoint aCurveControlPoint1,
            EWImmutablePoint aCurveControlPoint2,
            EWImmutablePoint aCurveEndPoint)
        {
            var vPoints = BezierIntersection.GetPointsWhereLineIntersectsCubic(
                aLinePoint1,
                aLinePoint2,
                aCurveStartPoint,
                aCurveControlPoint1,
                aCurveControlPoint2,
                aCurveEndPoint);

            var vLine = new EWLine(new EWPoint(aLinePoint1), new EWPoint(aLinePoint2));
            vPoints = CheckAndAddIfEndPointIsOnLine(vLine, aCurveStartPoint, vPoints);
            vPoints = CheckAndAddIfEndPointIsOnLine(vLine, aCurveEndPoint, vPoints);

            //Logger.Debug("GetPointsWhereLineIntersectsCubicCurve: {0} {1} {2} {3} {4} {5} : {6}", aLinePoint1, aLinePoint2, aCurveStartPoint, aCurveControlPoint1, aCurveControlPoint2, aCurveEndPoint, vPoints != null ? vPoints.Length : 0);
            return vPoints;
        }

        public static EWPoint[] GetPointsWhereLineIntersectsQuadCurve(
            EWImmutablePoint aLinePoint1,
            EWImmutablePoint aLinePoint2,
            EWImmutablePoint aCurveStartPoint,
            EWImmutablePoint aCurveControlPoint1,
            EWImmutablePoint aCurveEndPoint)
        {
            var vPoints = BezierIntersection.GetPointsWhereLineIntersectsQuadratic(
                aLinePoint1,
                aLinePoint2,
                aCurveStartPoint,
                aCurveControlPoint1,
                aCurveEndPoint);

            var vLine = new EWLine(new EWPoint(aLinePoint1), new EWPoint(aLinePoint2));
            vPoints = CheckAndAddIfEndPointIsOnLine(vLine, aCurveStartPoint, vPoints);
            vPoints = CheckAndAddIfEndPointIsOnLine(vLine, aCurveEndPoint, vPoints);
            return vPoints;
        }

        public static EWPoint[] CheckAndAddIfEndPointIsOnLine(EWLine aLine, EWImmutablePoint aPoint, EWPoint[] aPoints)
        {
            var vPoints = aPoints;

            if (Math.Abs(aLine.DistanceFromLine(aPoint) - 0) < Epsilon)
            {
                if (vPoints == null)
                {
                    vPoints = new[] {new EWPoint(aPoint)};
                }
                else
                {
                    vPoints = new EWPoint[aPoints.Length + 1];
                    for (var i = 0; i < aPoints.Length; i++)
                    {
                        vPoints[i] = aPoints[i];
                    }

                    vPoints[aPoints.Length] = new EWPoint(aPoint);
                }
            }

            return vPoints;
        }

        public static float GetFactor(float aMin, float aMax, float aValue)
        {
            var vAdjustedValue = aValue - aMin;
            var vRange = aMax - aMin;

            if (Math.Abs(vAdjustedValue - vRange) < Epsilon)
            {
                return 1;
            }

            return vAdjustedValue / vRange;
        }

        public static float GetLinearValue(float aMin, float aMax, float aFactor)
        {
            var d = aMax - aMin;
            d *= aFactor;
            return aMin + d;
        }

        public static List<EWPoint> CheckAndAddIfEndPointIsOnLine(EWLine aLine, EWPoint aPoint, List<EWPoint> aPoints)
        {
            var vPoints = aPoints;

            if (Math.Abs(aLine.DistanceFromLine(aPoint) - 0) < Epsilon)
            {
                if (vPoints == null)
                {
                    vPoints = new List<EWPoint> {aPoint};
                }
                else
                {
                    vPoints.Add(aPoint);
                }
            }

            return vPoints;
        }

        //public static EWPoint[] GetPointsWhereLineIntersectsCubicCurve(EWPoint aLinePoint1, EWPoint aLinePoint2, EWPoint aCurveStartPoint, EWPoint aCurveControlPoint1, EWPoint aCurveControlPoint2, EWPoint aCurveEndPoint)
        //{
        //		return BezierIntersection.GetPointsWhereLineIntersectsCubic(aLinePoint1, aLinePoint2, aCurveStartPoint, aCurveControlPoint1, aCurveControlPoint2, aCurveEndPoint);

        /*var vPath = new EWPath(aCurveStartPoint);
         vPath.CurveTo(aCurveControlPoint1, aCurveEndPoint, aCurveControlPoint2);
         var vFlattened = vPath.GetFlattenedPath();
  
         List<EWPoint> vPoints = GetPointsWhereLineIntersectsPath(aLinePoint1, aLinePoint2,vFlattened);
         if (vPoints != null)
         {
            return vPoints.ToArray();	
         }
         return null;*/
        //}

        //public static EWPoint[] GetPointsWhereLineIntersectsQuadCurve(EWPoint aLinePoint1, EWPoint aLinePoint2, EWPoint aCurveStartPoint, EWPoint aCurveControlPoint, EWPoint aCurveEndPoint)
        //{
        //	return BezierIntersection.GetPointsWhereLineIntersectsQuadratic(aLinePoint1, aLinePoint2, aCurveStartPoint, aCurveControlPoint, aCurveEndPoint);

        /*var vPath = new EWPath(aCurveStartPoint);
         vPath.QuadTo(aCurveControlPoint, aCurveEndPoint);
         var vFlattened = vPath.GetFlattenedPath();
  
         List<EWPoint> vPoints = GetPointsWhereLineIntersectsPath(aLinePoint1, aLinePoint2,vFlattened);
         if (vPoints != null)
         {
            return vPoints.ToArray();	
         }
         return null;*/
        //}
    }

    public class TranslationData
    {
        public float xFactor;
        public float xOffset;
        public float xOrigin;
        public float yFactor;
        public float yOffset;
        public float yOrigin;
    }
}