using Android.Graphics;
using Android.Text;
using Color = Android.Graphics.Color;
using PointF = Android.Graphics.PointF;
using RectangleF = System.Drawing.RectangleF;
using SizeF = System.Drawing.SizeF;
    
namespace System.Graphics.Android
{
    public static class MDGraphicsExtensions
    {
        public static global::Android.Graphics.Color AsColorMultiplyAlpha(this Color target, float alpha)
        {
            var r = (int) (target.Red * 255f);
            var g = (int) (target.Green * 255f);
            var b = (int) (target.Blue * 255f);
            var a = (int) (target.Alpha * alpha * 255f);

            if (a > 255)
            {
                a = 255;
            }

            var color = new global::Android.Graphics.Color(r, g, b, a);
            return color;
        }

        public static int ToArgb(this Color target)
        {
            var a = (int) (target.Alpha * 255f);
            var r = (int) (target.Red * 255f);
            var g = (int) (target.Green * 255f);
            var b = (int) (target.Blue * 255f);

            int argb = a << 24 | r << 16 | g << 8 | b;
            return argb;
        }

        public static int ToArgb(this Color target, float alpha)
        {
            var a = (int) (target.Alpha * 255f * alpha);
            var r = (int) (target.Red * 255f);
            var g = (int) (target.Green * 255f);
            var b = (int) (target.Blue * 255f);

            int argb = a << 24 | r << 16 | g << 8 | b;
            return argb;
        }

        public static global::Android.Graphics.Color AsColor(this Color target)
        {
            var r = (int) (target.Red * 255f);
            var g = (int) (target.Green * 255f);
            var b = (int) (target.Blue * 255f);
            var a = (int) (target.Alpha * 255f);
            return new global::Android.Graphics.Color(r, g, b, a);
        }

        public static Color AsColor(this global::Android.Graphics.Color target)
        {
            var r = (int) target.R;
            var g = (int) target.G;
            var b = (int) target.B;
            var a = (int) target.A;
            return new Color(r, g, b, a);
        }

        public static Drawing.RectangleF AsRectangleF(this RectangleF target)
        {
            return new Drawing.RectangleF(target.Left, target.Top, Math.Abs(target.Width), Math.Abs(target.Height));
        }

        public static RectF AsRectF(this RectangleF target)
        {
            return new RectF(target.Left, target.Top, target.Right, target.Bottom);
        }

        public static RectangleF AsRectangleF(this Drawing.RectangleF target)
        {
            return new RectangleF(target.Left, target.Top, Math.Abs(target.Width), Math.Abs(target.Height));
        }

        public static RectangleF AsRectangleF(this RectF target)
        {
            return new RectangleF(target.Left, target.Top, Math.Abs(target.Width()), Math.Abs(target.Height()));
        }

        public static RectF AsRectF(this Rect target)
        {
            return new RectF(target);
        }

        public static global::Android.Graphics.PointF ToPointF(this PointF target)
        {
            return new global::Android.Graphics.PointF(target.X, target.Y);
        }

        /* public static EWPath AsEWPath(this global::Android.Graphics.Path target)
        {
            var vPath = new EWPath();
  
            var vConverter = new PathConverter(vPath);
            target.Apply(vConverter.ApplyCGPathFunction);
  
            return vPath;
        } */

        public static Matrix AsMatrix(this AffineTransform transform)
        {
            var values = new float[9];
            transform.GetMatrix(values);

            /*values [Matrix.MscaleX] = transform.GetScaleX (); // 0
            values [Matrix.MskewX] = transform.GetShearX (); // 1
            values [Matrix.MtransX] = transform.GetTranslateX (); // 2
            values [Matrix.MskewY] = transform.GetShearY (); // 3
            values [Matrix.MscaleY] = transform.GetScaleY (); // 4
            values [Matrix.MtransY] = transform.GetTranslateY (); // 5*/

            values[Matrix.Mpersp0] = 0; // 6
            values[Matrix.Mpersp1] = 0; // 7
            values[Matrix.Mpersp2] = 1; // 8

            var matrix = new Matrix();
            matrix.SetValues(values);
            return matrix;
        }

        public static Path AsAndroidPath(this PathF target)
        {
            return AsAndroidPath(target, 1);
        }

        public static Path AsAndroidPath(this PathF path, float ppu)
        {
            return AsAndroidPath(path, ppu, 0, 0, 1, 1);
        }

