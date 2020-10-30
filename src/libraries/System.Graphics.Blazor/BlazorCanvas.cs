using System.Graphics.Blazor.Canvas2D;
using System.Graphics.Text;

namespace System.Graphics.Blazor
{
    public class BlazorCanvas : AbstractCanvas<BlazorCanvasState>
    {
        private readonly float[] _matrix = new float[6];
        private readonly EWRectangle _dirtyRect = new EWRectangle();
        private CanvasRenderingContext2D _context;
        
        public BlazorCanvas() : base(CreateNewState, CreateStateCopy)
        {
        }

        private static BlazorCanvasState CreateNewState(object context)
        {
            return new BlazorCanvasState();
        }

        private static BlazorCanvasState CreateStateCopy(BlazorCanvasState prototype)
        {
            return new BlazorCanvasState(prototype);
        }

        public CanvasRenderingContext2D Context
        {
            get => _context;
            set => _context = value;
        }

        public override float MiterLimit { set => CurrentState.MiterLimit = value; }
        public override EWColor StrokeColor { set => CurrentState.StrokeColor = value; }
        public override EWLineCap StrokeLineCap { set => CurrentState.LineCap = value; }
        public override EWLineJoin StrokeLineJoin { set => CurrentState.LineJoin = value; }
        public override EWColor FillColor { set => CurrentState.FillColor = value; }
        public override EWColor FontColor { set => CurrentState.TextColor = value; }
        public override string FontName { set => CurrentState.Font = value; }
        public override float FontSize { set => CurrentState.FontSize = value; }
        public override float Alpha { set => _context.GlobalAlpha = value; }
        public override bool Antialias { set { /* do nothing */} }
        public override EWBlendMode BlendMode { set { /* do nothing */ } }

        public void ClearRect(float x1, float y1, float width, float height)
        {
            _context.ClearRect(x1, y1, width, height);
            _dirtyRect.X1 = x1;
            _dirtyRect.Y1 = y1;
            _dirtyRect.Width = width;
            _dirtyRect.Height = height;
        }

        protected override float NativeStrokeSize { set => CurrentState.LineWidth = value; }

        public override void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            AddPath(path,1,1);
            _context.Clip(windingMode == EWWindingMode.NonZero ? "nonzero" : "evenodd");
        }

        public override void ClipRectangle(float x, float y, float width, float height)
        {
            _context.Rect(x,y,width,height);
            _context.Clip();
        }

        public override void DrawImage(EWImage image, float x, float y, float width, float height)
        {
            Logger.Debug("BlazorCanvas.DrawImage - not yet supported.");
        }

        private EWSize GetTextSize(TextMetrics metrics, string value)
        {
            var lines = value.Split('\n');

            var w = metrics.ActualBoundingBoxRight - metrics.ActualBoundingBoxLeft;
            var h = lines.Length * (metrics.ActualBoundingBoxAscent + metrics.ActualBoundingBoxDescent);
            
            return new EWSize((float)w,(float)h);
        }
        
        public override void DrawString(
            string value, 
            float x, 
            float y, 
            EwHorizontalAlignment horizontalAlignment)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var alpha = CurrentState.SetTextStyle(_context);
            var metrics = _context.MeasureText(value);

            y += (float)(metrics.FontBoundingBoxAscent + metrics.FontBoundingBoxDescent);
            
            if (horizontalAlignment == EwHorizontalAlignment.Right)
            {
                x -= (float)metrics.Width;
            }
            else if (horizontalAlignment == EwHorizontalAlignment.Center)
            {
                x -= (float)(metrics.Width / 2);
            }
    
