using GLib;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeCanvasView : global::Gtk.EventBox {

		private IDrawable _drawable;
		private RectangleF _dirtyRect;
		private Color _backgroundColor;

		public NativeCanvasView() {
			AppPaintable = true;
			VisibleWindow = false;
		}

		protected override bool OnDrawn(Cairo.Context cr) {
			// ensure cr does not get disposed before it is passed back to Gtk
			var context = new NativeCanvas {Context = cr};

			context.SaveState();

			if (_backgroundColor != null)
			{
				context.FillColor = _backgroundColor;
				context.FillRectangle(_dirtyRect);
			}
			else
			{
				context.ClipRectangle(_dirtyRect);
			}

			context.RestoreState();
			Drawable?.Draw(context, _dirtyRect);

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

		protected override void OnSizeAllocated (Gdk.Rectangle allocation) {
			_dirtyRect.Width = allocation.Width;
			_dirtyRect.Height = allocation.Height;
			base.OnSizeAllocated (allocation);
		}
	}

}
