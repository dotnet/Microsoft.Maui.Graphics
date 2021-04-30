using Pango;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class FontExtensions {

		/// <summary>
		/// size in points
		/// <seealso cref="https://developer.gnome.org/pygtk/stable/class-pangofontdescription.html#method-pangofontdescription--set-size"/>
		/// the size of a font description is specified in pango units.
		/// There are <see cref="Pango.Scale.PangoScale"/> pango units in one device unit (the device unit is a point for font sizes).
		/// </summary>
		/// <param name="it"></param>
		/// <returns></returns>
		public static double GetSize(this Pango.FontDescription it)
			=> it.Size / Pango.Scale.PangoScale;

		public static double GetPangoSize(double it)
			=> it * Pango.Scale.PangoScale;

		public static Pango.Style ToFontStyle(this FontStyleType it) => it switch {
			FontStyleType.Oblique => Pango.Style.Oblique, // ??
			FontStyleType.Italic => Pango.Style.Italic,
			_ => Pango.Style.Normal
		};

		// enum Pango.Weight { Thin = 100, Ultralight = 200, Light = 300, Semilight = 350, Book = 380, Normal = 400, Medium = 500, Semibold = 600, Bold = 700, Ultrabold = 800, Heavy = 900, Ultraheavy = 1000,}

		public static Pango.Weight ToFontWeigth(double it) {
			Pango.Weight Div(double v) => (Pango.Weight) ((int) v / 100  * 100);
			switch (it) {
				case < 100:
					return Weight.Thin;
				case > 1000:
					return Weight.Ultraheavy;
				case < 325:
					return Div(it);
				case > 390:
					return Div(it);

				// missing: Semilight = 350, Book = 380

			}

		}


		public static double ToFontWeigth(this Pango.Weight it)
			=> (int) it;

		public static Pango.FontDescription ToFontStyle(this IFontStyle it) =>
			new() {
				Style = it.StyleType.ToFontStyle(),
				Weight = ToFontWeigth(it.Weight),
				Family = it.FontFamily?.Name,

			};

	}

}
