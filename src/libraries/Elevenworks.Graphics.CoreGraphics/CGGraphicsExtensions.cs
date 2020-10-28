using System;
using CoreGraphics;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public static class CGGraphicsExtensions
    {
        public static CGRect AsCGRect(this EWRectangle target)
        {
            return new CGRect(target.MinX, target.MinY, Math.Abs(target.Width), Math.Abs(target.Height));
        }

        public static EWRectangle AsEWRectangle(this CGRect target)
        {
            return new EWRectangle(
                (float) target.Left,
                (float) target.Top,
                (float) Math.Abs(target.Width),
                (float) Math.Abs(target.Height));
        }

        public static EWSize AsEWSize(this CGSize target)
        {
            return new EWSize(
                (float) target.Width,
                (float) target.Height);
        }

        public static CGPoint ToCGPoint(this EWImmutablePoint target)
        {
            return new CGPoint(target.X, target.Y);
        }

        public static EWPath AsEWPath(this CGPath target)
        {
            var path = new EWPath();
            var converter = new PathConverter(path);
            target.Apply(converter.ApplyCGPathFunction);
            return path;
        }

        public static CGPath AsCGPath(
            this EWPath target)
        {
            return AsCGPath(target, 0, 0, 1, 1);
        }

        public static CGPath AsCGPath(
            this EWPath target,
            float ox,
            float oy,
            float fx,
            float fy)
        {
            var path = new CGPath();

            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

            foreach (var type in target.SegmentTypes)
            {
                if (type == PathOperation.MOVE_TO)
                {
                    var point = target[pointIndex++];
                    path.MoveToPoint((ox + point.X * fx), (oy + point.Y * fy));
                }
                else if (type == PathOperation.LINE)
                {
                    var endPoint = target[pointIndex++];
                    path.AddLineToPoint((ox + endPoint.X * fx), (oy + endPoint.Y * fy));
                }

                else if (type == PathOperation.QUAD)
                {
                    var controlPoint = target[pointIndex++];
                    var endPoint = target[pointIndex++];
                    path.AddQuadCurveToPoint(
                        (ox + controlPoint.X * fx),
                        (oy + controlPoint.Y * fy),
                        (ox + endPoint.X * fx),
                        (oy + endPoint.Y * fy));
                }
                else if (type == PathOperation.CUBIC)
                {
                    var controlPoint1 = target[pointIndex++];
                    var controlPoint2 = target[pointIndex++];
                    var endPoint = target[pointIndex++];
                    path.AddCurveToPoint(
                        (ox + controlPoint1.X * fx),
                        (oy + controlPoint1.Y * fy),
                        (ox + controlPoint2.X * fx),
                        (oy + controlPoint2.Y * fy),
                        (ox + endPoint.X * fx),
                        (oy + endPoint.Y * fy));
                }
                else if (type == PathOperation.ARC)
                {
                    var topLeft = target[pointIndex++];
                    var bottomRight = target[pointIndex++];
                    float startAngle = target.GetArcAngle(arcAngleIndex++);
                    float endAngle = target.GetArcAngle(arcAngleIndex++);
                    var clockwise = target.GetArcClockwise(arcClockwiseIndex++);

                    var startAngleInRadians = Geometry.DegreesToRadians(-startAngle);
                    var endAngleInRadians = Geometry.DegreesToRadians(-endAngle);

                    while (startAngleInRadians < 0)
                    {
                        startAngleInRadians += (float) Math.PI * 2;
                    }

                    while (endAngleInRadians < 0)
                    {
                        endAngleInRadians += (float) Math.PI * 2;
                    }

                    var cx = (bottomRight.X + topLeft.X) / 2;
                    var cy = (bottomRight.Y + topLeft.Y) / 2;
                    var width = bottomRight.X - topLeft.X;
                    var height = bottomRight.Y - topLeft.Y;
                    var r = width / 2;

                    var transform = CGAffineTransform.MakeTranslation(ox + cx, oy + cy);
                    transform = CGAffineTransform.Multiply(CGAffineTransform.MakeScale(1, height / width), transform);

                    path.AddArc(transform, 0, 0, r * fx, startAngleInRadians, endAngleInRadians, !clockwise);
                }
                else if (type == PathOperation.CLOSE)
                {
                    path.CloseSubpath();
                }
            }

            return path;
        }

        public static CGPath AsCGPath(
            this EWPath target,
            float scale,
            float zoom)
        {
            var factor = scale * zoom;
            return AsCGPath(target, 0, 0, factor, factor);
        }

        public static CGPath AsCGPathFromSegment(
            this EWPath target,
            int segmentIndex,
            float ppu,
            float zoom)
        {
            ppu = ppu * zoom;
            var path = new CGPath();

            var type = target.GetSegmentType(segmentIndex);
            if (type == PathOperation.LINE)
            {
                var pointIndex = target.GetSegmentPointIndex(segmentIndex);
                var startPoint = target[pointIndex - 1];
                path.MoveToPoint(startPoint.X * ppu, startPoint.Y * ppu);
                var endPoint = target[pointIndex];
                path.AddLineToPoint(endPoint.X * ppu, endPoint.Y * ppu);
            }
            else if (type == PathOperation.QUAD)
            {
                var pointIndex = target.GetSegmentPointIndex(segmentIndex);
                var startPoint = target[pointIndex - 1];
                path.MoveToPoint(startPoint.X * ppu, startPoint.Y * ppu);
                var controlPoint = target[pointIndex++];
                var endPoint = target[pointIndex];
                path.AddQuadCurveToPoint(controlPoint.X * ppu, controlPoint.Y * ppu, endPoint.X * ppu, endPoint.Y * ppu);
            }
            else if (type == PathOperation.CUBIC)
            {
                var pointIndex = target.GetSegmentPointIndex(segmentIndex);
                var startPoint = target[pointIndex - 1];
                path.MoveToPoint(startPoint.X * ppu, startPoint.Y * ppu);
                var controlPoint1 = target[pointIndex++];
                var controlPoint2 = target[pointIndex++];
                var endPoint = target[pointIndex];
                path.AddCurveToPoint(controlPoint1.X * ppu, controlPoint1.Y * ppu, controlPoint2.X * ppu, controlPoint2.Y * ppu, endPoint.X * ppu, endPoint.Y * ppu);
            }
            else if (type == PathOperation.ARC)
            {
                target.GetSegmentInfo(segmentIndex, out var pointIndex, out var arcAngleIndex, out var arcClockwiseIndex);

                var topLeft = target[pointIndex++];
                var bottomRight = target[pointIndex++];
                var startAngle = target.GetArcAngle(arcAngleIndex++);
                var endAngle = target.GetArcAngle(arcAngleIndex++);
                var clockwise = target.GetArcClockwise(arcClockwiseIndex++);

                var startAngleInRadians = Geometry.DegreesToRadians(-startAngle);
                var endAngleInRadians = Geometry.DegreesToRadians(-endAngle);

                while (startAngleInRadians < 0)
                {
                    startAngleInRadians += (float) Math.PI * 2;
                }

                while (endAngleInRadians < 0)
                {
                    endAngleInRadians += (float) Math.PI * 2;
                }

                var cx = (bottomRight.X + topLeft.X) / 2;
                var cy = (bottomRight.Y + topLeft.Y) / 2;
                var width = bottomRight.X - topLeft.X;
                var height = bottomRight.Y - topLeft.Y;
                var r = width / 2;

                var transform = CGAffineTransform.MakeTranslation(cx * ppu, cy * ppu);
                transform = CGAffineTransform.Multiply(CGAffineTransform.MakeScale(1, height / width), transform);

                path.AddArc(transform, 0, 0, r * ppu, startAngleInRadians, endAngleInRadians, !clockwise);
            }

            return path;
        }

        public static CGPath AsRotatedCGPath(this EWPath target, EWImmutablePoint center, float ppu, float zoom, float angle)
        {
            ppu = ppu * zoom;
            var path = new CGPath();

            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

            foreach (var type in target.SegmentTypes)
            {
                if (type == PathOperation.MOVE_TO)
                {
                    var point = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.MoveToPoint(point.X * ppu, point.Y * ppu);
                }
                else if (type == PathOperation.LINE)
                {
                    var endPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.AddLineToPoint(endPoint.X * ppu, endPoint.Y * ppu);
                }
                else if (type == PathOperation.QUAD)
                {
                    var controlPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    var endPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.AddQuadCurveToPoint(
                        controlPoint.X * ppu,
                        controlPoint.Y * ppu,
                        endPoint.X * ppu,
                        endPoint.Y * ppu);
                }
                else if (type == PathOperation.CUBIC)
                {
                    var controlPoint1 = target.GetRotatedPoint(pointIndex++, center, angle);
                    var controlPoint2 = target.GetRotatedPoint(pointIndex++, center, angle);
                    var endPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.AddCurveToPoint(
                        controlPoint1.X * ppu,
                        controlPoint1.Y * ppu,
                        controlPoint2.X * ppu,
                        controlPoint2.Y * ppu,
                        endPoint.X * ppu,
                        endPoint.Y * ppu);
                }
                else if (type == PathOperation.ARC)
                {
                    var topLeft = target[pointIndex++];
                    var bottomRight = target[pointIndex++];
                    float startAngle = target.GetArcAngle(arcAngleIndex++);
                    float endAngle = target.GetArcAngle(arcAngleIndex++);
                    var clockwise = target.GetArcClockwise(arcClockwiseIndex++);

                    var startAngleInRadians = Geometry.DegreesToRadians(-startAngle);
                    var endAngleInRadians = Geometry.DegreesToRadians(-endAngle);

                    while (startAngleInRadians < 0)
                    {
                        startAngleInRadians += (float) Math.PI * 2;
                    }

                    while (endAngleInRadians < 0)
                    {
                        endAngleInRadians += (float) Math.PI * 2;
                    }

                    var cx = (bottomRight.X + topLeft.X) / 2;
                    var cy = (bottomRight.Y + topLeft.Y) / 2;
                    var width = bottomRight.X - topLeft.X;
                    var height = bottomRight.Y - topLeft.Y;
                    var r = width / 2;

                    var rotatedCenter = Geometry.RotatePoint(center, new EWPoint(cx, cy), angle);

                    var transform = CGAffineTransform.MakeTranslation(rotatedCenter.X * ppu, rotatedCenter.Y * ppu);
                    transform = CGAffineTransform.Multiply(CGAffineTransform.MakeScale(1, height / width), transform);

                    path.AddArc(transform, 0, 0, r * ppu, startAngleInRadians, endAngleInRadians, !clockwise);
                }
                else if (type == PathOperation.CLOSE)
                {
                    path.CloseSubpath();
                }
            }

            return path;
        }

        public static CGSize AsSizeF(this EWSize target)
        {
            return new CGSize(target.Width, target.Height);
        }

        public static EWPoint AsEWPoint(this CGPoint target)
        {
            return new EWPoint((float) target.X, (float) target.Y);
        }

        public class PathConverter
        {
            private readonly EWPath _path;

            public PathConverter(EWPath aPath)
            {
                _path = aPath;
            }

            public void ApplyCGPathFunction(CGPathElement element)
            {
                if (element.Type == CGPathElementType.MoveToPoint)
                {
                    _path.MoveTo(element.Point1.AsEWPoint());
                }
                else if (element.Type == CGPathElementType.AddLineToPoint)
                {
                    _path.LineTo(element.Point1.AsEWPoint());
                }
                else if (element.Type == CGPathElementType.AddQuadCurveToPoint)
                {
                    _path.QuadTo(element.Point1.AsEWPoint(), element.Point2.AsEWPoint());
                }
                else if (element.Type == CGPathElementType.AddCurveToPoint)
                {
                    _path.CurveTo(element.Point1.AsEWPoint(), element.Point2.AsEWPoint(), element.Point3.AsEWPoint());
                }
                else if (element.Type == CGPathElementType.CloseSubpath)
                {
                    _path.Close();
                }
            }
        }

        public static CGAffineTransform AsCGAffineTransform(this EWAffineTransform transform)
        {
            if (transform != null)
            {
                var matrix = new float[6];
                transform.GetMatrix(matrix);
                float xx = matrix[0];
                float yx = matrix[1];
                float xy = matrix[2];
                float yy = matrix[3];
                float x0 = matrix[4];
                float y0 = matrix[5];

                return new CGAffineTransform(xx, yx, xy, yy, x0, y0);
            }

            return CGAffineTransform.MakeIdentity();
        }
    }
}