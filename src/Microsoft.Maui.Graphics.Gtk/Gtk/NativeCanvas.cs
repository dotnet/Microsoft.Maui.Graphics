using System;
using Microsoft.Maui.Graphics.Text;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeCanvas : AbstractCanvas<NativeCanvasState> {

		public NativeCanvas() : base(CreateNewState, CreateStateCopy) { }

		private Cairo.Context _context;

		public Cairo.Context Context {
			get => _context;
			set {
				_context = null;
				ResetState();
				_context = value;
			}
		}

		private static NativeCanvasState CreateNewState(object context) {
			return new NativeCanvasState() { };
		}

		private static NativeCanvasState CreateStateCopy(NativeCanvasState prototype) {
			return new NativeCanvasState(prototype);
		}

		public override void SaveState() {
			Context?.Save();
			base.SaveState();
		}

		public override bool RestoreState() {
			Context?.Restore();

			return base.RestoreState();
		}

		public override bool Antialias {
			set => CurrentState.Antialias = CanvasExtensions.ToAntialias(value);
		}

		public override float MiterLimit {
			set => CurrentState.MiterLimit = value;
		}

		public override Color StrokeColor {
			set => CurrentState.StrokeColor = value.ToCairoColor();
		}

		public override LineCap StrokeLineCap {
			set => CurrentState.LineCap = value.ToLineCap();
		}

		public override LineJoin StrokeLineJoin {
			set => CurrentState.LineJoin = value.ToLineJoin();
		}

		protected override float NativeStrokeSize {
			set => CurrentState.StrokeSize = value;
		}

		public override Color FillColor {
			set => CurrentState.FillColor = value.ToCairoColor();
		}

		public override Color FontColor {
			set => CurrentState.FontColor = value.ToCairoColor();
		}

		public override string FontName {
			set => CurrentState.FontName = value;
		}

		public override float FontSize {
			set => CurrentState.FontSize = value;
		}

		public override float Alpha {
			set => CurrentState.Alpha = value;
		}

		public override BlendMode BlendMode {
			set => CurrentState.BlendMode = value;
		}

		protected override void NativeSetStrokeDashPattern(float[] pattern, float strokeSize) {
			CurrentState.StrokeDashPattern = pattern;
		}

		void AddLine(Cairo.Context context, float x1, float y1, float x2, float y2) {
			context.MoveTo(x1, y1);
			context.LineTo(x2, y2);
		}

		void Draw() {
			Context.SetSourceRGBA(CurrentState.StrokeColor.R, CurrentState.StrokeColor.G, CurrentState.StrokeColor.B, CurrentState.StrokeColor.A * CurrentState.Alpha);
			Context.LineWidth = CurrentState.StrokeSize;
			Context.MiterLimit = CurrentState.MiterLimit;
			Context.LineCap = CurrentState.LineCap;
			Context.LineJoin = CurrentState.LineJoin;

			Context.SetDash(CurrentState.NativeDash, 0);
			DrawShadow(false);
			Context.Stroke();
		}

		public Cairo.Context CreateContext() {
			var sf = new Cairo.ImageSurface(null, Cairo.Format.A1, 0, 0, 0);
			var context = new Cairo.Context(sf);

			return context;

		}

		public void DrawShadow(bool fill) {

			if (CurrentState.Shadow != default) {
				using var path = Context.CopyPath();
				Context.Save();
				var sfctx = Context.GetTarget();

				var extents = Context.PathExtents();
				var pathSize = new Size(extents.X + extents.Width, extents.Height + extents.Y);

				var s = sfctx.GetSize();

				var shadowSurface = s.HasValue ?
					sfctx.CreateSimilar(sfctx.Content, (int) pathSize.Width, (int) pathSize.Height) :
					new Cairo.ImageSurface(Cairo.Format.ARGB32, (int) pathSize.Width, (int) pathSize.Height);

				var shadowCtx = new Cairo.Context(shadowSurface);

				var shadow = CurrentState.Shadow;

				shadowCtx.AppendPath(path);

				if (fill)
					shadowCtx.ClosePath();

				var color = shadow.color.ToCairoColor();
				shadowCtx.SetSourceRGBA(color.R, color.G, color.B, color.A);
				shadowCtx.Clip();

				if (true)
					shadowCtx.PaintWithAlpha(0.3);
				else {
					shadowCtx.LineWidth = 10;
					shadowCtx.Stroke();
				}

				shadowCtx.PopGroupToSource();
				Context.SetSource(shadowSurface, shadow.offset.Width, shadow.offset.Height);
				Context.Paint();

				shadowCtx.Dispose();

				shadowSurface.Dispose();

				Context.Restore();
			}
		}

		public void Fill() {
			Context.SetSourceRGBA(CurrentState.FillColor.R, CurrentState.FillColor.G, CurrentState.FillColor.B, CurrentState.FillColor.A * CurrentState.Alpha);
			DrawShadow(true);
			Context.Fill();
		}

		protected override void NativeDrawLine(float x1, float y1, float x2, float y2) {
			AddLine(Context, x1, y1, x2, y2);
			Draw();
		}

		void AddArc(Cairo.Context context, float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed) {
			// https://developer.gnome.org/cairo/stable/cairo-Paths.html#cairo-arc
			// Angles are measured in radians

			AddArc(context, x, y, width, height, startAngle, endAngle, clockwise);

			if (closed)
				context.ClosePath();
		}

		protected override void NativeDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed) {
			AddArc(Context, x, y, width, height, startAngle, endAngle, clockwise, closed);
			Draw();

		}

		void AddRectangle(Cairo.Context context, float x, float y, float width, float height) {
			context.Rectangle(x, y, width, height);
		}

		protected override void NativeDrawRectangle(float x, float y, float width, float height) {
			AddRectangle(Context, x, y, width, height);
			Draw();
		}

		const double degrees = System.Math.PI / 180d;

		void AddRoundedRectangle(Cairo.Context context, float left, float top, float width, float height, float radius) {

			context.NewPath();
			// top left
			context.Arc(left + radius, top + radius, radius, 180 * degrees, 270 * degrees);
			// // top right
			context.Arc(left + width - radius, top + radius, radius, 270 * degrees, 0);
			// // bottom right
			context.Arc(left + width - radius, top + height - radius, radius, 0, 90 * degrees);
			// // bottom left
			context.Arc(left + radius, top + height - radius, radius, 90 * degrees, 180 * degrees);
			context.ClosePath();
		}

		protected override void NativeDrawRoundedRectangle(float x, float y, float width, float height, float radius) {
			AddRoundedRectangle(Context, x, y, width, height, radius);
			Draw();
		}

		public void AddEllipse(Cairo.Context context, float x, float y, float width, float height) {
			context.Save();
			context.NewPath();

			context.Translate(x + width / 2, y + height / 2);
			context.Scale(width / 2f, height / 2f);
			context.Arc(0, 0, 1, 0, 2 * Math.PI);
			context.Restore();

		}

		protected override void NativeDrawEllipse(float x, float y, float width, float height) {
			AddEllipse(Context, x, y, width, height);
			Draw();
		}

		protected override void NativeDrawPath(PathF path) {
			AddPath(Context, path);
			Draw();
		}

		private void AddArc(Cairo.Context context, float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise) {

			var startAngleInRadians = startAngle * -degrees;
			var endAngleInRadians = endAngle * -degrees;

			var cx = x + width / 2f;
			var cy = y + height / 2f;

			var r = 1;

			context.Save();

			context.Translate(cx, cy);
			context.Scale(width / 2f, height / 2f);

			if (clockwise)
				context.Arc(0, 0, r, startAngleInRadians, endAngleInRadians);
			else {
				context.ArcNegative(0, 0, r, startAngleInRadians, endAngleInRadians);
			}

			context.Restore();

		}

		private void AddPath(Cairo.Context context, PathF target) {
			var pointIndex = 0;
			var arcAngleIndex = 0;
			var arcClockwiseIndex = 0;

			foreach (var type in target.SegmentTypes) {
				if (type == PathOperation.Move) {
					var point = target[pointIndex++];
					context.MoveTo(point.X, point.Y);
				} else if (type == PathOperation.Line) {
					var endPoint = target[pointIndex++];
					context.LineTo(endPoint.X, endPoint.Y);

				} else if (type == PathOperation.Quad) {
					var p1 = pointIndex > 0 ? target[pointIndex - 1] : context.CurrentPoint.ToPointF();
					var c = target[pointIndex++];
					var p2 = target[pointIndex++];

					// quad bezier to cubic bezier:
					// C1 = 2/3•C + 1/3•P1
					// C2 = 2/3•C + 1/3•P2

					var c1 = new PointF(c.X * 2 / 3 + p1.X / 3, c.Y * 2 / 3 + p1.Y / 3);
					var c2 = new PointF(c.X * 2 / 3 + p2.X / 3, c.Y * 2 / 3 + p2.Y / 3);

					// Adds a cubic Bézier spline to the path
					context.CurveTo(
						c1.X, c1.Y,
						c2.X, c2.Y,
						p2.X, p2.Y);

				} else if (type == PathOperation.Cubic) {
					var controlPoint1 = target[pointIndex++];
					var controlPoint2 = target[pointIndex++];
					var endPoint = target[pointIndex++];

					// https://developer.gnome.org/cairo/stable/cairo-Paths.html#cairo-curve-to
					// Adds a cubic Bézier spline to the path from the current point to position (x3, y3) in user-space coordinates,
					// using (x1, y1) and (x2, y2) as the control points. After this call the current point will be (x3, y3).
					// If there is no current point before the call to cairo_curve_to() this function will behave as if preceded by a call to cairo_move_to(cr, x1, y1).
					context.CurveTo(
						controlPoint1.X, controlPoint1.Y,
						controlPoint2.X, controlPoint2.Y,
						endPoint.X, endPoint.Y);

				} else if (type == PathOperation.Arc) {
					var topLeft = target[pointIndex++];
					var bottomRight = target[pointIndex++];
					var startAngle = target.GetArcAngle(arcAngleIndex++);
					var endAngle = target.GetArcAngle(arcAngleIndex++);
					var clockwise = target.GetArcClockwise(arcClockwiseIndex++);

					AddArc(context, topLeft.X, topLeft.Y, bottomRight.X - topLeft.X, bottomRight.Y - topLeft.Y, startAngle, endAngle, clockwise);

				} else if (type == PathOperation.Close) {
					context.ClosePath();
				}
			}
		}

		protected override void NativeRotate(float degrees, float radians, float x, float y) {
			Context.Translate(x, y);
			Context.Rotate(radians);
			Context.Translate(-x, -y);
		}

		protected override void NativeRotate(float degrees, float radians) {
			Context.Rotate(radians);
		}

		protected override void NativeScale(float fx, float fy) {
			Context.Scale(fx, fy);
		}

		protected override void NativeTranslate(float tx, float ty) {
			Context.Translate(tx, ty);
		}

		protected override void NativeConcatenateTransform(AffineTransform transform) { }

		public override void SetShadow(SizeF offset, float blur, Color color) {
			CurrentState.Shadow = (offset, blur, color);
		}

		public override void SetFillPaint(Paint paint, RectangleF rectangle) {
			CurrentState.FillPaint = (paint, rectangle);
		}

		public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise) {
			AddArc(Context, x, y, width, height, startAngle, endAngle, clockwise, true);
			Fill();
		}

		public void AddFillPaint(Cairo.Context context, Paint paint, RectangleF rectangle) {
			if (paint == null)
				paint = Colors.White.AsPaint();

			if (paint is LinearGradientPaint linearGradientPaint) {
				var x1 = (float) (linearGradientPaint.StartPoint.X * rectangle.Width) + rectangle.X;
				var y1 = (float) (linearGradientPaint.StartPoint.Y * rectangle.Height) + rectangle.Y;

				var x2 = (float) (linearGradientPaint.EndPoint.X * rectangle.Width) + rectangle.X;
				var y2 = (float) (linearGradientPaint.EndPoint.Y * rectangle.Height) + rectangle.Y;

				var colors = Array.ConvertAll(linearGradientPaint.GetSortedStops(), s => s.Color.ToCairoColor());

				var stops = Array.ConvertAll(linearGradientPaint.GetSortedStops(), s => s.Offset);
				;

				try {
					;
				} catch (Exception exc) {
					Logger.Debug(exc);
					FillColor = linearGradientPaint.BlendStartAndEndColors();
				}
			} else if (paint is RadialGradientPaint radialGradientPaint) {

				var colors = Array.ConvertAll(radialGradientPaint.GetSortedStops(), s => s.Color.ToCairoColor());

				var stops = Array.ConvertAll(radialGradientPaint.GetSortedStops(), s => s.Offset);
				;

				var centerX = (float) (radialGradientPaint.Center.X * rectangle.Width) + rectangle.X;
				var centerY = (float) (radialGradientPaint.Center.Y * rectangle.Height) + rectangle.Y;
				var radius = (float) radialGradientPaint.Radius * Math.Max(rectangle.Height, rectangle.Width);

				if (radius == 0)
					radius = Geometry.GetDistance(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

				try { } catch (Exception exc) {
					Logger.Debug(exc);
					FillColor = radialGradientPaint.BlendStartAndEndColors();
				}
			} else if (paint is PatternPaint patternPaint) {
				var bitmap = patternPaint.GetPatternBitmap(DisplayScale);

				if (bitmap != null) {
					try { } catch (Exception exc) {
						Logger.Debug(exc);
						FillColor = paint.BackgroundColor;
					}
				} else {
					FillColor = paint.BackgroundColor;
				}
			} else if (paint is ImagePaint imagePaint) {
				var image = imagePaint.Image as GtkImage;

				if (image != null) {
					var bitmap = image.NativeImage;

					if (bitmap != null) {
						try { } catch (Exception exc) {
							Logger.Debug(exc);
							FillColor = paint.BackgroundColor;
						}
					} else {
						FillColor = Colors.White;
					}
				} else {
					FillColor = Colors.White;
				}
			} else {
				FillColor = paint.BackgroundColor;
			}
		}

		public override void FillRectangle(float x, float y, float width, float height) {
			AddRectangle(Context, x, y, width, height);
			Fill();
		}

		public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius) {
			AddRoundedRectangle(Context, x, y, width, height, cornerRadius);
			Fill();
		}

		public override void FillEllipse(float x, float y, float width, float height) {
			AddEllipse(Context, x, y, width, height);
			Fill();
		}

		public override void FillPath(PathF path, WindingMode windingMode) {
			Context.Save();
			Context.FillRule = windingMode.ToFillRule();
			AddPath(Context, path);
			Fill();
			Context.Restore();
		}

		public void DrawPixbuf(Cairo.Context context, Gdk.Pixbuf pixbuf, double x, double y, double width, double height) {
			context.Save();
			context.Translate(x, y);

			context.Scale(width / (double) pixbuf.Width, height / (double) pixbuf.Height);
			Gdk.CairoHelper.SetSourcePixbuf(context, pixbuf, 0, 0);

			using (var p = context.GetSource()) {
				if (p is Cairo.SurfacePattern pattern) {
					if (width > pixbuf.Width || height > pixbuf.Height) {
						// Fixes blur issue when rendering on an image surface
						pattern.Filter = Cairo.Filter.Fast;
					} else
						pattern.Filter = Cairo.Filter.Good;
				}
			}

			context.Paint();

			context.Restore();
		}

		public override void DrawImage(IImage image, float x, float y, float width, float height) {
			if (image is GtkImage g && g.NativeImage is Gdk.Pixbuf pixbuf) {
				DrawPixbuf(Context, pixbuf, x, y, width, height);
			}
		}

		public override void SetToSystemFont() { }

		public override void SetToBoldSystemFont() { }

		public override void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment) { }

		public override void DrawString(string value, float x, float y, float width, float height, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment, TextFlow textFlow = TextFlow.ClipBounds, float lineSpacingAdjustment = 0) { }

		public override void DrawText(IAttributedText value, float x, float y, float width, float height) { }

		public override void SubtractFromClip(float x, float y, float width, float height) { }

		public override void ClipPath(PathF path, WindingMode windingMode = WindingMode.NonZero) { }

		public override void ClipRectangle(float x, float y, float width, float height) { }

	}

}
