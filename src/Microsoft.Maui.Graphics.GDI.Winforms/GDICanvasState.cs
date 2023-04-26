using System.Drawing;
using System.Drawing.Drawing2D;
using System.Numerics;

using Drawing = System.Drawing;

namespace Microsoft.Maui.Graphics.GDI
{
	public class GDICanvasState : CanvasState
	{
		internal const float DpiAdjustment = 72f / 96f;

		private readonly System.Drawing.Graphics _graphics;

		private float _strokeWidth;
		private float _scale;

		private Pen _strokePen;
		private GraphicsState _state;

		private SolidBrush _fillBrushSolid;
		private LinearGradientBrush _fillBrushLinear;
		private PathGradientBrush _fillBrushRadial;
		private Brush _activeBrush;

		private SolidBrush _textBrush;
		private IFont _font;
		private float _fontSize;

		private Matrix _originalTransform;

		public GDICanvasState(System.Drawing.Graphics graphics)
		{
			_graphics = graphics;
			SetToDefaults();
		}

		public GDICanvasState(GDICanvasState prototype) : base(prototype)
		{
			_graphics = prototype._graphics;

			_strokeWidth = prototype._strokeWidth;
			_scale = prototype._scale;
			StrokeColor = prototype.StrokeColor;
			StrokeDashPattern = prototype.StrokeDashPattern;
			StrokeLineJoin = prototype.StrokeLineJoin;
			StrokeLineCap = prototype.StrokeLineCap;
			StrokeMiterLimit = prototype.StrokeMiterLimit;

			FillColor = prototype.FillColor;

			TextColor = prototype.TextColor;
			_font = prototype._font;
			_fontSize = prototype._fontSize;

			/*strokeBrush = prototype.strokeBrush;
			fillBrush = prototype.fillBrush;
			fontBrush = prototype.fontBrush;
			shadowColor = prototype.shadowColor;

			sourceStrokeColor = prototype.sourceStrokeColor;
			sourceFillpaint = prototype.sourceFillpaint;
			sourceFillColor = prototype.sourceFillColor;
			sourceFontColor = prototype.sourceFontColor;
			sourceShadowColor = prototype.sourceShadowColor;

			strokeBrushValid = prototype.strokeBrushValid;
			fillBrushValid = prototype.fillBrushValid;
			fontBrushValid = prototype.fontBrushValid;
			shadowColorValid = prototype.shadowColorValid;

			strokeStyle = prototype.strokeStyle;
			strokeStyleProperties = prototype.strokeStyleProperties;
			needsStrokeStyle = prototype.needsStrokeStyle;

			IsShadowed = prototype.IsShadowed;
			ShadowOffset = prototype.ShadowOffset;
			ShadowBlur = prototype.ShadowBlur;

			ActualScale = prototype.ActualScale;
			Matrix = prototype.Matrix;

			FontName = prototype.FontName;
			FontSize = prototype.FontSize;
			FontWeight = prototype.FontWeight;
			FontStyle = prototype.FontStyle;

			alpha = prototype.alpha;

			IsBlurred = prototype.IsBlurred;
			BlurRadius = prototype.BlurRadius;*/
		}

		public float ActualScale { get; set; }

		public float StrokeWidth
		{
			get => _strokeWidth * _scale;
			set => _strokeWidth = value;
		}

		public Pen StrokePen
		{
			get
			{
				if (_strokePen == null)
				{
					_strokePen = new Pen(StrokeColor, _strokeWidth);
				}
				else
				{
					_strokePen.Color = StrokeColor;
					_strokePen.Width = _strokeWidth;
				}

				if (StrokeDashPattern == null)
				{
					_strokePen.DashStyle = DashStyle.Solid;
				}
				else
				{
					_strokePen.DashStyle = DashStyle.Custom;
					_strokePen.DashPattern = StrokeDashPattern;
				}

				_strokePen.LineJoin = StrokeLineJoin;
				_strokePen.SetLineCap(StrokeLineCap, StrokeLineCap, DashCap.Flat);
				_strokePen.MiterLimit = StrokeMiterLimit;

				return _strokePen;
			}
		}

		public Drawing.Color StrokeColor { get; set; }

		private Drawing.Color _fillColor;
		public Drawing.Color FillColor
		{
			get => _fillColor;
			set
			{
				_fillColor = value;
				SetFillSolid(value);
			}
		}

		public void SetFillSolid(Drawing.Color color)
		{
			if (_fillBrushSolid is null)
				_fillBrushSolid = new SolidBrush(Drawing.Color.White);

			_fillBrushSolid.Color = color;

			_activeBrush = _fillBrushSolid;
		}

		public void SetFillLinear(Drawing.PointF point1, Drawing.PointF point2, Drawing.Color color1, Drawing.Color color2)
		{
			_fillBrushLinear?.Dispose();
			_fillBrushLinear = new LinearGradientBrush(point1, point2, color1, color2);

			_activeBrush = _fillBrushLinear;
		}

