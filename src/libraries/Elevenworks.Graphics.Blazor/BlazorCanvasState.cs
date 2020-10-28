using Elevenworks.Graphics.Blazor.Canvas2D;

namespace Elevenworks.Graphics.Blazor
{
    public class BlazorCanvasState : CanvasState
    {
        private static readonly float[] EmptyFloatArray = new float[] { };

        private bool _strokeDirty = true;
        private float _miterLimit = EWCanvas.DefaultMiterLimit;
        private EWColor _strokeColor = StandardColors.Black;
        private float _lineWidth = 1;
        private EWLineJoin _lineJoin;
        private EWLineCap _lineCap;
        private float _alpha = 1;
        private float[] _dashPattern;

        private bool _fillDirty = true;
        private EWColor _fillColor = StandardColors.White;
        private EWPaint _fillPaint = null;
        private double _fillX1;
        private double _fillY1;
        private double _fillX2;
        private double _fillY2;

        private bool _textDirty = true;
        private EWColor _textColor = StandardColors.Black;
        private string _font = "Arial";
        private float _fontSize = 12f;

        public BlazorCanvasState()
        {

        }

        public BlazorCanvasState(BlazorCanvasState prototype) : base(prototype)
        {
            _strokeDirty = true;
            _strokeColor = prototype._strokeColor;
            _lineWidth = prototype._lineWidth;
            _lineJoin = prototype._lineJoin;
            _lineCap = prototype._lineCap;
            _miterLimit = prototype._miterLimit;

            _fillDirty = true;
            _fillColor = prototype._fillColor;
            _fillPaint = prototype._fillPaint;
            _fillX1 = prototype._fillX1;
            _fillY1 = prototype._fillY1;
            _fillX2 = prototype._fillX2;
            _fillY2 = prototype._fillY2;
            _dashPattern = prototype._dashPattern;

            _textDirty = true;
            _fontSize = prototype._fontSize;
            _textColor = prototype._textColor;
            _font = prototype._font;
        }

        public EWColor StrokeColor
        {
            get => _strokeColor;
            set
            {
                if (_strokeColor != value)
                {
                    _strokeColor = value ?? StandardColors.Black;
                    _strokeDirty = true;
                }
            }
        }

        public float MiterLimit
        {
            get => _miterLimit;
            set
            {
                if (_miterLimit != value)
                {
                    _miterLimit = value;
                    _strokeDirty = true;
                }
            }
        }

        public EWLineJoin LineJoin
        {
            get => _lineJoin;
            set
            {
                if (_lineJoin != value)
                {
                    _lineJoin = value;
                    _strokeDirty = true;
                }
            }
        }

        public EWLineCap LineCap
        {
            get => _lineCap;
            set
            {
                if (_lineCap != value)
                {
                    _lineCap = value;
                    _strokeDirty = true;
                }
            }
        }

        public float LineWidth
        {
            get => _lineWidth;
            set
            {
                if (_lineWidth != value)
                {
                    _lineWidth = value;
                    _strokeDirty = true;
                }
            }
        }

        public float[] BlazorDashPattern
        {
            get => _dashPattern;
            set
            {
                _dashPattern = value;
                _strokeDirty = true;
            }
        }

        public EWColor FillColor
        {
            get => _fillColor;
            set
            {
                if (_fillColor != value)
                {
                    _fillPaint = null;
                    _fillColor = value ?? StandardColors.White;
                    _fillDirty = true;
                }
            }
        }


        public EWColor TextColor
        {
            get => _textColor;
            set
            {
                if (_textColor != value)
                {
                    _textColor = value ?? StandardColors.Black;
                    _textDirty = true;
                }
            }
        }

        public float Alpha
        {
            get => _alpha;
            set => _alpha = value;
        }

        public string Font      
        {
            get => _font;
            set
            {
                if (!string.Equals(_font,value))
                {
                    _font = value ?? "Arial";
                    _textDirty = true;
                }
            }
        }
        
        public float FontSize
        {
            get => _fontSize;
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    _textDirty = true;
                }
            }
        }

        public float SetStrokeStyle(CanvasRenderingContext2D context)
        {
            context.GlobalAlpha = _strokeColor.Alpha * _alpha;

            if (_strokeDirty)
            {
                context.StrokeStyle = _strokeColor.AsCanvasValue();
                context.LineWidth = _lineWidth;
                context.LineCap = _lineCap.AsCanvasValue();
                context.LineJoin = _lineJoin.AsCanvasValue();
                context.SetLineDash(_dashPattern ?? EmptyFloatArray);
                _strokeDirty = false;
            }

            return _alpha;
        }

        public float SetFillStyle(CanvasRenderingContext2D context)
        {
            if (_fillDirty)
            {
                if (_fillColor != null)
                {
                    context.GlobalAlpha = _fillColor.Alpha * _alpha;
                    context.FillStyle = _fillColor.AsCanvasValue();
                }
                else if (_fillPaint != null)
                {
                    /*
                    if (_fillPaint.PaintType == EWPaintType.LINEAR_GRADIENT)
                    {
                        var gradient = context.CreateLinearGradient(_fillX1, _fillY1, _fillX2, _fillY2);
                        for (int i = 0; i < _fillPaint.Stops.Length; i++)
                        {
                            var color = _fillPaint.Stops[i].Color;
                            var offset = _fillPaint.Stops[i].Offset;
                            //var oouiColor = color != null ? color.AsOouiColorObject() : new Color(255, 255, 255, 255);
                            var oouiColor = color != null ? color.AsOouiColor() : StandardColors.White.AsOouiColor();
                            gradient.AddColorStop(offset, oouiColor);
                        }
                        context.FillStyle = gradient;
                    }
                    else if (_fillPaint.PaintType == EWPaintType.RADIAL_GRADIENT)
                    {

                    }*/
                }

                _fillDirty = false;
            }

            return _alpha;
        }

        public float SetTextStyle(CanvasRenderingContext2D context)
        {
            context.GlobalAlpha = _textColor.Alpha * _alpha;

            if (_textDirty)
            {
                context.Font = $"{_fontSize}px {_font}";
                context.FillStyle = _textColor.AsCanvasValue("black");
                _textDirty = false;
            }

            return _alpha;
        }

        internal void Restore()
        {
            _fillDirty = true;
            _strokeDirty = true;
            _textDirty = true;
        }

        internal void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            _fillColor = null;
            _fillPaint = paint;
            _fillX1 = x1;
            _fillY1 = y1;
            _fillX2 = x2;
            _fillY2 = y2;
            _fillDirty = true;
        }
    }
}
