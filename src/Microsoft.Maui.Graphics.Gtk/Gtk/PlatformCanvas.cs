using System.Numerics;

namespace Microsoft.Maui.Graphics.Platform.Gtk
{

	public partial class PlatformCanvas : AbstractCanvas<PlatformCanvasState>
	{

		public PlatformCanvas() : base(CreateNewState, CreateStateCopy)
		{
			_context = default;
		}

		private Cairo.Context _context;

		public Cairo.Context Context
		{
			get => _context;
			set
			{
				_context = default;
				ResetState();
				_context = value;
			}
		}

		private static PlatformCanvasState CreateNewState(object context)
		{
			return new PlatformCanvasState { };
		}

		private static PlatformCanvasState CreateStateCopy(PlatformCanvasState prototype)
		{
			return new PlatformCanvasState(prototype);
		}

		public override void SaveState()
		{
			Context?.Save();
			base.SaveState();
		}

		public override bool RestoreState()
		{
			Context?.Restore();

			return base.RestoreState();
		}

		public override bool Antialias
		{
			set => CurrentState.Antialias = CanvasExtensions.ToAntialias(value);
		}

		public override float MiterLimit
		{
			set => CurrentState.MiterLimit = value;
		}

		public override Color StrokeColor
		{
			set => CurrentState.StrokeColor = value.ToCairoColor();
		}

		public override LineCap StrokeLineCap
		{
			set => CurrentState.LineCap = value.ToLineCap();
		}

		public override LineJoin StrokeLineJoin
		{
			set => CurrentState.LineJoin = value.ToLineJoin();
		}

		protected override float PlatformStrokeSize
		{
			set => CurrentState.StrokeSize = value;
		}

		public override Color FillColor
		{
			set => CurrentState.FillColor = value.ToCairoColor();
		}

		public override Color FontColor
		{
			set => CurrentState.FontColor = value.ToCairoColor();
		}

		public override IFont Font
		{
			set => CurrentState.Font = value;
		}

		public override float FontSize
		{
			set => CurrentState.FontSize = value;
		}

		public override float Alpha
		{
			set => CurrentState.Alpha = value;
		}

		public override BlendMode BlendMode
		{
			set => CurrentState.BlendMode = value;
		}

		protected override void PlatformSetStrokeDashPattern(float[] strokePattern, float strokeDashOffset, float strokeSize)
		{
			CurrentState.StrokeDashPattern = strokePattern;
		}

		private void Draw(bool preserve = false)
		{
			Context.SetSourceRGBA(CurrentState.StrokeColor.R, CurrentState.StrokeColor.G, CurrentState.StrokeColor.B, CurrentState.StrokeColor.A * CurrentState.Alpha);
			Context.LineWidth = CurrentState.StrokeSize;
			Context.MiterLimit = CurrentState.MiterLimit;
			Context.LineCap = CurrentState.LineCap;
			Context.LineJoin = CurrentState.LineJoin;

			Context.SetDash(CurrentState.NativeDash, 0);
			DrawShadow(false);

			if (preserve)
				Context.StrokePreserve();
			else
			{
				Context.Stroke();
			}
		}

		public void Fill(bool preserve = false)
		{

			Context.SetSourceRGBA(CurrentState.FillColor.R, CurrentState.FillColor.G, CurrentState.FillColor.B, CurrentState.FillColor.A * CurrentState.Alpha);

			DrawShadow(true);

			DrawFillPaint(Context, CurrentState.FillPaint.paint, CurrentState.FillPaint.rectangle);

			if (preserve)
			{
				Context.FillPreserve();
			}
			else
			{
				Context.Fill();
				CurrentState.FillPaint = default;
			}

		}

		protected override void PlatformDrawLine(float x1, float y1, float x2, float y2)
		{
			AddLine(Context, x1, y1, x2, y2);
			Draw();
		}

		protected override void PlatformDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
		{
			AddArc(Context, x, y, width, height, startAngle, endAngle, clockwise, closed);
			Draw();

		}

		protected override void PlatformDrawRectangle(float x, float y, float width, float height)
		{
			AddRectangle(Context, x, y, width, height);
			Draw();
		}

		protected override void PlatformDrawRoundedRectangle(float x, float y, float width, float height, float radius)
		{
			AddRoundedRectangle(Context, x, y, width, height, radius);
			Draw();
		}

		protected override void PlatformDrawEllipse(float x, float y, float width, float height)
		{
			AddEllipse(Context, x, y, width, height);
			Draw();
		}

		protected override void PlatformDrawPath(PathF path)
		{
			AddPath(Context, path);
			Draw();
		}

		protected override void PlatformRotate(float degrees, float radians, float x, float y)
		{
			Context.Translate(x, y);
			Context.Rotate(radians);
			Context.Translate(-x, -y);
		}

		protected override void PlatformRotate(float degrees, float radians)
		{
			Context.Rotate(radians);
		}

		protected override void PlatformScale(float fx, float fy)
		{
			Context.Scale(fx, fy);
		}

		protected override void PlatformTranslate(float tx, float ty)
		{
			Context.Translate(tx, ty);
		}

		[GtkMissingImplementation]
		protected override void PlatformConcatenateTransform(Matrix3x2 transform) { }

		public override void SetShadow(SizeF offset, float blur, Color color)
		{
			CurrentState.Shadow = (offset, blur, color);
		}

		public override void SetFillPaint(Paint paint, RectF rectangle)
		{
			CurrentState.FillPaint = (paint, rectangle);
		}

		public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
		{
			AddArc(Context, x, y, width, height, startAngle, endAngle, clockwise, true);
			Fill();
		}

		public override void FillRectangle(float x, float y, float width, float height)
		{
			AddRectangle(Context, x, y, width, height);
			Fill();
		}

		public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
		{
			AddRoundedRectangle(Context, x, y, width, height, cornerRadius);
			Fill();
		}

		public override void FillEllipse(float x, float y, float width, float height)
		{
			AddEllipse(Context, x, y, width, height);
			Fill();
		}

		public override void FillPath(PathF path, WindingMode windingMode)
		{
			Context.Save();
			Context.FillRule = windingMode.ToFillRule();
			AddPath(Context, path);
			Fill();
			Context.Restore();
		}

		public override void DrawImage(IImage image, float x, float y, float width, float height)
		{
			if (image is GtkImage { NativeImage: { } pixbuf })
			{
				DrawPixbuf(Context, pixbuf, x, y, width, height);
			}
		}

		[GtkMissingImplementation]
		public override void SubtractFromClip(float x, float y, float width, float height) { }

		[GtkMissingImplementation]
		public override void ClipPath(PathF path, WindingMode windingMode = WindingMode.NonZero) { }

		[GtkMissingImplementation]
		public override void ClipRectangle(float x, float y, float width, float height) { }

	}

}
