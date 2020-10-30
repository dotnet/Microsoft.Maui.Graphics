using System.Graphics.Text;
using System.Graphics.Text.Android;
using Android.Content;
using Android.Graphics;
using Android.Text;

namespace System.Graphics.Android
{
    public class MDCanvas : AbstractCanvas<MDCanvasState>
    {
        private static Paint _defaultFillPaint;
        private static TextPaint _defaultFontPaint;
        private static Paint _defaultStrokePaint;

        private Canvas _canvas;
        private Shader _shader;

        private readonly Matrix _shaderMatrix = new Matrix();

        public MDCanvas(Context context) : base(CreateNewState, CreateStateCopy)
        {
            DisplayScale = context?.Resources?.DisplayMetrics?.Density ?? 1;
        }

        private static MDCanvasState CreateNewState(object context)
        {
            if (_defaultFillPaint == null)
            {
                _defaultFillPaint = new Paint();
                _defaultFillPaint.SetARGB(255, 255, 255, 255);
                _defaultFillPaint.SetStyle(Paint.Style.Fill);
                _defaultFillPaint.AntiAlias = true;

                _defaultStrokePaint = new Paint();
                _defaultStrokePaint.SetARGB(255, 0, 0, 0);
                _defaultStrokePaint.StrokeWidth = 1;
                _defaultStrokePaint.StrokeMiter = CanvasDefaults.DefaultMiterLimit;
                _defaultStrokePaint.SetStyle(Paint.Style.Stroke);
                _defaultStrokePaint.AntiAlias = true;

                _defaultFontPaint = new TextPaint();
                _defaultFontPaint.SetARGB(255, 0, 0, 0);
                _defaultFontPaint.AntiAlias = true;

                var arial = MDFontService.Instance.GetTypeface("Arial");
                if (arial != null)
                    _defaultFontPaint.SetTypeface(arial);
                else
                    Logger.Warn("Unable to set the default font paint to Arial");
            }

            var state = new MDCanvasState
            {
                FillPaint = new Paint(_defaultFillPaint),
                StrokePaint = new Paint(_defaultStrokePaint),
                FontPaint = new TextPaint(_defaultFontPaint),
                FontName = MDFontService.SystemFont
            };

            return state;
        }

        private static MDCanvasState CreateStateCopy(MDCanvasState prototype)
        {
            return new MDCanvasState(prototype);
        }

        public override void Dispose()
        {
            _defaultFillPaint.Dispose();
            _defaultStrokePaint.Dispose();
            _defaultFontPaint.Dispose();

            base.Dispose();
        }
        
        public Canvas Canvas
        {
            get => _canvas;
            set
            {
                _canvas = null;
                ResetState();
                _canvas = value;
            }
        }

        public override bool Antialias
        {
            set => CurrentState.AntiAlias = value;
        }

        protected override float NativeStrokeSize
        {
            set => CurrentState.NativeStrokeSize = value;
        }

        public override float MiterLimit
        {
            set => CurrentState.MiterLimit = value;
        }

        public override float Alpha
        {
            set => CurrentState.Alpha = value;
        }

        public override EWLineCap StrokeLineCap
        {
            set => CurrentState.StrokeLineCap = value;
        }

        public override EWLineJoin StrokeLineJoin
        {
            set => CurrentState.StrokeLineJoin = value;
        }

        public override EWColor StrokeColor
        {
            set => CurrentState.StrokeColor = value ?? StandardColors.Black;
        }

        public override EWColor FontColor
        {
            set => CurrentState.FontColor = value ?? StandardColors.Black;
        }

        public override String FontName
        {
            set => CurrentState.FontName = value ?? MDFontService.SystemFont;
        }

        public override float FontSize
        {
            set => CurrentState.FontSize = value;
        }

        public override EWColor FillColor
        {
            set => CurrentState.FillColor = value ?? StandardColors.White;
        }

