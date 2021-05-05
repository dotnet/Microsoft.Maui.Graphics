using System;
using Cairo;
using Gtk;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class ImageExtensions {

		public static string ToLineJoin(this ImageFormat imageFormat) =>
			imageFormat switch {
				ImageFormat.Bmp => "bmp",
				ImageFormat.Png => "png",
				ImageFormat.Jpeg => "jpg",
				ImageFormat.Gif => "gif",
				ImageFormat.Tiff => "tiff",
				_ => throw new ArgumentOutOfRangeException(nameof(imageFormat), imageFormat, null)
			};

		[GtkMissingImplementation]
		public static Gdk.Pixbuf GetPatternBitmap(this Paint it, float scale) {
			// TODO:
			if (it is PatternPaint pat) {
				using var sf = new Cairo.ImageSurface(Format.Argb32, (int) pat.Pattern.Width, (int) pat.Pattern.Height);
				using var ctx = new Cairo.Context(sf);

				using var canv = new NativeCanvas {
					Context = ctx,
					DisplayScale = scale,
				};

				pat.Pattern.Draw(canv);
				sf.Flush();
			}

			return default;
		}

	}

}
