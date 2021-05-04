using System;
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

		public static Gdk.Pixbuf GetPatternBitmap(this Paint it, float scale) => null;

	}

}
