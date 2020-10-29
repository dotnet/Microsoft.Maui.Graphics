using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Elevenworks.Graphics;
using Xamarin.Text;

namespace Xamarin.Graphics.GDI
{
    public class GDICanvas : AbstractCanvas<GDICanvasState>
    {
        private System.Drawing.Graphics _graphics;

        private RectangleF _rect;
        private Rectangle _rectI;

        public GDICanvas()
            : base(CreateNewState, CreateStateCopy)
        {
        }

        private static GDICanvasState CreateNewState(object context)
        {
            var canvas = (GDICanvas) context;
            return new GDICanvasState(canvas.Graphics);
        }

        private static GDICanvasState CreateStateCopy(GDICanvasState prototype)
        {
            return new GDICanvasState(prototype);
        }

        public System.Drawing.Graphics Graphics
        {
            get => _graphics;
            set
            {
                if (_graphics != value)
                {
                    _graphics = value;
                    _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    _graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                    ResetState();
                }
                else
                {
                    ResetState();
                }
            }
        }

        public override float MiterLimit
        {
            set => CurrentState.StrokeMiterLimit = value;
        }

        public override EWColor StrokeColor
        {
            set => CurrentState.StrokeColor = value?.AsColor() ?? Color.Black;
        }

        public override EWLineCap StrokeLineCap
        {
            set
            {
                switch (value)
                {
                    case EWLineCap.ROUND:
                        CurrentState.StrokeLineCap = LineCap.Round;
                        break;
                    case EWLineCap.SQUARE:
                        CurrentState.StrokeLineCap = LineCap.Square;
                        break;
                    default:
                        CurrentState.StrokeLineCap = LineCap.Flat;
                        break;
                }
            }
        }

        public override EWLineJoin StrokeLineJoin
        {
            set
            {
                switch (value)
                {
                    case EWLineJoin.BEVEL:
                        CurrentState.StrokeLineJoin = LineJoin.Bevel;
                        break;
                    case EWLineJoin.ROUND:
                        CurrentState.StrokeLineJoin = LineJoin.Round;
                        break;
                    default:
                        CurrentState.StrokeLineJoin = LineJoin.Miter;
                        break;
                }
            }
        }

        public override EWStrokeLocation StrokeLocation
        {
            set => CurrentState.StrokeLocation = value;
        }

        public override EWColor FillColor
        {
            set => CurrentState.FillColor = value?.AsColor() ?? Color.White;
        }

        public override EWColor FontColor
        {
            set => CurrentState.TextColor = value?.AsColor() ?? Color.Black;
        }

        public override string FontName
        {
            set
            {
                FontMapping vMapping = GDIFontManager.GetMapping(value);
                CurrentState.FontName = vMapping.Name;
                CurrentState.FontStyle = vMapping.Style;
            }
        }

        public override float FontSize
        {
            set => CurrentState.FontSize = value;
        }


        public override float Alpha
        {
            set
            {
                if (value < 1)
                {
                    Logger.Debug("Not implemented");
                }
            }
        }

        public override bool Antialias
        {
            set => Logger.Debug("Not implemented");
        }

        public override EWBlendMode BlendMode
        {
            set => Logger.Debug("Not implemented");
        }

        public override bool PixelShifted { get; set; }

        public override void SubtractFromClip(float x, float y, float width, float height)
        {
            var region = new Region(new RectangleF(x, y, width, height));
            _graphics.ExcludeClip(region);
            region.Dispose();
        }

        protected override void NativeDrawLine(float x1, float y1, float x2, float y2)
        {
            Draw(g => g.DrawLine(CurrentState.StrokePen, x1, y1, x2, y2));
        }

        protected override void NativeDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            while (startAngle < 0)
            {
                startAngle += 360;
            }

            while (endAngle < 0)
            {
                endAngle += 360;
            }

            float sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);
            CreateRectUsingStrokeLocation(x, y, width, height);
            if (!clockwise)
            {
                startAngle = endAngle;
            }

