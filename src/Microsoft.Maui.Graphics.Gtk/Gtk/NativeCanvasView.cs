using GLib;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeCanvasView : global::Gtk.EventBox {

		private IDrawable _drawable;
		private RectangleF _dirty;
		private Color _backgroundColor;

		public NativeCanvasView() {
			AppPaintable = true;
			VisibleWindow = false;
		}

		protected override bool OnDrawn(Cairo.Context cr) {
			// ensure cr does not get disposed before it is passed back to Gtk
			var context = new NativeCanvas {Context = cr};


			return base.OnDrawn(cr);
		}

		public Color BackgroundColor {
			get => _backgroundColor;
			set {
				_backgroundColor = value;
				QueueDraw();
			}
		}

		public IDrawable Drawable {
			get => _drawable;
			set {
				_drawable = value;
				QueueDraw();
			}
		}

	}

}
