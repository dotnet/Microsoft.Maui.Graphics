using System.IO;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeGraphicsService : IGraphicsService {

		public static NativeGraphicsService Instance = new NativeGraphicsService();

		public string SystemFontName { get; set; }

		public string BoldSystemFontName { get; set; }

		public SizeF GetStringSize(string value, string fontName, float textSize) {
			throw new System.NotImplementedException();
		}

		public SizeF GetStringSize(string value, string fontName, float textSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment) {
			throw new System.NotImplementedException();
		}

		public IImage LoadImageFromStream(Stream stream, ImageFormat format = ImageFormat.Png) {
			throw new System.NotImplementedException();
		}

		public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1) {
			throw new System.NotImplementedException();
		}

	}

}