		public void SetFillRadial(GraphicsPath path, Drawing.Color center, Drawing.Color surround)
		{
			_fillBrushRadial?.Dispose();
			_fillBrushRadial = new PathGradientBrush(path)
			{
				CenterColor = center,
				SurroundColors = new Drawing.Color[] { surround }
			};

			_activeBrush = _fillBrushRadial;
		}

		public Brush FillBrush
		{
			get
			{
				if (_activeBrush is null)
					SetFillSolid(FillColor);

				return _activeBrush;
			}
		}

		public Drawing.Color TextColor { get; set; }


		public Brush TextBrush
		{
			get
			{
				if (_textBrush == null)
				{
					_textBrush = new SolidBrush(TextColor);
				}
				else
				{
					_textBrush.Color = TextColor;
				}

				return _textBrush;
			}
		}

		public bool IsBlurred { get; set; }

		public bool IsShadowed { get; set; }

		public IFont Font
		{
			get => _font;
			set
			{
				_font = value;
				if (_font != null)
				{
					//_font.Dispose();
					_font = null;
				}
			}
		}

		public FontStyle FontStyle
			=> (_font?.StyleType ?? Graphics.Font.Default.StyleType) switch
			{
				FontStyleType.Normal => FontStyle.Regular,
				FontStyleType.Italic => FontStyle.Italic,
				FontStyleType.Oblique => FontStyle.Italic,
				_ => FontStyle.Regular,
			};

		public float FontSize
		{
			set => _fontSize = value;
		}

		public float NativeFontSize
			=> _fontSize * DpiAdjustment;

		public Drawing.Drawing2D.LineJoin StrokeLineJoin { get; set; }

		public float StrokeMiterLimit { get; set; }

		public Drawing.Drawing2D.LineCap StrokeLineCap { get; set; }

		public override void Dispose()
		{
			if (_strokePen != null)
			{
				_strokePen.Dispose();
				_strokePen = null;
			}

			if (_fillBrushSolid != null)
			{
				_fillBrushSolid.Dispose();
				_fillBrushSolid = null;
			}

			if (_fillBrushLinear != null)
			{
				_fillBrushLinear.Dispose();
				_fillBrushLinear = null;
			}

			if (_fillBrushRadial != null)
			{
				_fillBrushRadial.Dispose();
				_fillBrushRadial = null;
			}

			if (_textBrush != null)
			{
				_textBrush.Dispose();
				_textBrush = null;
			}

			if (_font != null)
			{
				//_font.Dispose();
				_font = null;
			}

			base.Dispose();
		}

		public void SaveState()
		{
			var m = _graphics.Transform.Elements;
			_originalTransform = new Matrix(m[0], m[1], m[2], m[3], m[4], m[5]);

			_state = _graphics.Save();
		}

		public void RestoreState()
		{
			_graphics.Restore(_state);
			_graphics.Transform = _originalTransform;
			_originalTransform.Dispose();
			_state = null;
		}

		internal void SetToDefaults()
		{
			ActualScale = 1;

			_scale = 1;
			_strokeWidth = 1;
			StrokeColor = Drawing.Color.Black;
			StrokeDashPattern = null;
			StrokeLineJoin = Drawing.Drawing2D.LineJoin.Miter;
			StrokeLineCap = Drawing.Drawing2D.LineCap.Flat;
			StrokeMiterLimit = CanvasDefaults.DefaultMiterLimit;

			FillColor = Drawing.Color.White;

			TextColor = Drawing.Color.Black;
			_font = Graphics.Font.Default;
			_fontSize = 12 * DpiAdjustment;
			
			/* sourceStrokeColor = StandardColors.Black;
			strokeBrushValid = false;
			needsStrokeStyle = false;
			strokeStyle = null;

			Matrix = Matrix3x2.Identity;

			IsShadowed = false;
			sourceShadowColor = Canvas.DefaultShadowColor;

			alpha = 1;

			IsBlurred = false;
			BlurRadius = 0;*/
		}

		public void NativeTranslate(float tx, float ty)
		{
			_graphics.TranslateTransform(tx, ty);
		}

		public void NativeScale(float tx, float ty)
		{
			_scale *= tx;
			_graphics.ScaleTransform(tx, ty);
		}

		public void NativeRotate(float aAngle)
		{
			_graphics.RotateTransform(aAngle);
		}

		public void NativeRotate(float degrees, float x, float y)
		{
			_graphics.TranslateTransform(x, y);
			_graphics.RotateTransform(degrees);
			_graphics.TranslateTransform(-x, -y);
		}

		public void NativeConcatenateTransform(Matrix3x2 transform)
		{			
			_scale *= GetLengthScale(transform);
			var transformMatrix = new Matrix(transform.M11, transform.M12, transform.M21, transform.M22, transform.M31, transform.M32);
			_graphics.MultiplyTransform(transformMatrix);
		}
	}
}
