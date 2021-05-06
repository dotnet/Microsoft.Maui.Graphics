using Microsoft.Maui.Graphics.Extras;

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
			var (width, height) = it.GetPixelSize(text, (int) textHeigth);

			return new Size(width, height);
		}

		public static Pango.Alignment ToPango(this HorizontalAlignment it) => it switch {
			HorizontalAlignment.Center => Pango.Alignment.Center,
			HorizontalAlignment.Right => Pango.Alignment.Right,
			_ => Pango.Alignment.Left
		};

		public static Pango.WrapMode ToPangoWrap(this Extras.LineBreakMode it) {
			if (it.HasFlag(Extras.LineBreakMode.CharacterWrap))
				return Pango.WrapMode.WordChar;
			else if (it.HasFlag(Extras.LineBreakMode.WordCharacterWrap))
				return Pango.WrapMode.Word;
			else
				return Pango.WrapMode.Char;
		}

		public static Pango.EllipsizeMode ToPangoEllipsize(this Extras.LineBreakMode lbm) {
			var it = (LineBreakFlags) lbm;

			if (it.HasFlag(Extras.LineBreakFlags.Elipsis | Extras.LineBreakFlags.End))
				return Pango.EllipsizeMode.End;

			if (it.HasFlag(Extras.LineBreakFlags.Elipsis | Extras.LineBreakFlags.Center))
				return Pango.EllipsizeMode.Middle;

			if (it.HasFlag(Extras.LineBreakFlags.Elipsis | Extras.LineBreakFlags.Start))
				return Pango.EllipsizeMode.Start;

			if (it.HasFlag(Extras.LineBreakFlags.Elipsis))
				return Pango.EllipsizeMode.End;

			return Pango.EllipsizeMode.None;
		}

	}

}
