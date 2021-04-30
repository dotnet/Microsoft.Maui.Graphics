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

		public static Pango.Weight ToFontWeigth(double it)
			=> Pango.Weight.Normal;

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