            _context.FillText(value, x, y);
            _context.GlobalAlpha = alpha;
        }

        public override void DrawString(
            string value, 
            float x, 
            float y, 
            float width, 
            float height, 
            EwHorizontalAlignment horizontalAlignment, 
            EwVerticalAlignment verticalAlignment, 
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, 
            float lineSpacingAdjustment = 0)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var alpha = CurrentState.SetTextStyle(_context);
            var metrics = _context.MeasureText(value);
            var size = GetTextSize(metrics, value);

            float dx = 0;
            float dy = 0;

            if (horizontalAlignment == EwHorizontalAlignment.Center)
            {
                var diff = width - size.Width;
                dx = diff / 4;
            }
            else if (horizontalAlignment == EwHorizontalAlignment.Right)
            {
                var diff = width - size.Width;
                dx = diff / 2;
            }

            if (verticalAlignment == EwVerticalAlignment.Top)
                dy = CurrentState.FontSize * .8f;
            else if (verticalAlignment == EwVerticalAlignment.Center)
                dy = ((height - size.Height) / 2) + (CurrentState.FontSize *.6f / 2);
            else if (verticalAlignment == EwVerticalAlignment.Bottom)
                dy = height - size.Height - CurrentState.FontSize * .3f;
               
            x += dx;
            y += dy;
            
            _context.FillText(value, x, y, width);
            _context.GlobalAlpha = alpha;
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            Logger.Debug("BlazorCanvas.DrawText - not yet supported.");
        }

        public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            var alpha = CurrentState.SetFillStyle(_context);
            _context.BeginPath();
            _context.Arc(x, y, width / 2, startAngle, endAngle, !clockwise);
            _context.Fill();
            _context.GlobalAlpha = alpha;
        }

        public override void FillOval(float x, float y, float width, float height)
        {
            var alpha = CurrentState.SetFillStyle(_context);
            _context.BeginPath();
            _context.Ellipse(x + (width / 2), y + (height / 2), width / 2, height / 2, 0, 0, 2 * Math.PI, true);
            _context.Fill();
            _context.GlobalAlpha = alpha;
        }

        public override void FillPath(EWPath path, EWWindingMode windingMode)
        {
            var alpha = CurrentState.SetFillStyle(_context);
            _context.BeginPath();
            AddPath(path,1,1);
            _context.Fill();
            _context.GlobalAlpha = alpha;
        }

        public override void FillRectangle(float x, float y, float width, float height)
        {
            var alpha = CurrentState.SetFillStyle(_context);
            _context.FillRect(x, y, width, height);
            _context.GlobalAlpha = alpha;
        }

        public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            var alpha = CurrentState.SetFillStyle(_context);
            _context.BeginPath();
            AddRoundedRectangle(x, y, width, height, cornerRadius);
            _context.Fill();
            _context.GlobalAlpha = alpha;
        }

        public override void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            if (paint.PaintType == EWPaintType.SOLID)
            {
                FillColor = paint.StartColor;
            }
            else
            {
                CurrentState.SetFillPaint(paint, x1, y1, x2, y2);
            }
        }

        public override void SetShadow(EWSize offset, float blur, EWColor color, float zoom)
        {
            Logger.Debug("BlazorCanvas.SetShadow - not yet supported.");
        }

        public override void SetToBoldSystemFont()
        {
            FontName = "Arial";
        }

        public override void SetToSystemFont()
        {
            FontName = "Arial-Bold";
        }

        public override void SubtractFromClip(float x, float y, float width, float height)
        {
            _context.Rect(_dirtyRect.X1, _dirtyRect.Y1, _dirtyRect.Width, _dirtyRect.Width);
            _context.Rect(x,y,width,height);
            _context.Clip("evenodd");
        }
        
        protected override void NativeConcatenateTransform(EWAffineTransform transform)
        {
            transform.GetMatrix(_matrix);
            _context.SetTransform(_matrix[0], _matrix[1], _matrix[2], _matrix[3], _matrix[4], _matrix[5]);
        }

        protected override void NativeDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            var alpha = CurrentState.SetStrokeStyle(_context);
            _context.BeginPath();
            _context.Arc(x, y, width / 2, startAngle, endAngle, !clockwise);
            _context.Stroke();
            _context.GlobalAlpha = alpha;
        }

        protected override void NativeDrawLine(float x1, float y1, float x2, float y2)
        {
            var alpha = CurrentState.SetStrokeStyle(_context);

            _context.BeginPath();
            _context.MoveTo(x1, y1);
            _context.LineTo(x2, y2);
            _context.Stroke();

            _context.GlobalAlpha = alpha;
        }

        protected override void NativeDrawOval(float x, float y, float width, float height)
        {
            var alpha = CurrentState.SetStrokeStyle(_context);

            _context.BeginPath();
            _context.Ellipse(x + (width / 2), y + (height / 2), width / 2, height / 2, 0, 0, 2 * Math.PI, true);
            _context.Stroke();

            _context.GlobalAlpha = alpha;
        }

        protected override void NativeDrawPath(EWPath path)
        {
            var alpha = CurrentState.SetStrokeStyle(_context);
            _context.BeginPath();
            AddPath(path, 1, 1);
            _context.Stroke();
            _context.GlobalAlpha = alpha;
        }

        protected override void NativeDrawRectangle(float x, float y, float width, float height)
        {
            var alpha = CurrentState.SetStrokeStyle(_context);
            _context.StrokeRect(x, y, width, height);
            _context.GlobalAlpha = alpha;
        }

        protected override void NativeDrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            var alpha = CurrentState.SetStrokeStyle(_context);

            _context.BeginPath();
            AddRoundedRectangle(x, y, width, height, cornerRadius);
            _context.Stroke();

            _context.GlobalAlpha = alpha;
        }

        protected override void NativeRotate(float degrees, float radians, float x, float y)
        {
            _context.Translate(x, y);
            _context.Rotate(radians);
            _context.Translate(-x, -y);
        }

        protected override void NativeRotate(float degrees, float radians)
        {
            _context.Rotate(radians);
        }

        protected override void NativeScale(float fx, float fy)
        {
            _context.Scale(fx, fy);
        }

        protected override void NativeSetStrokeDashPattern(float[] pattern, float strokeSize)
        {
            float[] finalPattern = null;
            if (pattern != null)
            {
                finalPattern = new float[pattern.Length];
                for (int i = 0; i < pattern.Length; i++)
                    finalPattern[i] = pattern[i] * strokeSize;
            }

            CurrentState.BlazorDashPattern = finalPattern;
        }

        protected override void NativeTranslate(float tx, float ty)
        {
            _context.Translate(tx, ty);
        }

        public override void ResetState()
        {
            base.ResetState();
            _context.ResetTransform();
        }

        public override bool RestoreState()
        {
            var result = base.RestoreState();
            if (result)
            {
                _context.Restore();
                CurrentState.Restore();
            }

            return result;
        }

        public override void SaveState()
        {
            base.SaveState();
            _context.Save();
        }

        public void AddRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            var finalCornerRadius = cornerRadius;

            var rect = new EWRectangle(x, y, width, height);

            if (finalCornerRadius > rect.Width)
            {
                finalCornerRadius = rect.Width / 2;
            }

            if (finalCornerRadius > rect.Height)
            {
                finalCornerRadius = rect.Height / 2;
            }

            var minx = rect.X1;
            var miny = rect.Y1;
            var maxx = minx + rect.Width;
            var maxy = miny + rect.Height;
            var midx = minx + (rect.Width / 2);
            var midy = miny + (rect.Height / 2);

            _context.MoveTo(minx, midy);
            _context.ArcTo(minx, miny, midx, miny, finalCornerRadius);
            _context.ArcTo(maxx, miny, maxx, midy, finalCornerRadius);
            _context.ArcTo(maxx, maxy, midx, maxy, finalCornerRadius);
            _context.ArcTo(minx, maxy, minx, midy, finalCornerRadius);
            _context.ClosePath();
        }

        private void AddPath(EWPath target, float scaleX, float scaleY)
        {
            int pointIndex = 0;
            int arcAngleIndex = 0;
            int arcClockwiseIndex = 0;

            foreach (var type in target.SegmentTypes)
            {
                if (type == PathOperation.MOVE_TO)
                {
                    var point = target[pointIndex++];
                    _context.MoveTo((point.X * scaleX), (point.Y * scaleY));
                }
                else if (type == PathOperation.LINE)
                {
                    var endPoint = target[pointIndex++];
                    _context.LineTo((endPoint.X * scaleX), (endPoint.Y * scaleY));
                }

                else if (type == PathOperation.QUAD)
                {
                    var controlPoint = target[pointIndex++];
                    var endPoint = target[pointIndex++];
                    _context.QuadraticCurveTo(
                        (controlPoint.X * scaleX), (controlPoint.Y * scaleY),
                        (endPoint.X * scaleX), (endPoint.Y * scaleY));
                }
                else if (type == PathOperation.CUBIC)
                {
                    var controlPoint1 = target[pointIndex++];
                    var controlPoint2 = target[pointIndex++];
                    var endPoint = target[pointIndex++];
                    _context.BezierCurveTo(
                        (controlPoint1.X * scaleX), (controlPoint1.Y * scaleY),
                        (controlPoint2.X * scaleX), (controlPoint2.Y * scaleY),
                        (endPoint.X * scaleX), (endPoint.Y * scaleY));
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
                        startAngleInRadians += (float)Math.PI * 2;
                    }

                    while (endAngleInRadians < 0)
                    {
                        endAngleInRadians += (float)Math.PI * 2;
                    }

                    var cx = (bottomRight.X + topLeft.X) / 2;
                    var cy = (bottomRight.Y + topLeft.Y) / 2;
                    var width = bottomRight.X - topLeft.X;
                    var height = bottomRight.Y - topLeft.Y;
                    var r = width / 2;

                    /*var transform = CGAffineTransform.MakeTranslation(ox + cx * ppu, oy + cy * ppu);
                    transform = CGAffineTransform.Multiply(CGAffineTransform.MakeScale(1, height / width), transform);
                    */

                    _context.Arc(cx, cy, r, startAngle, endAngle, !clockwise);
                }
                else if (type == PathOperation.CLOSE)
                {
                    _context.ClosePath();
                }
            }
        }
    }
}
