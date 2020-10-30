using Android.Graphics;
using Android.Text;
using Color = Android.Graphics.Color;

namespace System.Graphics.Android
{
    public class MDCanvasState : CanvasState, BlurrableCanvas
    {
        public float Alpha = 1;
        private Paint _fillPaint;
        private Paint _strokePaint;
        private string _fontName = "Arial";
        private TextPaint _fontPaint;
        private float _fontSize = 10f;
        private float _scaleX = 1;
        private float _scaleY = 1;
        private bool _typefaceInvalid;
        private bool _isBlurred;
        private float _blurRadius;
        private BlurMaskFilter _blurFilter;
        private bool _shadowed;
        private Color _shadowColor;
        private float _shadowX;
        private float _shadowY;
        private float _shadowBlur;

        private EWColor _strokeColor = StandardColors.Black;
        private EWColor _fillColor = StandardColors.White;
        private EWColor _fontColor = StandardColors.Black;

        public MDCanvasState()
        {
        }

        public MDCanvasState(MDCanvasState prototype) : base(prototype)
        {
            _strokeColor = prototype._strokeColor;
            _fillColor = prototype._fillColor;
            _fontColor = prototype._fontColor;

            _fontPaint = new TextPaint(prototype.FontPaint);
            _fillPaint = new Paint(prototype.FillPaint);
            _strokePaint = new Paint(prototype.StrokePaint);
            _fontName = prototype._fontName;
            _fontSize = prototype._fontSize;
            Alpha = prototype.Alpha;
            _scaleX = prototype._scaleX;
            _scaleY = prototype._scaleY;
            _typefaceInvalid = false;

            _isBlurred = prototype._isBlurred;
            _blurRadius = prototype._blurRadius;

            _shadowed = prototype._shadowed;
            _shadowColor = prototype._shadowColor;
            _shadowX = prototype._shadowX;
            _shadowY = prototype._shadowY;
            _shadowBlur = prototype._shadowBlur;
        }

        public EWColor StrokeColor
        {
            get => _strokeColor;
            set => _strokeColor = value;
        }

        public EWColor FillColor
        {
            get => _fillColor;
            set => _fillColor = value;
        }

        public EWColor FontColor
        {
            get => _fontColor;
            set
            {
                _fontColor = value;
                FontPaint.Color = value != null ? _fontColor.AsColor() : Color.Black;
            }
        }

        public EWLineCap StrokeLineCap
        {
            set
            {
                if (value == EWLineCap.BUTT)
                    StrokePaint.StrokeCap = Paint.Cap.Butt;
                else if (value == EWLineCap.ROUND)
                    StrokePaint.StrokeCap = Paint.Cap.Round;
                else if (value == EWLineCap.SQUARE)
                    StrokePaint.StrokeCap = Paint.Cap.Square;
            }
        }

        public EWLineJoin StrokeLineJoin
        {
            set
            {
                if (value == EWLineJoin.MITER)
                    StrokePaint.StrokeJoin = Paint.Join.Miter;
                else if (value == EWLineJoin.ROUND)
                    StrokePaint.StrokeJoin = Paint.Join.Round;
                else if (value == EWLineJoin.BEVEL)
                    StrokePaint.StrokeJoin = Paint.Join.Bevel;
            }
        }

        public float MiterLimit
        {
            set => StrokePaint.StrokeMiter = value;
        }

        public void SetStrokeDashPattern(float[] pattern, float strokeSize)
        {
            if (pattern == null || pattern.Length == 0 || strokeSize == 0)
            {
                StrokePaint.SetPathEffect(null);
            }
            else
            {
                float scaledStrokeSize = strokeSize * ScaleX;

                if (scaledStrokeSize == 1)
                {
                    StrokePaint.SetPathEffect(new DashPathEffect(pattern, 0));
                }
                else
                {
                    var scaledPattern = new float[pattern.Length];
                    for (int i = 0; i < pattern.Length; i++)
                        scaledPattern[i] = pattern[i] * scaledStrokeSize;
                    StrokePaint.SetPathEffect(new DashPathEffect(scaledPattern, 0));
                }
            }
        }

        public bool AntiAlias
        {
            set => StrokePaint.AntiAlias = value;
        }

        public bool IsBlurred => _isBlurred;

        public float BlurRadius => _blurRadius;

        public void SetBlur(float aRadius)
        {
            if (aRadius != _blurRadius)
            {
                if (_blurFilter != null)
                {
                    _blurFilter.Dispose();
                    _blurFilter = null;
                }

                if (aRadius > 0)
                {
                    _isBlurred = true;
                    _blurRadius = aRadius;

                    _blurFilter = new BlurMaskFilter(_blurRadius, BlurMaskFilter.Blur.Normal);

                    _fillPaint?.SetMaskFilter(_blurFilter);
                    _strokePaint?.SetMaskFilter(_blurFilter);
                    _fontPaint?.SetMaskFilter(_blurFilter);
                }
                else
                {
                    _isBlurred = false;
                    _blurRadius = 0;

                    _fillPaint?.SetMaskFilter(null);
                    _strokePaint?.SetMaskFilter(null);
                    _fontPaint?.SetMaskFilter(null);
                }
            }
        }

        public float NativeStrokeSize
        {
            set => StrokePaint.StrokeWidth = value * _scaleX;
        }

        public float FontSize
        {
            set
            {
                _fontSize = value;
                FontPaint.TextSize = _fontSize * _scaleX;
            }
        }

        public string FontName
        {
            set
            {
                if (_fontName != value && (_fontName != null && !_fontName.Equals(value)))
                {
                    _fontName = value;
                    _typefaceInvalid = true;
                }
            }

            get => _fontName;
        }