        public override BlendMode BlendMode
        {
            set
            {
                /* todo: implement this 
                CGBlendMode vBlendMode = CGBlendMode.Normal;
    
                switch (value)
                {
                    case EWBlendMode.Clear:
                        vBlendMode = CGBlendMode.Clear;
                        break;
                    case EWBlendMode.Color:
                        vBlendMode = CGBlendMode.Color;
                        break;
                    case EWBlendMode.ColorBurn:
                        vBlendMode = CGBlendMode.ColorBurn;
                        break;
                    case EWBlendMode.ColorDodge:
                        vBlendMode = CGBlendMode.ColorDodge;
                        break;
                    case EWBlendMode.Copy:
                        vBlendMode = CGBlendMode.Copy;
                        break;
                    case EWBlendMode.Darken:
                        vBlendMode = CGBlendMode.Darken;
                        break;
                    case EWBlendMode.DestinationAtop:
                        vBlendMode = CGBlendMode.DestinationAtop;
                        break;
                    case EWBlendMode.DestinationIn:
                        vBlendMode = CGBlendMode.DestinationIn;
                        break;
                    case EWBlendMode.DestinationOut:
                        vBlendMode = CGBlendMode.DestinationOut;
                        break;
                    case EWBlendMode.DestinationOver:
                        vBlendMode = CGBlendMode.DestinationOver;
                        break;
                    case EWBlendMode.Difference:
                        vBlendMode = CGBlendMode.Difference;
                        break;
                    case EWBlendMode.Exclusion:
                        vBlendMode = CGBlendMode.Exclusion;
                        break;
                    case EWBlendMode.HardLight:
                        vBlendMode = CGBlendMode.HardLight;
                        break;
                    case EWBlendMode.Hue:
                        vBlendMode = CGBlendMode.Hue;
                        break;
                    case EWBlendMode.Lighten:
                        vBlendMode = CGBlendMode.Lighten;
                        break;
                    case EWBlendMode.Luminosity:
                        vBlendMode = CGBlendMode.Luminosity;
                        break;
                    case EWBlendMode.Multiply:
                        vBlendMode = CGBlendMode.Multiply;
                        break;
                    case EWBlendMode.Normal:
                        vBlendMode = CGBlendMode.Normal;
                        break;
                    case EWBlendMode.Overlay:
                        vBlendMode = CGBlendMode.Overlay;
                        break;
                    case EWBlendMode.PlusDarker:
                        vBlendMode = CGBlendMode.PlusDarker;
                        break;
                    case EWBlendMode.PlusLighter:
                        vBlendMode = CGBlendMode.PlusLighter;
                        break;
                    case EWBlendMode.Saturation:
                        vBlendMode = CGBlendMode.Saturation;
                        break;
                    case EWBlendMode.Screen:
                        vBlendMode = CGBlendMode.Screen;
                        break;
                    case EWBlendMode.SoftLight:
                        vBlendMode = CGBlendMode.SoftLight;
                        break;
                    case EWBlendMode.SourceAtop:
                        vBlendMode = CGBlendMode.SourceAtop;
                        break;
                    case EWBlendMode.SourceIn:
                        vBlendMode = CGBlendMode.SourceIn;
                        break;
                    case EWBlendMode.SourceOut:
                        vBlendMode = CGBlendMode.SourceOut;
                        break;
                    case EWBlendMode.XOR:
                        vBlendMode = CGBlendMode.XOR;
                        break; 
                }
    
                canvas.SetBlendMode(vBlendMode);*/

                //CurrentState.FillPaint.SetXfermode(new 
            }
        }

        protected override void NativeSetStrokeDashPattern(float[] patter, float linewidth)
        {
            CurrentState.SetStrokeDashPattern(patter, linewidth);
        }

        public override void SetToSystemFont()
        {
            CurrentState.FontName = MDFontService.SystemFont;
        }

        public override void SetToBoldSystemFont()
        {
            CurrentState.FontName = MDFontService.SystemBoldFont;
        }

        public override void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            if (paint == null)
                paint = StandardColors.White.AsPaint();

            if (_shader != null)
            {
                CurrentState.SetFillPaintShader(null);
                _shader.Dispose();
                _shader = null;
            }