        public static Path AsAndroidPath(this PathF path, float ppu, float ox, float oy, float fx, float fy)
        {
            var nativePath = new Path();

            float ppux = ppu * fx;
            float ppuy = ppu * fy;

            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

            foreach (PathOperation vType in path.SegmentTypes)
            {
                if (vType == PathOperation.Move)
                {
                    var point = path[pointIndex++];
                    nativePath.MoveTo(ox + point.X * ppux, oy + point.Y * ppuy);
                }
                else if (vType == PathOperation.Line)
                {
                    var point = path[pointIndex++];
                    nativePath.LineTo(ox + point.X * ppux, oy + point.Y * ppuy);
                }

                else if (vType == PathOperation.Quad)
                {
                    var controlPoint = path[pointIndex++];
                    var point = path[pointIndex++];
                    nativePath.QuadTo(ox + controlPoint.X * ppux, oy + controlPoint.Y * ppuy, ox + point.X * ppux, oy + point.Y * ppuy);
                }
                else if (vType == PathOperation.Cubic)
                {
                    var controlPoint1 = path[pointIndex++];
                    var controlPoint2 = path[pointIndex++];
                    var point = path[pointIndex++];
                    nativePath.CubicTo(ox + controlPoint1.X * ppux, oy + controlPoint1.Y * ppuy, ox + controlPoint2.X * ppux, oy + controlPoint2.Y * ppuy, ox + point.X * ppux,
                        oy + point.Y * ppuy);
                }
                else if (vType == PathOperation.Arc)
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

                    var rect = new RectF(ox + topLeft.X * ppux, oy + topLeft.Y * ppuy, ox + bottomRight.X * ppux, oy + bottomRight.Y * ppuy);
                    var sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);

                    startAngle *= -1;
                    if (!clockwise)
                    {
                        sweep *= -1;
                    }

                    nativePath.ArcTo(rect, startAngle, sweep);
                }
                else if (vType == PathOperation.Close)
                {
                    nativePath.Close();
                }
            }

