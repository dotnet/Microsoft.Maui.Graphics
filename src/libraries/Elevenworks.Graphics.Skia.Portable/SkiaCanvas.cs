using System;
using Elevenworks.Text;
using SkiaSharp;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public class SkiaCanvas : AbstractCanvas<SkiaCanvasState>, BlurrableCanvas
    {
        private static SKPaint _defaultFillPaint;
        private static SKPaint _defaultFontPaint;
        private static SKPaint _defaultStrokePaint;

        private readonly SKMatrix _shaderMatrix = new SKMatrix();

        private SKCanvas _canvas;
        private float _displayScale = 1;
        private SKShader _shader;

        public SkiaCanvas() : base(CreateNewState, CreateStateCopy)
        {
        }

        public override bool PixelShifted { get; set; }

        public override float DisplayScale => _displayScale;
        
        public SKCanvas Canvas
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

        public override string FontName
        {
            set
            {
                if (value != null)
                    CurrentState.FontName = value;
                else
                    CurrentState.FontName = SkiaGraphicsService.Instance.SystemFontName;
            }
        }

        public override float FontSize
        {
            set => CurrentState.FontSize = value;
        }

        public override EWColor FillColor
        {
            set
            {
                if (_shader != null)
                {
                    CurrentState.SetFillPaintShader(null);
                    _shader.Dispose();
                    _shader = null;
                }

                CurrentState.FillColor = value ?? StandardColors.White;
            }
        }

        public override EWBlendMode BlendMode
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

        private static SkiaCanvasState CreateNewState(object context)
        {
            if (_defaultFillPaint == null)
            {
                _defaultFillPaint = new SKPaint
                {
                    Color = SKColors.White,
                    IsStroke = false,
                    IsAntialias = true
                };

                _defaultStrokePaint = new SKPaint
                {
                    Color = SKColors.Black,
                    StrokeWidth = 1,
                    StrokeMiter = DefaultMiterLimit,
                    IsStroke = true,
                    IsAntialias = true
                };

                _defaultFontPaint = new SKPaint
                {
                    Color = SKColors.Black,
                    IsAntialias = true,
                    Typeface = SKTypeface.FromFamilyName("Arial")
                };
            }

            var state = new SkiaCanvasState
            {
                FillPaint = _defaultFillPaint.CreateCopy(),
                StrokePaint = _defaultStrokePaint.CreateCopy(),
                FontPaint = _defaultFontPaint.CreateCopy(),
                FontName = SkiaGraphicsService.Instance.SystemFontName
            };

            return state;
        }

        public void SetDisplayScale(float value)
        {
            _displayScale = value;
        }

        private static SkiaCanvasState CreateStateCopy(SkiaCanvasState prototype)
        {
            return new SkiaCanvasState(prototype);
        }

        public override void Dispose()
        {
            _defaultFillPaint.Dispose();
            _defaultStrokePaint.Dispose();
            _defaultFontPaint.Dispose();

            base.Dispose();
        }

        protected override void NativeSetStrokeDashPattern(
            float[] pattern,
            float strokeSize)
        {
            CurrentState.SetStrokeDashPattern(pattern, strokeSize);
        }

        public override void SetToSystemFont()
        {
            CurrentState.FontName = SkiaGraphicsService.Instance.SystemFontName;
        }

        public override void SetToBoldSystemFont()
        {
            CurrentState.FontName = SkiaGraphicsService.Instance.BoldSystemFontName;
        }

        public override void SetFillPaint(
            EWPaint paint,
            float x1,
            float y1,
            float x2,
            float y2)
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
                var colors = new SKColor[paint.Stops.Length];
                var stops = new float[colors.Length];

                var vStops = paint.GetSortedStops();

                for (var i = 0; i < vStops.Length; i++)
                {
                    colors[i] = vStops[i].Color.ToColor(CurrentState.Alpha);
                    stops[i] = vStops[i].Offset;
                }

                try
                {
                    CurrentState.FillColor = StandardColors.White;
                    _shader = SKShader.CreateLinearGradient(
                        new SKPoint(x1, y1),
                        new SKPoint(x2, y2),
                        colors,
                        stops,
                        SKShaderTileMode.Clamp);
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
                var colors = new SKColor[paint.Stops.Length];
                var stops = new float[colors.Length];

                var vStops = paint.GetSortedStops();

                for (var i = 0; i < vStops.Length; i++)
                {
                    colors[i] = vStops[i].Color.ToColor(CurrentState.Alpha);
                    stops[i] = vStops[i].Offset;
                }

                var r = Geometry.GetDistance(x1, y1, x2, y2);
                try
                {
                    CurrentState.FillColor = StandardColors.White;
                    _shader = SKShader.CreateRadialGradient(
                        new SKPoint(x1, y1),
                        r,
                        colors,
                        stops,
                        SKShaderTileMode.Clamp);
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
                SKBitmap bitmap = paint.GetPatternBitmap(CurrentFigure, DisplayScale);

                if (bitmap != null)
                {
                    try
                    {
                        CurrentState.FillColor = StandardColors.White;
                        CurrentState.SetFillPaintFilterBitmap(true);

                        _shader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);

                        //_shaderMatrix.Reset ();
                        //_shaderMatrix.PreScale (CurrentState.ScaleX, CurrentState.ScaleY);
                        //_shader.SetLocalMatrix (shaderMatrix);

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
                var image = paint.Image as SkiaImage;
                if (image != null)
                {
                    SKBitmap bitmap = image.NativeImage;

                    if (bitmap != null)
                    {
                        try
                        {
                            CurrentState.FillColor = StandardColors.White;
                            CurrentState.SetFillPaintFilterBitmap(true);

                            _shader = SKShader.CreateBitmap(bitmap, SKShaderTileMode.Repeat, SKShaderTileMode.Repeat);
                            //_shaderMatrix.Reset ();
                            //_shaderMatrix.PreScale (CurrentState.ScaleX, CurrentState.ScaleY);
                            //_shader.SetLocalMatrix (shaderMatrix);

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
                        FillColor = StandardColors.White;
                    }
                }
                else
                {
                    FillColor = StandardColors.White;
                }
            }
            else
            {
                FillColor = paint.StartColor;
            }
        }

        protected override void NativeDrawLine(
            float x1,
            float y1,
            float x2,
            float y2)
        {
            _canvas.DrawLine(x1, y1, x2, y2, CurrentState.StrokePaintWithAlpha);
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
                startAngle += 360;

            while (endAngle < 0)
                endAngle += 360;

            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            var sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);
            var strokeLocation = CurrentState.StrokeLocation;

            var rect = new SKRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight);

            startAngle *= -1;
            if (!clockwise)
                sweep *= -1;

            if (closed)
            {
                var nativePath = new SKPath();
                nativePath.AddArc(rect, startAngle, sweep);
                nativePath.Close();
                _canvas.DrawPath(nativePath, CurrentState.StrokePaintWithAlpha);
                nativePath.Dispose();
            }
            else
            {
                // todo: delete this after the api is bound
                var nativePath = new SKPath();
                nativePath.AddArc(rect, startAngle, sweep);
                _canvas.DrawPath(nativePath, CurrentState.StrokePaintWithAlpha);
                nativePath.Dispose();

                // todo: restore this when the api is bound.
                //_canvas.DrawArc (rect, startAngle, sweep, false, CurrentState.StrokePaintWithAlpha);
            }
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
                startAngle += 360;

            while (endAngle < 0)
                endAngle += 360;

            var sweep = Geometry.GetSweep(startAngle, endAngle, clockwise);
            var rect = new SKRect(x, y, x + width, y + height);

            startAngle *= -1;
            if (!clockwise)
                sweep *= -1;

            // todo: delete this after the api is bound
            var nativePath = new SKPath();
            nativePath.AddArc(rect, startAngle, sweep);
            _canvas.DrawPath(nativePath, CurrentState.FillPaintWithAlpha);
            nativePath.Dispose();

            // todo: restore this when the api is bound.
            //_canvas.DrawArc (rect, startAngle, sweep, false, CurrentState.FillPaintWithAlpha);
        }

        protected override void NativeDrawRectangle(
            float x,
            float y,
            float width,
            float height)
        {
            float rectX = 0, rectY = 0, rectWidth = 0, rectHeight = 0;

            var strokeSize = CurrentState.ScaledStrokeSize;
            if (strokeSize <= 0)
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

            _canvas.DrawRect(rectX, rectY, rectWidth, rectHeight, CurrentState.StrokePaintWithAlpha);
        }

        public override void FillRectangle(
            float x,
            float y,
            float width,
            float height)
        {
            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            _canvas.DrawRect(rectX, rectY, rectWidth, rectHeight, CurrentState.FillPaintWithAlpha);
        }

        protected override void NativeDrawRoundedRectangle(
            float x,
            float y,
            float width,
            float height,
            float aCornerRadius)
        {
            // These values work for a stroke location of center.
            var strokeSize = CurrentState.ScaledStrokeSize;

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

            _canvas.DrawRoundRect(rectX, rectY, rectWidth, rectHeight, radius, radius, CurrentState.StrokePaintWithAlpha);
        }

        public override void FillRoundedRectangle(
            float x,
            float y,
            float width,
            float height,
            float aCornerRadius)
        {
            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;
            var radius = aCornerRadius;

            var rect = new SKRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawRoundRect(rect, radius, radius, CurrentState.FillPaintWithAlpha);
        }

        protected override void NativeDrawOval(
            float x,
            float y,
            float width,
            float height)
        {
            // These values work for a stroke location of center.
            var strokeSize = CurrentState.ScaledStrokeSize;

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

            var rect = new SKRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawOval(rect, CurrentState.StrokePaintWithAlpha);
        }

        public override void FillOval(
            float x,
            float y,
            float width,
            float height)
        {
            // These values work for a stroke location of center.
            var rectX = x;
            var rectY = y;
            var rectWidth = width;
            var rectHeight = height;

            var rect = new SKRect(rectX, rectY, rectX + rectWidth, rectY + rectHeight);
            _canvas.DrawOval(rect, CurrentState.FillPaintWithAlpha);
        }

        public override void SubtractFromClip(
            float x,
            float y,
            float width,
            float height)
        {
            var rect = new SKRect(x, y, x + width, y + height);
            _canvas.ClipRect(rect, SKClipOperation.Difference);
        }

        protected override void NativeDrawPath(
            EWPath path)
        {
            var strokeLocation = CurrentState.StrokeLocation;
            if (strokeLocation == EWStrokeLocation.CENTER)
            {
                var nativePath = path.AsSkiaPath();
                _canvas.DrawPath(nativePath, CurrentState.StrokePaintWithAlpha);
                nativePath.Dispose();
            }
            else if (strokeLocation == EWStrokeLocation.INSIDE)
            {
                _canvas.Save();
                var nativePath = path.AsSkiaPath();
                _canvas.ClipPath(nativePath);
                var paint = CurrentState.StrokePaintWithAlpha;
                var strokeSize = paint.StrokeWidth;
                paint.StrokeWidth = strokeSize * 2;
                _canvas.DrawPath(nativePath, paint);
                paint.StrokeWidth = strokeSize;
                _canvas.Restore();
                nativePath.Dispose();
            }
            else if (strokeLocation == EWStrokeLocation.OUTSIDE)
            {
                var origClip = _canvas.LocalClipBounds;
                var nativePath = path.AsSkiaPath();

                var clippingPath = new SKPath();
                var origClipAsRect = origClip;
                clippingPath.AddRect(origClipAsRect, SKPathDirection.Clockwise);
                clippingPath.AddPath(nativePath);

                _canvas.Save();
                _canvas.ClipPath(clippingPath);

                var paint = CurrentState.StrokePaintWithAlpha;
                var vStrokeSize = paint.StrokeWidth;
                paint.StrokeWidth = vStrokeSize * 2;
                _canvas.DrawPath(nativePath, paint);
                paint.StrokeWidth = vStrokeSize;
                _canvas.Restore();

                //origClipAsRect.Dispose ();
                nativePath.Dispose();
                clippingPath.Dispose();
            }
        }

        public override void ClipPath(EWPath path,
            EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            var nativePath = path.AsSkiaPath();
            nativePath.FillType = windingMode == EWWindingMode.NonZero ? SKPathFillType.Winding : SKPathFillType.EvenOdd;
            _canvas.ClipPath(nativePath);
        }

        public override void FillPath(EWPath path,
            EWWindingMode windingMode)
        {
            var nativePath = path.AsSkiaPath();
            nativePath.FillType = windingMode == EWWindingMode.NonZero ? SKPathFillType.Winding : SKPathFillType.EvenOdd;
            _canvas.DrawPath(nativePath, CurrentState.FillPaintWithAlpha);
            nativePath.Dispose();
        }

        public override void DrawString(
            string value,
            float x,
            float y,
            EwHorizontalAlignment horizAlignment)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (horizAlignment == EwHorizontalAlignment.Left)
            {
                _canvas.DrawText(value, x, y, CurrentState.FontPaint);
            }
            else if (horizAlignment == EwHorizontalAlignment.Right)
            {
                var paint = CurrentState.FontPaint;
                var width = paint.MeasureText(value);
                x -= width;
                _canvas.DrawText(value, x, y, CurrentState.FontPaint);
            }
            else
            {
                var paint = CurrentState.FontPaint;
                var width = paint.MeasureText(value);
                x -= width / 2;
                _canvas.DrawText(value, x, y, CurrentState.FontPaint);
            }
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
            if (string.IsNullOrEmpty(value))
                return;

            var rect = new EWRectangle(x, y, width, height);

            var attributes = new StandardTextAttributes()
            {
                FontSize = CurrentState.FontPaint.TextSize,
                FontName = CurrentState.FontName,
                HorizontalAlignment = horizAlignment,
                VerticalAlignment = vertAlignment,
            };

            LayoutLine callback = (
                point,
                textual,
                text,
                ascent,
                descent,
                leading) =>
            {
                _canvas.DrawText(text, point.X, point.Y, CurrentState.FontPaint);
            };

            using (var textLayout = new SkiaTextLayout(value, rect, attributes, callback, textFlow, CurrentState.FontPaint))
            {
                textLayout.LayoutText();
            }
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            Logger.Debug("Not yet implemented.");
            DrawString(value?.Text, x, y, width, height, EwHorizontalAlignment.Left, EwVerticalAlignment.Top);
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

        protected override void StateRestored(SkiaCanvasState state)
        {
            _canvas?.Restore();
        }

        public override void SetShadow(
            EWSize offset,
            float blur,
            EWColor color,
            float zoom)
        {
            var actualOffset = offset;
            if (actualOffset == null)
                actualOffset = DefaultShadowOffset;

            var sx = actualOffset.Width * zoom;
            var sy = actualOffset.Height * zoom;

            var finalBlur = blur * zoom;

            if (color == null)
            {
                var actualColor = StandardColors.Black.AsSKColorMultiplyAlpha(CurrentState.Alpha);
                CurrentState.SetShadow(finalBlur, sx, sy, actualColor);
            }
            else
            {
                var actualColor = color.AsSKColorMultiplyAlpha(CurrentState.Alpha);
                CurrentState.SetShadow(finalBlur, sx, sy, actualColor);
            }
        }

        protected override void NativeRotate(
            float degrees,
            float radians,
            float x,
            float y)
        {
            _canvas.RotateDegrees(degrees, x, y);
        }

        protected override void NativeRotate(
            float degrees,
            float radians)
        {
            _canvas.RotateDegrees(degrees);
        }

        protected override void NativeScale(
            float xFactor,
            float yFactor)
        {
            //canvas.Scale((float)aXFactor, (float)aYFactor);
            CurrentState.SetScale(Math.Abs(xFactor), Math.Abs(yFactor));
            if (xFactor < 0 || yFactor < 0)
                _canvas.Scale(xFactor < 0 ? -1 : 1, yFactor < 0 ? -1 : 1);
        }

        protected override void NativeTranslate(
            float tx,
            float ty)
        {
            _canvas.Translate(tx * CurrentState.ScaleX, ty * CurrentState.ScaleY);
        }

        protected override void NativeConcatenateTransform(EWAffineTransform transform)
        {
            var matrix = new SKMatrix();

            var values = new float[6];
            _canvas.TotalMatrix.GetValues(values);
            matrix.Values = values;

            // todo: implement me
            //matrix.PostConcat (transform.AsMatrix ());
            //canvas.Matrix = matrix;
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

        public override void DrawImage(
            EWImage image,
            float x,
            float y,
            float width,
            float height)
        {
            var skiaImage = image as SkiaImage;
            var bitmap = skiaImage?.NativeImage;
            if (bitmap != null)
            {
                var scaleX = CurrentState.ScaleX < 0 ? -1 : 1;
                var scaleY = CurrentState.ScaleY < 0 ? -1 : 1;

                _canvas.Save();
                //canvas.Scale (scaleX, scaleY);
                var srcRect = new SKRect(0, 0, bitmap.Width, bitmap.Height);

                x *= scaleX;
                y *= scaleY;
                width *= scaleX;
                height *= scaleY;

                var rx1 = Math.Min(x, x + width);
                var ry1 = Math.Min(y, y + height);
                var rx2 = Math.Max(x, x + width);
                var ry2 = Math.Max(y, y + height);

                var destRect = new SKRect(rx1, ry1, rx2, ry2);
                var paint = CurrentState.GetImagePaint(1, 1);
                _canvas.DrawBitmap(bitmap, srcRect, destRect, paint);
                paint?.Dispose();
                _canvas.Restore();
            }
        }

        public override void ClipRectangle(
            float x,
            float y,
            float width,
            float height)
        {
            _canvas.ClipRect(new SKRect(x, y, x + width, y + height));
        }
    }
}