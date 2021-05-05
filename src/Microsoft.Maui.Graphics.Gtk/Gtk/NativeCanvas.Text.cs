using Context = Cairo.Context;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public partial class NativeCanvas {

		private void draw_text(Context cr, IFontStyle fs, int rectangle_width, int rectangle_height, string text) {
			var font = new Pango.FontDescription();

			font.Family = fs.FontFamily.Name;
			font.Weight = FontExtensions.ToFontWeigth(fs.Weight);

			Pango.Layout layout = Pango.CairoHelper.CreateLayout(cr);

			layout.FontDescription = font;

			int text_width;
			int text_height;

			//get the text dimensions (it updates the variables -- by reference)
			layout.GetPixelSize(out text_width, out text_height);

			// Position the text in the middle
			cr.MoveTo((rectangle_width - text_width) / 2d, (rectangle_height - text_height) / 2d);

			Pango.CairoHelper.ShowLayout(cr, layout);
		}

	}

}
