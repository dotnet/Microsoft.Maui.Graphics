using System;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class PaintExtensions {

		public static Cairo.Context PaintToSurface(this PatternPaint it, Cairo.Surface surface, float scale) {
			var context = new Cairo.Context(surface);
			context.Scale(scale, scale);

			using var canv = new NativeCanvas {
				Context = context,
			};

			it.Pattern.Draw(canv);

			return context;

		}

		/*
		  Cairo.Extend is used to describe how pattern color/alpha will be determined for areas "outside" the pattern's natural area,
		  (for example, outside the surface bounds or outside the gradient geometry).
		  The default extend mode is CAIRO_EXTEND_NONE for surface patterns and CAIRO_EXTEND_PAD for gradient patterns.

		   NONE	    pixels outside of the source pattern are fully transparent
		   REPEAT   the pattern is tiled by repeating
		   REFLECT  the pattern is tiled by reflecting at the edges (Implemented for surface patterns since 1.6)
		   PAD      pixels outside of the pattern copy the closest pixel from the source (only implemented for surface patterns since 1.6)

		 */

		public static void SetCairoExtend(Cairo.Extend it) { }

		public static Gdk.Pixbuf GetPatternBitmap(this PatternPaint it, float scale) {
			using var surface = new Cairo.ImageSurface(Cairo.Format.Argb32, (int) it.Pattern.Width, (int) it.Pattern.Height);
			using var context = it.PaintToSurface(surface, scale);
			surface.Flush();

			return surface.CreatePixbuf();

		}

		/// <summary>
		/// does not work, pattern isn't shown
		/// </summary>

		[GtkMissingImplementation]
		public static Cairo.Pattern GetCairoPattern(this PatternPaint it, Cairo.Surface surface, float scale) {

			using var context = it.PaintToSurface(surface, scale);
			surface.Flush();

			var pattern = new Cairo.SurfacePattern(surface);

			return pattern;
		}

		public static Cairo.Pattern GetCairoPattern(this LinearGradientPaint linearGradientPaint, RectangleF rectangle, float scaleFactor) {
			var x1 = linearGradientPaint.StartPoint.X * rectangle.Width + rectangle.X;
			var y1 = linearGradientPaint.StartPoint.Y * rectangle.Height + rectangle.Y;

			var x2 = linearGradientPaint.EndPoint.X * rectangle.Width + rectangle.X;
			var y2 = linearGradientPaint.EndPoint.Y * rectangle.Height + rectangle.Y;

			// https://developer.gnome.org/cairo/stable/cairo-cairo-pattern-t.html#cairo-pattern-create-linear
			var pattern = new Cairo.LinearGradient(x1, y1, x2, y2);

			foreach (var s in linearGradientPaint.GetSortedStops()) {
				pattern.AddColorStop(s.Offset, s.Color.ToCairoColor());
			}

			return pattern;
		}

		public static Cairo.Pattern GetCairoPattern(this RadialGradientPaint radialGradientPaint, RectangleF rectangle, float scaleFactor) {

			var centerX = radialGradientPaint.Center.X * rectangle.Width;
			var centerY = radialGradientPaint.Center.Y * rectangle.Height;

			var x1 = centerX + rectangle.X;
			var y1 = centerY + rectangle.Y;

			var x2 = rectangle.Right - centerX;
			var y2 = rectangle.Bottom - centerY;

			var radius1 = radialGradientPaint.Radius * 1;
			var radius2 = radialGradientPaint.Radius * Math.Max(rectangle.Width, rectangle.Height);

			// https://developer.gnome.org/cairo/stable/cairo-cairo-pattern-t.html#cairo-pattern-create-radial
			var pattern = new Cairo.RadialGradient(x1, y1, radius1, x2, y2, radius2);

			foreach (var s in radialGradientPaint.GetSortedStops()) {
				pattern.AddColorStop(s.Offset, s.Color.ToCairoColor());
			}

			return pattern;
		}

	}

}
