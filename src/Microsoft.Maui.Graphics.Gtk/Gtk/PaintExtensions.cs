namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class PaintExtensions {

		public static Cairo.Context PaintToSurface(this PatternPaint it, Cairo.Surface sf, float scale) {
			var ctx = new Cairo.Context(sf);

			ctx.Scale(scale, scale);

			using var canv = new NativeCanvas {
				Context = ctx,
			};

			it.Pattern.Draw(canv);

			return ctx;

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

		public static Gdk.Pixbuf GetPatternBitmap(this Paint it, float scale) {
			if (it is PatternPaint pat) {
				using var sf = new Cairo.ImageSurface(Cairo.Format.Argb32, (int) pat.Pattern.Width, (int) pat.Pattern.Height);
				using var ctx = pat.PaintToSurface(sf, scale);
				sf.Flush();

				return sf.CreatePixbuf();
			}

			return default;
		}

		public static Cairo.Pattern GetCairoPattern(this PatternPaint pat, Cairo.Surface surface, float scale) {

			using var ctx = pat.PaintToSurface(surface, scale);
			surface.Flush();
			var pattern = new Cairo.SurfacePattern(surface) { };

			return pattern;
		}

	}

}
