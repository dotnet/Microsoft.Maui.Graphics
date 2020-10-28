using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Elevenworks.Graphics
{
    public static class GDIGraphicsExtensions
    {
        public static RectangleF AsRectangleF(this EWRectangle target)
        {
            return new RectangleF(target.MinX, target.MinY, Math.Abs(target.Width), Math.Abs(target.Height));
        }

        public static EWRectangle AsEWRectangle(this RectangleF target)
        {
            return new EWRectangle(target.Left, target.Top, Math.Abs(target.Width), Math.Abs(target.Height));
        }

        public static EWRectangle AsEWRectangle(this Rectangle target)
        {
            return new EWRectangle(target.Left, target.Top, Math.Abs(target.Width), Math.Abs(target.Height));
        }

        public static SizeF AsSizeF(this EWSize target)
        {
            return new SizeF(target.Width, target.Height);
        }

        public static EWSize AsEWSize(this SizeF target)
        {
            return new EWSize(target.Width, target.Height);
        }

        public static PointF ToPointF(this EWImmutablePoint target)
        {
            return new PointF(target.X, target.Y);
        }

        public static Color AsColor(this EWColor color)
        {
            if (color == null) return Color.Black;

            var alpha = (int) (color.Alpha * 255);
            var red = (int) (color.Red * 255);
            var green = (int) (color.Green * 255);
            var blue = (int) (color.Blue * 255);

            return Color.FromArgb(alpha, red, green, blue);
        }


        public static GraphicsPath AsGDIPath(this EWPath target)
        {
            return AsGDIPath(target, 1);
        }

        public static GraphicsPath AsGDIPath(this EWPath target, float ppu)
        {
            return AsGDIPath(target, ppu, 0, 0, 1, 1);
        }

        public static GraphicsPath AsGDIPath(this EWPath target, float ppu, float ox, float oy, float fx, float fy)
        {
            var path = new GraphicsPath();

#if DEBUG
            try
            {
#endif

                float ppux = ppu * fx;
                float ppuy = ppu * fy;

                int pointIndex = 0;
                var arcAngleIndex = 0;
                var arcClockwiseIndex = 0;

                foreach (var type in target.SegmentTypes)
                {
                    if (type == PathOperation.MOVE_TO)
                    {
                        path.StartFigure();
                        pointIndex++;
                    }
                    else if (type == PathOperation.LINE)
                    {
                        var startPoint = target[pointIndex - 1];
                        var endPoint = target[pointIndex++];
                        path.AddLine(
                            ox + startPoint.X * ppux,
                            oy + startPoint.Y * ppuy,
                            ox + endPoint.X * ppux,
                            oy + endPoint.Y * ppuy);
                    }
                    else if (type == PathOperation.QUAD)
                    {
                        var startPoint = target[pointIndex - 1];
                        var quadControlPoint = target[pointIndex++];
                        var endPoint = target[pointIndex++];

                        var cubicControlPoint1_X = startPoint.X + 2.0f * (quadControlPoint.X - startPoint.X) / 3.0f;
                        var cubicControlPoint1_Y = startPoint.Y + 2.0f * (quadControlPoint.Y - startPoint.Y) / 3.0f;

                        var cubicControlPoint2_X = endPoint.X + 2.0f * (quadControlPoint.X - endPoint.X) / 3.0f;
                        var cubicControlPoint2_Y = endPoint.Y + 2.0f * (quadControlPoint.Y - endPoint.Y) / 3.0f;

                        path.AddBezier(
                            ox + startPoint.X * ppux,
                            oy + startPoint.Y * ppuy,
                            ox + cubicControlPoint1_X * ppux,
                            oy + cubicControlPoint1_Y * ppuy,
                            ox + cubicControlPoint2_X * ppux,
                            oy + cubicControlPoint2_Y * ppuy,
                            ox + endPoint.X * ppux,
                            oy + endPoint.Y * ppuy);
                    }
                    else if (type == PathOperation.CUBIC)
                    {
                        var startPoint = target[pointIndex - 1];
                        var cubicControlPoint1 = target[pointIndex++];
                        var cubicControlPoint2 = target[pointIndex++];
                        var endPoint = target[pointIndex++];

                        path.AddBezier(
                            ox + startPoint.X * ppux,
                            oy + startPoint.Y * ppuy,
                            ox + cubicControlPoint1.X * ppux,
                            oy + cubicControlPoint1.Y * ppuy,
                            ox + cubicControlPoint2.X * ppux,
                            oy + cubicControlPoint2.Y * ppuy,
                            ox + endPoint.X * ppux,
                            oy + endPoint.Y * ppuy);
                    }
                    else if (type == PathOperation.ARC)
                    {
                        var topLeft = target[pointIndex++];
                        var bottomRight = target[pointIndex++];
                        var startAngle = target.GetArcAngle(arcAngleIndex++);
                        var endAngle = target.GetArcAngle(arcAngleIndex++);
                        var clockwise = target.GetArcClockwise(arcClockwiseIndex++);

                        while (startAngle < 0)
                        {
                            startAngle += 360;
                        }

                        while (endAngle < 0)
                        {
                            endAngle += 360;
                        }

                        var x = ox + topLeft.X * ppux;
                        var y = oy + topLeft.Y * ppuy;
                        var width = (bottomRight.X - topLeft.X) * ppux;
                        var height = (bottomRight.Y - topLeft.Y) * ppuy;

                        float sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);
                        if (!clockwise)
                        {
                            startAngle = endAngle;
                        }

                        startAngle *= -1;

                        path.AddArc(x, y, width, height, startAngle, sweep);
                    }
                    else if (type == PathOperation.CLOSE)
                    {
                        path.CloseFigure();
                    }
                }

                return path;
#if DEBUG
            }
            catch (Exception exc)
            {
                path.Dispose();

                var definition = target.ToDefinitionString();
                Logger.Debug(string.Format("Unable to convert the path to a GDIPath: {0}", definition), exc);
                return null;
            }
#endif
        }
    }
}