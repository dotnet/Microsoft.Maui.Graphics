using System.IO;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeGraphicsService : IGraphicsService {

		public static NativeGraphicsService Instance = new NativeGraphicsService();

		[GtkMissingImplementation]
		public string SystemFontName { get; set; }

		[GtkMissingImplementation]
		public string BoldSystemFontName { get; set; }

		[GtkMissingImplementation]
		public SizeF GetStringSize(string value, string fontName, float textSize) {
			return new SizeF(value?.Length * 10 ?? 0, 10);
		}

		[GtkMissingImplementation]
		public SizeF GetStringSize(string value, string fontName, float textSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment) {
			return new SizeF(value?.Length * 10 ?? 0, 10);
		}

		public IImage LoadImageFromStream(Stream stream, ImageFormat format = ImageFormat.Png) {
			var px = new Gdk.Pixbuf(stream);
			var img = new GtkImage(px);

			return img;
		}

		public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1) {
			return new GtkBitmapExportContext(width, height, displayScale);
		}

	}

}