            startAngle *= -1;
            if (closed)
            {
                var path = CreatePathForArc(_rect, startAngle, sweep, true);
                Draw(g => g.DrawPath(CurrentState.StrokePen, path));
            }
            else
            {
                Draw(g => g.DrawArc(CurrentState.StrokePen, _rect, startAngle, sweep));
            }
        }

        private GraphicsPath CreatePathForArc(RectangleF arcRect, float startAngle, float sweep, bool closed = false)
        {
            var path = new GraphicsPath();
            path.AddArc(arcRect, startAngle, sweep);
            if (closed)
            {
                path.CloseFigure();
            }

            return path;
        }

        private void CreateRectUsingStrokeLocation(float x, float y, float width, float height)
        {
            if (CurrentState.StrokeLocation == EWStrokeLocation.CENTER)
            {
                SetRect(x, y, width, height);
            }
            else if (CurrentState.StrokeLocation == EWStrokeLocation.INSIDE)
            {
                var strokeWidth = CurrentState.StrokeWidth;
                _rect.X = Math.Min(x, x + width) + strokeWidth / 2;
                _rect.Y = Math.Min(y, y + height) + strokeWidth / 2;
                _rect.Width = Math.Abs(width) - strokeWidth;
                _rect.Height = Math.Abs(height) - strokeWidth;
            }
            else if (CurrentState.StrokeLocation == EWStrokeLocation.OUTSIDE)
            {
                var strokeWidth = CurrentState.StrokeWidth;
                _rect.X = Math.Min(x, x + width) - strokeWidth / 2;
                _rect.Y = Math.Min(y, y + height) - strokeWidth / 2;
                _rect.Width = Math.Abs(width) + strokeWidth;
                _rect.Height = Math.Abs(height) + strokeWidth;
            }
        }

        public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            while (startAngle < 0)
            {
                startAngle += 360;
            }

            while (endAngle < 0)
            {
                endAngle += 360;
            }

            float sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);
            CreateRectUsingStrokeLocation(x, y, width, height);
            if (!clockwise)
            {
                startAngle = endAngle;
            }

            startAngle *= -1;
            var path = CreatePathForArc(_rect, startAngle, sweep, true);
            Draw(g => g.FillPath(CurrentState.FillBrush, path));
        }

        protected override void NativeDrawRectangle(float x, float y, float width, float height)
        {
            CreateRectUsingStrokeLocation(x, y, width, height);
            Draw(g => g.DrawRectangle(CurrentState.StrokePen, _rect.X, _rect.Y, _rect.Width, _rect.Height));
        }

        public override void FillRectangle(float x, float y, float width, float height)
        {
            CreateRectUsingStrokeLocation(x, y, width, height);
            Draw(g => g.FillRectangle(CurrentState.FillBrush, _rect.X, _rect.Y, _rect.Width, _rect.Height));
        }

        protected override void NativeDrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            float strokeWidth = CurrentState.StrokeWidth;

            if (CurrentState.StrokeLocation == EWStrokeLocation.CENTER)
            {
                SetRect(x, y, width, height);
            }
            else if (CurrentState.StrokeLocation == EWStrokeLocation.INSIDE)
            {
                _rect.X = Math.Min(x, x + width) + strokeWidth / 2;
                _rect.Y = Math.Min(y, y + height) + strokeWidth / 2;
                _rect.Width = Math.Abs(width) - strokeWidth;
                _rect.Height = Math.Abs(height) - strokeWidth;

                cornerRadius -= strokeWidth / 2;
            }
            else if (CurrentState.StrokeLocation == EWStrokeLocation.OUTSIDE)
            {
                _rect.X = Math.Min(x, x + width) - strokeWidth / 2;
                _rect.Y = Math.Min(y, y + height) - strokeWidth / 2;
                _rect.Width = Math.Abs(width) + strokeWidth;
                _rect.Height = Math.Abs(height) + strokeWidth;

                cornerRadius += strokeWidth / 2;
            }

            if (cornerRadius > _rect.Width / 2)
            {
                cornerRadius = _rect.Width / 2;
            }

            if (cornerRadius > _rect.Height / 2)
            {
                cornerRadius = _rect.Height / 2;
            }

            var path = new GraphicsPath();
            path.AddArc(_rect.X, _rect.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(_rect.X + _rect.Width - cornerRadius, _rect.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(_rect.X + _rect.Width - cornerRadius, _rect.Y + _rect.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(_rect.X, _rect.Y + _rect.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.CloseAllFigures();

            // ReSharper disable once AccessToDisposedClosure
            Draw(g => g.DrawPath(CurrentState.StrokePen, path));

            path.Dispose();
        }

        private GraphicsPath GetPath(EWPath path)
        {
            var graphicsPath = path.NativePath as GraphicsPath;

            if (graphicsPath == null)
            {
                graphicsPath = path.AsGDIPath();
                path.NativePath = graphicsPath;
            }

            return graphicsPath;
        }

        protected override void NativeDrawPath(EWPath path)
        {
            if (path == null)
            {
                return;
            }

            var vGeometry = GetPath(path);

            Draw(g =>
            {
                // ReSharper disable AccessToDisposedClosure
                if (CurrentState.StrokeLocation == EWStrokeLocation.CENTER)
                {
                    g.DrawPath(CurrentState.StrokePen, vGeometry);
                }
                else if (CurrentState.StrokeLocation == EWStrokeLocation.INSIDE)
                {
                    var graphicsState = g.Save();
                    g.SetClip(vGeometry);
                    var pen = CurrentState.StrokePen;
                    pen.Width *= 2;
                    g.DrawPath(pen, vGeometry);
                    g.Restore(graphicsState);
                }
                else if (CurrentState.StrokeLocation == EWStrokeLocation.OUTSIDE)
                {
                    var graphicsState = g.Save();
                    var region = new Region(vGeometry);
                    g.ExcludeClip(region);
                    var pen = CurrentState.StrokePen;
                    pen.Width *= 2;
                    g.DrawPath(pen, vGeometry);
                    region.Dispose();
                    g.Restore(graphicsState);
                }
            });
        }

        public override void FillPath(EWPath path, EWWindingMode windingMode)
        {
            if (path == null)
            {
                return;
            }

            var graphicsPath = GetPath(path);
            graphicsPath.FillMode = windingMode == EWWindingMode.NonZero ? FillMode.Winding : FillMode.Alternate;
            Draw(g => g.FillPath(CurrentState.FillBrush, graphicsPath));
        }

        public override void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            if (path == null)
            {
                return;
            }

            var graphicsPath = GetPath(path);
            graphicsPath.FillMode = windingMode == EWWindingMode.NonZero ? FillMode.Winding : FillMode.Alternate;
            var region = new Region(graphicsPath);
            _graphics.IntersectClip(region);
        }

        public override void ClipRectangle(float x, float y, float width, float height)
        {
            var region = new Region(new RectangleF(x, y, width, height));
            _graphics.IntersectClip(region);
        }

        public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            SetRect(x, y, width, height);

            var path = new GraphicsPath();
            path.AddArc(_rect.X, _rect.Y, cornerRadius, cornerRadius, 180, 90);
            path.AddArc(_rect.X + _rect.Width - cornerRadius, _rect.Y, cornerRadius, cornerRadius, 270, 90);
            path.AddArc(_rect.X + _rect.Width - cornerRadius, _rect.Y + _rect.Height - cornerRadius, cornerRadius, cornerRadius, 0, 90);
            path.AddArc(_rect.X, _rect.Y + _rect.Height - cornerRadius, cornerRadius, cornerRadius, 90, 90);
            path.CloseAllFigures();

            Draw(g => g.FillPath(CurrentState.FillBrush, path));

            path.Dispose();
        }

        protected override void NativeDrawOval(float x, float y, float width, float height)
        {
            CreateRectUsingStrokeLocation(x, y, width, height);
            Draw(g => g.DrawEllipse(CurrentState.StrokePen, _rect));
        }

        public override void FillOval(float x, float y, float width, float height)
        {
            CreateRectUsingStrokeLocation(x, y, width, height);
            Draw(g => g.FillEllipse(CurrentState.FillBrush, _rect));
        }

        public override void DrawString(string value, float x, float y, EwHorizontalAlignment horizontalAlignment)
        {
            var font = CurrentState.Font;
            var size = _graphics.MeasureString(value, font);

            switch (horizontalAlignment)
            {
                case EwHorizontalAlignment.Right:
                    x -= size.Width;
                    break;
                case EwHorizontalAlignment.Center:
                case EwHorizontalAlignment.Justified:
                    x -= size.Width / 2;
                    break;
            }

            Draw(g => g.DrawString(value, font, CurrentState.TextBrush, x, y - size.Height));
        }

        public override void DrawString(string value, float x, float y, float width, float height, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineAdjustment = 0)
        {
            var font = CurrentState.Font;
            var format = new StringFormat();

            switch (horizontalAlignment)
            {
                case EwHorizontalAlignment.Left:
                    format.Alignment = StringAlignment.Near;
                    break;
                case EwHorizontalAlignment.Center:
                    format.Alignment = StringAlignment.Center;
                    break;
                case EwHorizontalAlignment.Right:
                    format.Alignment = StringAlignment.Far;
                    break;
                default:
                    format.Alignment = StringAlignment.Near;
                    break;
            }

            switch (verticalAlignment)
            {
                case EwVerticalAlignment.Top:
                    format.LineAlignment = StringAlignment.Near;
                    break;
                case EwVerticalAlignment.Center:
                    format.LineAlignment = StringAlignment.Center;
                    break;
                case EwVerticalAlignment.Bottom:
                    format.LineAlignment = StringAlignment.Far;
                    break;
            }

            SetRect(x, y, width, height);

            if (textFlow == EWTextFlow.OVERFLOW_BOUNDS)
            {
                var size = _graphics.MeasureString(value, font, (int) width, format);

                if (size.Height > _rect.Height)
                {
                    var difference = size.Height - _rect.Height;

                    switch (verticalAlignment)
                    {
                        case EwVerticalAlignment.Center:
                            _rect.Y -= difference / 2;
                            break;
                        case EwVerticalAlignment.Bottom:
                            _rect.Y -= difference;
                            break;
                    }

                    _rect.Height = size.Height;
                }
            }

            Draw(g => g.DrawString(value, font, CurrentState.TextBrush, _rect, format));
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
        }

        protected override void NativeRotate(float degrees, float radians, float x, float y)
        {
            CurrentState.NativeRotate(degrees, x, y);
        }

        protected override void NativeRotate(float degrees, float radians)
        {
            CurrentState.NativeRotate(degrees);
        }

        protected override void NativeScale(float sx, float sy)
        {
            CurrentState.NativeScale(sx, sy);
        }

        protected override void NativeTranslate(float tx, float ty)
        {
            CurrentState.NativeTranslate(tx, ty);
        }

        protected override void NativeConcatenateTransform(EWAffineTransform transform)
        {
            CurrentState.NativeConcatenateTransform(transform);
        }

        public override void SetShadow(EWSize offset, float blur, EWColor color, float zoom)
        {
            Logger.Debug("Not implemented");
        }

        protected override float NativeStrokeSize
        {
            set => CurrentState.StrokeWidth = value;
        }

        protected override void NativeSetStrokeDashPattern(float[] pattern, float strokeSize)
        {
        }

        public override void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            if (paint == null)
            {
                CurrentState.FillColor = Color.White;
                return;
            }

            if (paint.PaintType == EWPaintType.SOLID)
            {
                CurrentState.FillColor = paint.StartColor.AsColor();
                return;
            }

            if (paint.PaintType == EWPaintType.PATTERN)
            {
                CurrentState.StrokeColor = paint.ForegroundColor.AsColor();
                CurrentState.FillColor = paint.BackgroundColor.AsColor();
                return;
            }

/*            if (paint.PaintType == EWPaintType.LINEAR_GRADIENT)
            {
                point1.X = x1;
                point1.Y = y1;
                point2.X = x2;
                point2.Y = y2;
                currentState.SetLinearGradient(paint, point1, point2);
            }
            else
            {
                point1.X = x1;
                point1.Y = y1;
                point2.X = x2;
                point2.Y = y2;
                currentState.SetRadialGradient(paint, point1, point2);
            }*/
        }

        public override void SetToSystemFont()
        {
            CurrentState.FontName = "Arial";
        }

        public override void SetToBoldSystemFont()
        {
            CurrentState.FontName = "Arial";
            CurrentState.FontStyle = FontStyle.Bold;
        }

        public override void DrawImage(EWImage image, float x, float y, float width, float height)
        {
            if (image is GDIImage nativeImage)
            {
                _rectI.X = (int) x;
                _rectI.Y = (int) y;
                _rectI.Width = (int) width;
                _rectI.Height = (int) height;

                Draw(g => g.DrawImage(nativeImage.NativeImage, _rectI, 0f, 0f, image.Width, image.Height, GraphicsUnit.Pixel));
            }
        }

        private void SetRect(float x, float y, float width, float height)
        {
            _rect.X = x;
            _rect.Y = y;
            _rect.Width = width;
            _rect.Height = height;
        }

        private void Draw(Action<System.Drawing.Graphics> drawingAction)
        {
            if (CurrentState.IsShadowed)
            {
                DrawShadow(drawingAction);
            }

            if (CurrentState.IsBlurred)
            {
                DrawBlurred(drawingAction);
            }
            else
            {
                drawingAction(_graphics);
            }
        }

        private void DrawShadow(Action<System.Drawing.Graphics> drawingAction)
        {
        }

        private void DrawBlurred(Action<System.Drawing.Graphics> drawingAction)
        {
        }
    }
}