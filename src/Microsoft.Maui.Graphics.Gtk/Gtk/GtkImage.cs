using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class GtkImage : IImage {

		public GtkImage(Gdk.Pixbuf pix) {
			_pixbuf = pix;
		}

		private Gdk.Pixbuf _pixbuf;

		// https://developer.gnome.org/gdk-pixbuf/stable/gdk-pixbuf-The-GdkPixbuf-Structure.html
		public Gdk.Pixbuf NativeImage => _pixbuf;

		public void Draw(ICanvas canvas, RectangleF dirtyRect) {
			canvas.DrawImage(this, dirtyRect.Left, dirtyRect.Top, (float) Math.Round(dirtyRect.Width), (float) Math.Round(dirtyRect.Height));
		}

		public void Dispose() {
			var previousValue = Interlocked.Exchange(ref _pixbuf, null);
			previousValue?.Dispose();
		}

		public float Width => NativeImage.Width;

		public float Height => NativeImage.Width;

		[GtkMissingImplementation]
		public IImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false) {
			return this;
		}

		[GtkMissingImplementation]
		public IImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false) {
			return this;
		}

		[GtkMissingImplementation]
		public IImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false) {
			return this;
		}

		public void Save(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1) {
			NativeImage.SaveToStream(stream, format, quality);
		}

		public async Task SaveAsync(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1) {
			await Task.Run(() => NativeImage.SaveToStream(stream, format, quality));
		}

	}

}
