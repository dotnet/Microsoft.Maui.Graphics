using System;
using System.IO;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class GtkBitmapExportContext : BitmapExportContext {

		public GtkBitmapExportContext(int width, int height, float dpi) : base(width, height, dpi) { }

		public override ICanvas Canvas { get; }

		public override void WriteToStream(Stream stream) {
		}

		public override IImage Image { get; }

	}

}