            return nativePath;
        }

        public static Path AsAndroidPath(this PathF path, float ppu, float zoom)
        {
            return AsAndroidPath(path, ppu * zoom);
        }

        public static Path AsAndroidPathFromSegment(this PathF target, int segmentIndex, float ppu, float zoom)
        {
            ppu = zoom * ppu;

            var path = new Path();

            var type = target.GetSegmentType(segmentIndex);
            if (type == PathOperation.Line)
            {
                int pointIndex = target.GetSegmentPointIndex(segmentIndex);
                var startPoint = target[pointIndex - 1];
                path.MoveTo(startPoint.X * ppu, startPoint.Y * ppu);

                var endPoint = target[pointIndex];
                path.LineTo(endPoint.X * ppu, endPoint.Y * ppu);
            }
            else if (type == PathOperation.Quad)
            {
                int pointIndex = target.GetSegmentPointIndex(segmentIndex);
                var startPoint = target[pointIndex - 1];
                path.MoveTo(startPoint.X * ppu, startPoint.Y * ppu);

                var controlPoint = target[pointIndex++];
                var endPoint = target[pointIndex];
                path.QuadTo(controlPoint.X * ppu, controlPoint.Y * ppu, endPoint.X * ppu, endPoint.Y * ppu);
            }
            else if (type == PathOperation.Cubic)
            {
                int pointIndex = target.GetSegmentPointIndex(segmentIndex);
                var startPoint = target[pointIndex - 1];
                path.MoveTo(startPoint.X * ppu, startPoint.Y * ppu);

                var controlPoint1 = target[pointIndex++];
                var controlPoint2 = target[pointIndex++];
                var endPoint = target[pointIndex];
                path.CubicTo(controlPoint1.X * ppu, controlPoint1.Y * ppu, controlPoint2.X * ppu, controlPoint2.Y * ppu, endPoint.X * ppu, endPoint.Y * ppu);
            }
            else if (type == PathOperation.Arc)
            {
                target.GetSegmentInfo(segmentIndex, out var pointIndex, out var arcAngleIndex, out var arcClockwiseIndex);

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

                var rect = new RectF(topLeft.X * ppu, topLeft.Y * ppu, bottomRight.X * ppu, bottomRight.Y * ppu);
                var sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);

                startAngle *= -1;
                if (!clockwise)
                {
                    sweep *= -1;
                }

                path.ArcTo(rect, startAngle, sweep);
            }

            return path;
        }

        public static Path AsRotatedAndroidPath(this PathF target, PointF center, float ppu, float zoom, float angle)
        {
            ppu = zoom * ppu;

            var path = new Path();

            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

            foreach (var type in target.SegmentTypes)
            {
                if (type == PathOperation.Move)
                {
                    var point = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.MoveTo(point.X * ppu, point.Y * ppu);
                }
                else if (type == PathOperation.Line)
                {
                    var endPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.LineTo(endPoint.X * ppu, endPoint.Y * ppu);
                }
                else if (type == PathOperation.Quad)
                {
                    var controlPoint1 = target.GetRotatedPoint(pointIndex++, center, angle);
                    var endPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.QuadTo(
                        controlPoint1.X * ppu,
                        controlPoint1.Y * ppu,
                        endPoint.X * ppu,
                        endPoint.Y * ppu);
                }
                else if (type == PathOperation.Cubic)
                {
                    var controlPoint1 = target.GetRotatedPoint(pointIndex++, center, angle);
                    var controlPoint2 = target.GetRotatedPoint(pointIndex++, center, angle);
                    var endPoint = target.GetRotatedPoint(pointIndex++, center, angle);
                    path.CubicTo(
                        controlPoint1.X * ppu,
                        controlPoint1.Y * ppu,
                        controlPoint2.X * ppu,
                        controlPoint2.Y * ppu,
                        endPoint.X * ppu,
                        endPoint.Y * ppu);
                }
                else if (type == PathOperation.Arc)
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

                    var rect = new RectF(topLeft.X * ppu, topLeft.Y * ppu, bottomRight.X * ppu, bottomRight.Y * ppu);
                    var sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);

                    startAngle *= -1;
                    if (!clockwise)
                    {
                        sweep *= -1;
                    }

                    path.ArcTo(rect, startAngle, sweep);
                }
                else if (type == PathOperation.Close)
                {
                    path.Close();
                }
            }

            return path;
        }

        public static SizeF AsSize(this Drawing.SizeF target)
        {
            return new SizeF(target.Width, target.Height);
        }

        public static Drawing.SizeF AsSizeF(this SizeF target)
        {
            return new Drawing.SizeF(target.Width, target.Height);
        }

        public static PointF AsEWPoint(this global::Android.Graphics.PointF target)
        {
            return new PointF(target.X, target.Y);
        }

        public static Bitmap GetPatternBitmap(this Paint paint, float scale = 1)
        {
            var pattern = paint?.Pattern;
            if (pattern == null)
                return null;

            using (var context = new MDBitmapExportContext((int) (pattern.Width * scale), (int) (pattern.Height * scale), scale, disposeBitmap: false))
            {
                var canvas = context.Canvas;
                canvas.Scale(scale, scale);
                pattern.Draw(canvas);
                return context.Bitmap;
            }
        }

        public static Bitmap GetPatternBitmap(
            this Paint paint,
            float scaleX,
            float scaleY)
        {
            var pattern = paint?.Pattern;
            if (pattern == null)
                return null;

            using (var context = new MDBitmapExportContext((int) (pattern.Width * scaleX), (int) (pattern.Height * scaleY), disposeBitmap: false))
            {
                var scalingCanvas = new ScalingCanvas(context.Canvas);
                scalingCanvas.Scale(scaleX, scaleY);
                
                pattern.Draw(scalingCanvas);

                return context.Bitmap;
            }
        }

        public static SizeF GetTextSizeAsEWSize(this StaticLayout target, bool hasBoundedWidth)
        {
            // We need to know if the static layout was created with a bounded width, as this is what 
            // StaticLayout.Width returns.
            if (hasBoundedWidth)
                return new SizeF(target.Width, target.Height);

            float maxWidth = 0;
            int lineCount = target.LineCount;

            for (int i = 0; i < lineCount; i++)
            {
                float lineWidth = target.GetLineWidth(i);
                if (lineWidth > maxWidth)
                {
                    maxWidth = lineWidth;
                }
            }

            return new SizeF(maxWidth, target.Height);
        }

        public static Drawing.SizeF GetOffsetsToDrawText(
            this StaticLayout target,
            float x,
            float y,
            float width,
            float height,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment)
        {
            if (verticalAlignment != VerticalAlignment.Top)
            {
                Drawing.SizeF vTextFrameSize = target.GetTextSize();

                float vOffsetX = 0;
                float vOffsetY = 0;

                if (height > 0)
                {
                    if (verticalAlignment == VerticalAlignment.Bottom)
                        vOffsetY = height - vTextFrameSize.Height;
                    else
                        vOffsetY = (height - vTextFrameSize.Height) / 2;
                }

                return new Drawing.SizeF(x + vOffsetX, y + vOffsetY);
            }

            return new Drawing.SizeF(x, y);
        }

        public static Bitmap Downsize(this Bitmap target, int maxSize, bool dispose = true)
        {
            if (target.Width > maxSize || target.Height > maxSize)
            {
                float factor;

                if (target.Width > target.Height)
                {
                    factor = maxSize / (float) target.Width;
                }
                else
                {
                    factor = maxSize / (float) target.Height;
                }

                var w = (int) Math.Round(factor * target.Width);
                var h = (int) Math.Round(factor * target.Height);

                var newImage = Bitmap.CreateScaledBitmap(target, w, h, true);
                if (dispose)
                {
                    target.Recycle();
                    target.Dispose();
                }

                return newImage;
            }

            return target;
        }

        public static Bitmap Downsize(this Bitmap target, int maxWidth, int maxHeight, bool dispose = true)
        {
            var newImage = Bitmap.CreateScaledBitmap(target, maxWidth, maxHeight, true);
            if (dispose)
            {
                target.Recycle();
                target.Dispose();
            }

            return newImage;
        }
    }
}