            if (paint.PaintType == EWPaintType.LINEAR_GRADIENT)
            {
                var colors = new int[paint.Stops.Length];
                var stops = new float[colors.Length];

                EWPaintStop[] vStops = paint.GetSortedStops();

                for (int i = 0; i < vStops.Length; i++)
                {
                    colors[i] = vStops[i].Color.ToArgb(CurrentState.Alpha);
                    stops[i] = vStops[i].Offset;
                }

                try
                {
                    CurrentState.FillColor = StandardColors.White;
                    _shader = new LinearGradient(x1, y1, x2, y2, colors, stops, Shader.TileMode.Clamp);
                    CurrentState.SetFillPaintShader(_shader);
                }
                catch (Exception exc)
                {
                    Logger.Debug(exc);
                    FillColor = paint.BlendStartAndEndColors();
                }
            }
            else if (paint.PaintType == EWPaintType.RADIAL_GRADIENT)
            {
                var colors = new int[paint.Stops.Length];
                var stops = new float[colors.Length];

                EWPaintStop[] vStops = paint.GetSortedStops();

                for (int i = 0; i < vStops.Length; i++)
                {
                    colors[i] = vStops[i].Color.ToArgb(CurrentState.Alpha);
                    stops[i] = vStops[i].Offset;
                }

                float r = Geometry.GetDistance(x1, y1, x2, y2);
                try
                {
                    CurrentState.FillColor = StandardColors.White;
                    _shader = new RadialGradient(x1, y1, r, colors, stops, Shader.TileMode.Clamp);
                    CurrentState.SetFillPaintShader(_shader);
                }
                catch (Exception exc)
                {
                    Logger.Debug(exc);
                    FillColor = paint.BlendStartAndEndColors();
                }
            }
            else if (paint.PaintType == EWPaintType.PATTERN)
            {
                var bitmap = paint.GetPatternBitmap(DisplayScale);

                if (bitmap != null)
                {
                    try
                    {
                        CurrentState.FillColor = StandardColors.White;
                        CurrentState.SetFillPaintFilterBitmap(true);

                        _shader = new BitmapShader(bitmap, Shader.TileMode.Repeat, Shader.TileMode.Repeat);
                        _shaderMatrix.Reset();
                        _shaderMatrix.PreScale(CurrentState.ScaleX, CurrentState.ScaleY);
                        _shader.SetLocalMatrix(_shaderMatrix);

                        CurrentState.SetFillPaintShader(_shader);
                    }
                    catch (Exception exc)
                    {
                        Logger.Debug(exc);
                        FillColor = paint.BackgroundColor;
                    }
                }
                else
                {
                    FillColor = paint.BackgroundColor;
                }
            }
            else if (paint.PaintType == EWPaintType.IMAGE)
            {
                if (paint.Image is MDImage image)
                {
                    var bitmap = image.NativeImage;

                    if (bitmap != null)
                    {
                        try
                        {
                            CurrentState.FillColor = StandardColors.White;
                            CurrentState.SetFillPaintFilterBitmap(true);

                            _shader = new BitmapShader(bitmap, Shader.TileMode.Repeat, Shader.TileMode.Repeat);
                            _shaderMatrix.Reset();
                            _shaderMatrix.PreScale(CurrentState.ScaleX, CurrentState.ScaleY);
                            _shader.SetLocalMatrix(_shaderMatrix);

                            CurrentState.SetFillPaintShader(_shader);
                        }
                        catch (Exception exc)
                        {
                            Logger.Debug(exc);
                            FillColor = paint.BackgroundColor;
                        }
                    }
                    else
                        FillColor = StandardColors.White;
                }
                else
                {
                    FillColor = StandardColors.White;
                }
            }
            else
                FillColor = paint.StartColor;

            //Logger.Debug("Gradient Set To: "+aPaint.PaintType);
        }

        protected override void NativeDrawLine(float x1, float y1, float x2, float y2)
        {
            //canvas.DrawLine (x1, y1, x2, y2, CurrentState.StrokePaintWithAlpha);

            var nativePath = new Path();
            nativePath.MoveTo(x1, y1);
            nativePath.LineTo(x2, y2);
            _canvas.DrawPath(nativePath, CurrentState.StrokePaintWithAlpha);
            nativePath.Dispose();
        }

        protected override void NativeDrawArc(
            float x,
            float y,
            float width,
            float height,
            float startAngle,
            float endAngle,
            bool clockwise,
            bool closed)
        {
            while (startAngle < 0)
            {
                startAngle += 360;
            }

            while (endAngle < 0)
            {
                endAngle += 360;
            }

            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            float sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);

            var rect = new RectF(rectX, rectY, rectX + rectWidth, rectY + rectHeight);

            startAngle *= -1;
            if (!clockwise)
            {
                sweep *= -1;
            }

            if (closed)
            {
                var nativePath = new Path();
                nativePath.AddArc(rect, startAngle, sweep);
                nativePath.Close();
                _canvas.DrawPath(nativePath, CurrentState.StrokePaintWithAlpha);
                nativePath.Dispose();
            }
            else
            {
                _canvas.DrawArc(rect, startAngle, sweep, false, CurrentState.StrokePaintWithAlpha);
            }

