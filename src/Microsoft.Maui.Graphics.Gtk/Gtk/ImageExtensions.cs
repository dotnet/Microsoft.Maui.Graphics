using System;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class ImageExtensions {

		public static string ToImageExtension(this ImageFormat imageFormat) =>
			imageFormat switch {
				ImageFormat.Bmp => "bmp",
				ImageFormat.Png => "png",
				ImageFormat.Jpeg => "jpg",
				ImageFormat.Gif => "gif",
				ImageFormat.Tiff => "tiff",
				_ => throw new ArgumentOutOfRangeException(nameof(imageFormat), imageFormat, null)
			};

		public static Gdk.Pixbuf CreatePixbuf(this Cairo.ImageSurface sf) {
			byte[] surfaceData = sf.Data;
			var nbytes = sf.Format == Cairo.Format.Argb32 ? 4 : 3;
			byte[] pixData = new byte[(surfaceData.Length / 4) * nbytes];

			var i = 0;
			var n = 0;
			var stride = sf.Stride;
			var ncols = sf.Width;

			if (BitConverter.IsLittleEndian) {
				var row = sf.Height;

				while (row-- > 0) {
					var prevPos = n;
					var col = ncols;

					while (col-- > 0) {
						var alphaFactor = nbytes == 4 ? 255d / surfaceData[n + 3] : 1;
						pixData[i] = (byte) (surfaceData[n + 2] * alphaFactor + 0.5);
						pixData[i + 1] = (byte) (surfaceData[n + 1] * alphaFactor + 0.5);
						pixData[i + 2] = (byte) (surfaceData[n + 0] * alphaFactor + 0.5);

						if (nbytes == 4)
							pixData[i + 3] = surfaceData[n + 3];

						n += 4;
						i += nbytes;
					}

					n = prevPos + stride;
				}
			} else {
				var row = sf.Height;

				while (row-- > 0) {
					var prevPos = n;
					var col = ncols;

					while (col-- > 0) {
						var alphaFactor = nbytes == 4 ? 255d / surfaceData[n + 3] : 1;
						pixData[i] = (byte) (surfaceData[n + 1] * alphaFactor + 0.5);
						pixData[i + 1] = (byte) (surfaceData[n + 2] * alphaFactor + 0.5);
						pixData[i + 2] = (byte) (surfaceData[n + 3] * alphaFactor + 0.5);

						if (nbytes == 4)
							pixData[i + 3] = surfaceData[n + 0];

						n += 4;
						i += nbytes;
					}

					n = prevPos + stride;
				}
			}

			return new Gdk.Pixbuf(pixData, Gdk.Colorspace.Rgb, nbytes == 4, 8, sf.Width, sf.Height, sf.Width * nbytes, null);
		}

		public static Cairo.Pattern CreatePattern(this Gdk.Pixbuf pixbuf, double scaleFactor) {

			using var surface = new Cairo.ImageSurface(Cairo.Format.Argb32, (int) (pixbuf.Width * scaleFactor), (int) (pixbuf.Height * scaleFactor));
			using var context = new Cairo.Context(surface);
			context.Scale((double) surface.Width / (double) pixbuf.Width, (double) surface.Height / (double) pixbuf.Height);
			Gdk.CairoHelper.SetSourcePixbuf(context, pixbuf, 0, 0);
			context.Paint();
			surface.Flush();

			var pattern = new Cairo.SurfacePattern(surface) {

			};

			var matrix = new Cairo.Matrix();
			matrix.Scale(scaleFactor, scaleFactor);
			pattern.Matrix = matrix;

			return pattern;

		}

	}

}
