namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class TextLayoutExtensions {

		public static void SetFontStyle(this TextLayout it, IFontStyle fs) {
			it.FontFamily = fs.FontFamily.Name;
			it.Weight = FontExtensions.ToFontWeigth(fs.Weight);
			it.Style = fs.StyleType.ToPangoStyle();
		}

		public static void SetCanvasState(this TextLayout it, NativeCanvasState state) {
			it.FontFamily = state.FontName;
			it.PangoFontSize = state.FontSize.ScaledToPango();
		}

		public static Size GetSize(this TextLayout it, string text, float textHeigth) {
			var (width, height) = it.GetPixelSize(text);

			return new Size(width.ScaledFromPango(), height.ScaledFromPango());
		}

	}

}
