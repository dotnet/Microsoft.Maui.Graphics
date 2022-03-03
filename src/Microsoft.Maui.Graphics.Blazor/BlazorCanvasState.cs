using Microsoft.Maui.Graphics.Blazor.Canvas2D;

namespace Microsoft.Maui.Graphics.Blazor
{
	public class BlazorCanvasState : CanvasState
	{
		private static readonly float[] EmptyFloatArray = new float[] { };

		private bool _strokeDirty = true;
		private float _miterLimit = CanvasDefaults.DefaultMiterLimit;
		private Color _strokeColor = Colors.Black;
		private float _lineWidth = 1;
		private LineJoin _lineJoin;
		private LineCap _lineCap;
		private float _alpha = 1;
		private float[] _dashPattern;

		private bool _fillDirty = true;
		private Color _fillColor = Colors.White;
		private Paint _fillPaint = null;
		private RectF _fillRectangle;

		private bool _textDirty = true;
		private Color _textColor = Colors.Black;
		private IFont _font = Graphics.Font.Default;
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
			_fillRectangle = prototype._fillRectangle;
			_dashPattern = prototype._dashPattern;

			_textDirty = true;
			_fontSize = prototype._fontSize;
			_textColor = prototype._textColor;
			_font = prototype._font;
		}

		public Color StrokeColor
		{
			get => _strokeColor;
			set
			{
				if (_strokeColor != value)
				{
					_strokeColor = value ?? Colors.Black;
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

		public LineJoin LineJoin
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

		public LineCap LineCap
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

		public Color FillColor
		{
			get => _fillColor;
			set
			{
				if (_fillColor != value)
				{
					_fillPaint = null;
					_fillColor = value ?? Colors.White;
					_fillDirty = true;
				}
			}
		}


		public Color TextColor
		{
			get => _textColor;
			set
			{
				if (_textColor != value)
				{
					_textColor = value ?? Colors.Black;
					_textDirty = true;
				}
			}
		}

		public float Alpha
		{
			get => _alpha;
			set => _alpha = value;
		}

		public IFont Font
		{
			get => _font;
			set
			{
				if (!string.Equals(_font,value))
				{
					_font = value ?? Graphics.Font.Default;
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
					if (_fillPaint is LinearPaint linearPaint)
					{
						var gradient = context.CreateLinearGradient(_fillX1, _fillY1, _fillX2, _fillY2);
						for (int i = 0; i < linearPaint.GradientStops.Length; i++)
						{
							var color = linearPaint.GradientStops[i].Color;
							var offset = linearPaint.GradientStops[i].Offset;
							//var oouiColor = color != null ? color.AsOouiColorObject() : new Color(255, 255, 255, 255);
							var oouiColor = color != null ? color.AsOouiColor() : StandardColors.White.AsOouiColor();
							gradient.AddColorStop(offset, oouiColor);
						}
						context.FillStyle = gradient;
					}
					else if (_fillPaint is RadialGradientPaint radialGradientPaint)
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

		internal void SetFillPaint(Paint paint, RectF rectangle)
		{
			_fillColor = null;
			_fillPaint = paint;
			_fillRectangle = rectangle;
			_fillDirty = true;
		}
	}
}