        public TextPaint FontPaint
        {
            get
            {
                if (_fontPaint == null)
                {
                    _fontPaint = new TextPaint();
                    _fontPaint.SetARGB(1, 0, 0, 0);
                    _fontPaint.AntiAlias = true;
                    _fontPaint.SetTypeface(MDFontService.Instance.GetTypeface("Arial"));
                }

                if (_typefaceInvalid)
                {
                    _fontPaint.SetTypeface(MDFontService.Instance.GetTypeface(_fontName));
                    _typefaceInvalid = false;
                }

                return _fontPaint;
            }

            set => _fontPaint = value;
        }

        public Paint FillPaint
        {
            private get
            {
                if (_fillPaint == null)
                {
                    _fillPaint = new Paint();
                    _fillPaint.SetARGB(1, 1, 1, 1);
                    _fillPaint.SetStyle(Paint.Style.Fill);
                    _fillPaint.AntiAlias = true;
                }

                return _fillPaint;
            }

            set { _fillPaint = value; }
        }

        public Paint StrokePaint
        {
            private get
            {
                if (_strokePaint == null)
                {
                    var paint = new Paint();
                    paint.SetARGB(1, 0, 0, 0);
                    paint.StrokeWidth = 1;
                    paint.StrokeMiter = EWCanvas.DefaultMiterLimit;
                    paint.SetStyle(Paint.Style.Stroke);
                    paint.AntiAlias = true;

                    _strokePaint = paint;

                    return paint;
                }

                return _strokePaint;
            }

            set { _strokePaint = value; }
        }

        public Paint StrokePaintWithAlpha
        {
            get
            {
                var paint = StrokePaint;

                var r = (int) (_strokeColor.Red * 255f);
                var g = (int) (_strokeColor.Green * 255f);
                var b = (int) (_strokeColor.Blue * 255f);
                var a = (int) (_strokeColor.Alpha * 255f * Alpha);

                paint.SetARGB(a, r, g, b);
                return paint;
            }
        }

        public Paint FillPaintWithAlpha
        {
            get
            {
                var paint = FillPaint;

                var r = (int) (_fillColor.Red * 255f);
                var g = (int) (_fillColor.Green * 255f);
                var b = (int) (_fillColor.Blue * 255f);
                var a = (int) (_fillColor.Alpha * 255f * Alpha);

                paint.SetARGB(a, r, g, b);
                return paint;
            }
        }

        public void SetFillPaintShader(Shader shader)
        {
            FillPaint.SetShader(shader);
        }

        public void SetFillPaintFilterBitmap(bool value)
        {
            FillPaint.FilterBitmap = value;
        }

        public float ScaledStrokeSize => StrokeSize * _scaleX;

        public float ScaledFontSize => _fontSize * _scaleX;

        public float ScaleX => _scaleX;

        public float ScaleY => _scaleY;

        #region IDisposable Members

        public override void Dispose()
        {
            if (_fontPaint != null)
            {
                _fontPaint.Dispose();
                _fontPaint = null;
            }

            if (_strokePaint != null)
            {
                _strokePaint.Dispose();
                _strokePaint = null;
            }

            if (_fillPaint != null)
            {
                _fillPaint.Dispose();
                _fillPaint = null;
            }

            base.Dispose();
        }

        #endregion

        public void SetShadow(float blur, float sx, float sy, Color color)
        {
            FillPaint.SetShadowLayer(blur, sx, sy, color);
            StrokePaint.SetShadowLayer(blur, sx, sy, color);
            FontPaint.SetShadowLayer(blur, sx, sy, color);

            _shadowed = true;
            _shadowBlur = blur;
            _shadowX = sx;
            _shadowY = sy;
            _shadowColor = color;
        }

        public Paint GetShadowPaint(float sx, float sy)
        {
            if (_shadowed)
            {
                var shadowPaint = new Paint();
                shadowPaint.SetARGB(255, 0, 0, 0);
                shadowPaint.SetStyle(Paint.Style.Fill);
                shadowPaint.AntiAlias = true;
                shadowPaint.SetShadowLayer(_shadowBlur, _shadowX * sx, _shadowY * sy, _shadowColor);
                shadowPaint.Alpha = (int) (Alpha * 255f);
                return shadowPaint;
            }

            return null;
        }

        public Paint GetImagePaint(float sx, float sy)
        {
            var imagePaint = GetShadowPaint(sx, sy);
            if (imagePaint == null && Alpha < 1)
            {
                imagePaint = new Paint();
                imagePaint.SetARGB(255, 0, 0, 0);
                imagePaint.SetStyle(Paint.Style.Fill);
                imagePaint.AntiAlias = true;
                imagePaint.Alpha = (int) (Alpha * 255f);
            }

            return imagePaint;
        }

        public void SetScale(float sx, float sy)
        {
            _scaleX = _scaleX * sx;
            _scaleY = _scaleY * sy;

            StrokePaint.StrokeWidth = StrokeSize * _scaleX;
            FontPaint.TextSize = _fontSize * _scaleX;
        }

        public void Reset(Paint aFontPaint, Paint aFillPaint, Paint aStrokePaint)
        {
            _fontPaint?.Dispose();
            _fontPaint = new TextPaint(aFontPaint);

            _fillPaint?.Dispose();
            _fillPaint = new Paint(aFillPaint);

            _strokePaint?.Dispose();
            _strokePaint = new Paint(aStrokePaint);

            _fontName = "Arial";
            _fontSize = 10f;
            Alpha = 1;
            _scaleX = 1;
            _scaleY = 1;
        }
    }
}