            rect.Dispose();
        }

        public override void FillArc(
            float x,
            float y,
            float width,
            float height,
            float startAngle,
            float endAngle,
            bool clockwise)
        {
            while (startAngle < 0)
            {
                startAngle += 360;
            }

            while (endAngle < 0)
            {
                endAngle += 360;
            }

            var sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);
            var rect = new RectF(x, y, x + width, y + height);

            startAngle *= -1;
            if (!clockwise)
            {
                sweep *= -1;
            }

            _canvas.DrawArc(rect, startAngle, sweep, false, CurrentState.FillPaintWithAlpha);
            rect.Dispose();
        }

        protected override void NativeDrawRectangle(float x, float y, float width, float height)
        {
            float rectX = 0, rectY = 0, rectWidth = 0, rectHeight = 0;

            float strokeSize = CurrentState.ScaledStrokeSize;
            if (strokeSize == 0)
                return;

            var strokeLocation = CurrentState.StrokeLocation;

            if (strokeLocation == EWStrokeLocation.CENTER)
            {
                rectX = x;
                rectY = y;
                rectWidth = width;
                rectHeight = height;
            }
            else if (strokeLocation == EWStrokeLocation.INSIDE)
            {
                rectX = x + strokeSize / 2;
                rectY = y + strokeSize / 2;
                rectWidth = width - strokeSize;
                rectHeight = height - strokeSize;
            }
            else if (strokeLocation == EWStrokeLocation.OUTSIDE)
            {
                rectX = x - strokeSize / 2;
                rectY = y - strokeSize / 2;
                rectWidth = width + strokeSize;
                rectHeight = height + strokeSize;
            }

            _canvas.DrawRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight, CurrentState.StrokePaintWithAlpha);
        }

        public override void FillRectangle(float x, float y, float width, float height)
        {
            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            _canvas.DrawRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight, CurrentState.FillPaintWithAlpha);
        }

        protected override void NativeDrawRoundedRectangle(
            float x,
            float y,
            float width,
            float height,
            float aCornerRadius)
        {
            // These values work for a stroke location of center.
            float strokeSize = CurrentState.ScaledStrokeSize;

            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;
            var radius = aCornerRadius;

            var strokeLocation = CurrentState.StrokeLocation;
            if (strokeLocation == EWStrokeLocation.INSIDE)
            {
                rectX += strokeSize / 2;
                rectY += strokeSize / 2;
                rectWidth -= strokeSize;
                rectHeight -= strokeSize;
                radius -= strokeSize / 2;
            }
            else if (strokeLocation == EWStrokeLocation.OUTSIDE)
            {
                rectX -= strokeSize / 2;
                rectY -= strokeSize / 2;
                rectWidth += strokeSize;
                rectHeight += strokeSize;
                radius += strokeSize / 2;
            }

            var rect = new RectF(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawRoundRect(rect, radius, radius, CurrentState.StrokePaintWithAlpha);
            rect.Dispose();
        }

        public override void FillRoundedRectangle(float x, float y, float width, float height, float aCornerRadius)
        {
            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;
            var radius = aCornerRadius;

            var rect = new RectF(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawRoundRect(rect, radius, radius, CurrentState.FillPaintWithAlpha);
            rect.Dispose();
        }

        protected override void NativeDrawOval(float x, float y, float width, float height)
        {
            // These values work for a stroke location of center.
            float strokeSize = CurrentState.ScaledStrokeSize;

            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            var strokeLocation = CurrentState.StrokeLocation;
            if (strokeLocation == EWStrokeLocation.INSIDE)
            {
                rectX += strokeSize / 2;
                rectY += strokeSize / 2;
                rectWidth -= strokeSize;
                rectHeight -= strokeSize;
            }
            else if (strokeLocation == EWStrokeLocation.OUTSIDE)
            {
                rectX -= strokeSize / 2;
                rectY -= strokeSize / 2;
                rectWidth += strokeSize;
                rectHeight += strokeSize;
            }

            var rect = new RectF(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawOval(rect, CurrentState.StrokePaintWithAlpha);
            rect.Dispose();
        }

        public override void FillOval(float x, float y, float width, float height)
        {
            /* todo: support gradients here */

            // These values work for a stroke location of center.
            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            var rect = new RectF(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawOval(rect, CurrentState.FillPaintWithAlpha);
            rect.Dispose();
        }

        public override void SubtractFromClip(float x, float y, float width, float height)
        {
            _canvas.ClipRect(x, y, x + width, y + height, Region.Op.Difference);
        }

        protected override void NativeDrawPath(EWPath aPath)
        {
            var strokeLocation = CurrentState.StrokeLocation;
            if (strokeLocation == EWStrokeLocation.CENTER)
            {
                var nativePath = aPath.AsAndroidPath();
                _canvas.DrawPath(nativePath, CurrentState.StrokePaintWithAlpha);
                nativePath.Dispose();
            }
            else if (strokeLocation == EWStrokeLocation.INSIDE)
            {
                _canvas.Save(SaveFlags.Clip);
                var nativePath = aPath.AsAndroidPath();
                _canvas.ClipPath(nativePath);
                var paint = CurrentState.StrokePaintWithAlpha;
                float strokeSize = paint.StrokeWidth;
                paint.StrokeWidth = strokeSize * 2;
                _canvas.DrawPath(nativePath, paint);
                paint.StrokeWidth = strokeSize;
                _canvas.Restore();
                nativePath.Dispose();
            }
            else if (strokeLocation == EWStrokeLocation.OUTSIDE)
            {
                var origClip = _canvas.ClipBounds;
                var nativePath = aPath.AsAndroidPath();

                var clippingPath = new Path();
                var origClipAsRect = origClip.AsRectF();
                clippingPath.AddRect(origClipAsRect, Path.Direction.Cw);
                clippingPath.AddPath(nativePath);

                _canvas.Save();
                _canvas.ClipPath(clippingPath, Region.Op.ReverseDifference);

                var paint = CurrentState.StrokePaintWithAlpha;
                float vStrokeSize = paint.StrokeWidth;
                paint.StrokeWidth = vStrokeSize * 2;
                _canvas.DrawPath(nativePath, paint);
                paint.StrokeWidth = vStrokeSize;
                _canvas.Restore();

                origClipAsRect.Dispose();
                nativePath.Dispose();
                clippingPath.Dispose();
            }
        }

        public override void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            var nativePath = path.AsAndroidPath();
            nativePath.SetFillType(windingMode == EWWindingMode.NonZero ? Path.FillType.Winding : Path.FillType.EvenOdd);
            _canvas.ClipPath(nativePath);
        }

        public override void ClipRectangle(float x, float y, float width, float height)
        {
            _canvas.ClipRect(x, y, x + width, y + height);
        }

        public override void FillPath(EWPath path, EWWindingMode windingMode)
        {
            var nativePath = path.AsAndroidPath();
            nativePath.SetFillType(windingMode == EWWindingMode.NonZero ? Path.FillType.Winding : Path.FillType.EvenOdd);
            _canvas.DrawPath(nativePath, CurrentState.FillPaintWithAlpha);
            nativePath.Dispose();
        }

        public override void DrawString(string value, float x, float y, EwHorizontalAlignment horizAlignment)
        {
            if (horizAlignment == EwHorizontalAlignment.Left)
                DrawString(value, x, y);
            else if (horizAlignment == EwHorizontalAlignment.Right)
            {
                EWSize vSize = MDGraphicsService.Instance.GetStringSize(
                    value,
                    CurrentState.FontName,
                    CurrentState.ScaledFontSize);
                x -= vSize.Width;
                DrawString(value, x, y);
            }
            else
            {
                EWSize vSize = MDGraphicsService.Instance.GetStringSize(
                    value,
                    CurrentState.FontName,
                    CurrentState.ScaledFontSize);
                x -= vSize.Width / 2f;
                DrawString(value, x, y);
            }
        }

        private void DrawString(string value, float x, float y)
        {
            if (value == null)
                return;

            _canvas.Save();
            _canvas.Translate(x, y - CurrentState.ScaledFontSize);
            var layout = new StaticLayout(
                value,
                CurrentState.FontPaint,
                512,
                Layout.Alignment.AlignNormal,
                1f,
                0f,
                false);
            layout.Draw(_canvas);
            _canvas.Restore();
        }

        public override void DrawString(
            string value,
            float x,
            float y,
            float width,
            float height,
            EwHorizontalAlignment horizAlignment,
            EwVerticalAlignment vertAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS,
            float lineSpacingAdjustment = 0)
        {
            if (value == null || value.Length == 0 || width == 0 || height == 0)
                return;

            _canvas.Save();

            var alignment = Layout.Alignment.AlignNormal;
            if (horizAlignment == EwHorizontalAlignment.Center)
            {
                alignment = Layout.Alignment.AlignCenter;
            }
            else if (horizAlignment == EwHorizontalAlignment.Right)
            {
                alignment = Layout.Alignment.AlignOpposite;
            }

            var layout = MDTextLayout.CreateLayout(value, CurrentState.FontPaint, (int) width, alignment);
            var offset = layout.GetOffsetsToDrawText(x, y, width, height, horizAlignment, vertAlignment);
            _canvas.Translate(offset.Width, offset.Height);
            layout.Draw(_canvas);
            _canvas.Restore();
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            _canvas.Save();
            var span = value.AsSpannableString();
            var layout = MDTextLayout.CreateLayoutForSpannedString(span, CurrentState.FontPaint, (int) width, Layout.Alignment.AlignNormal);
            var offset = layout.GetOffsetsToDrawText(x, y, width, height, EwHorizontalAlignment.Left, EwVerticalAlignment.Top);
            _canvas.Translate(offset.Width, offset.Height);
            layout.Draw(_canvas);
            layout.Dispose();
            span.Dispose();
            _canvas.Restore();
        }

        public override void ResetState()
        {
            base.ResetState();

            if (_shader != null)
            {
                _shader.Dispose();
                _shader = null;
            }

            CurrentState.Reset(_defaultFontPaint, _defaultFillPaint, _defaultStrokePaint);
        }

        public override bool RestoreState()
        {
            if (_shader != null)
            {
                _shader.Dispose();
                _shader = null;
            }

            return base.RestoreState();
        }

        protected override void StateRestored(MDCanvasState state)
        {
            _canvas.Restore();
        }

        public override void SetShadow(EWSize offset, float blur, EWColor color)
        {
            EWSize actualOffset = offset ?? CanvasDefaults.DefaultShadowOffset;

            var sx = actualOffset.Width;
            var sy = actualOffset.Height;
            
            if (color == null)
            {
                var actualColor = StandardColors.Black.AsColorMultiplyAlpha(CurrentState.Alpha);
                CurrentState.SetShadow(blur, sx, sy, actualColor);
            }
            else
            {
                var actualColor = color.AsColorMultiplyAlpha(CurrentState.Alpha);
                CurrentState.SetShadow(blur, sx, sy, actualColor);
            }
        }

        protected override void NativeRotate(float degrees, float radians, float x, float y)
        {
            _canvas.Rotate(degrees, x, y);
        }

        protected override void NativeRotate(float degrees, float radians)
        {
            _canvas.Rotate(degrees);
        }

        protected override void NativeScale(float xFactor, float yFactor)
        {
            CurrentState.SetScale(Math.Abs(xFactor), Math.Abs(yFactor));
            if (xFactor < 0 || yFactor < 0)
                _canvas.Scale(xFactor < 0 ? -1 : 1, yFactor < 0 ? -1 : 1);
        }

        protected override void NativeTranslate(float tx, float ty)
        {
            _canvas.Translate(tx * CurrentState.ScaleX, ty * CurrentState.ScaleY);
        }

        protected override void NativeConcatenateTransform(AffineTransform transform)
        {
            var matrix = new Matrix(_canvas.Matrix);
            matrix.PostConcat(transform.AsMatrix());
            _canvas.Matrix = matrix;
        }

        public override void SaveState()
        {
            _canvas.Save();
            base.SaveState();
        }

        public void SetBlur(float radius)
        {
            CurrentState.SetBlur(radius);
        }

        public override void DrawImage(EWImage image, float x, float y, float width, float height)
        {
            if (image is MDImage mdimage)
            {
                var bitmap = mdimage.NativeImage;
                if (bitmap != null)
                {
                    var scaleX = CurrentState.ScaleX < 0 ? -1 : 1;
                    var scaleY = CurrentState.ScaleY < 0 ? -1 : 1;

                    _canvas.Save();
                    //canvas.Scale (scaleX, scaleY);
                    var srcRect = new Rect(0, 0, bitmap.Width, bitmap.Height);

                    x *= scaleX;
                    y *= scaleY;
                    width *= scaleX;
                    height *= scaleY;

                    var rx1 = Math.Min(x, x + width);
                    var ry1 = Math.Min(y, y + height);
                    var rx2 = Math.Max(x, x + width);
                    var ry2 = Math.Max(y, y + height);

                    var destRect = new RectF(rx1, ry1, rx2, ry2);
                    var paint = CurrentState.GetImagePaint(1, 1);
                    _canvas.DrawBitmap(bitmap, srcRect, destRect, paint);
                    paint?.Dispose();
                    _canvas.Restore();
                }
            }
        }
    }
}