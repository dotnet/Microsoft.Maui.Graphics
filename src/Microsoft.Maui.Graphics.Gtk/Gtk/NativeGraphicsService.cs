using System.IO;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeGraphicsService : IGraphicsService {

		public static NativeGraphicsService Instance = new NativeGraphicsService();

		static Cairo.Context? _sharedContext;

		public Cairo.Context SharedContext {
			get {
				if (_sharedContext == null) {
					using var sf = new Cairo.ImageSurface(Cairo.Format.ARGB32, 1, 1);
					_sharedContext = new Cairo.Context(sf);
				}

				return _sharedContext;
			}
		}

	}

	public string SystemFontName => NativeFontService.Instance.SystemFontName;

	public string BoldSystemFontName => NativeFontService.Instance.BoldSystemFontName;

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
