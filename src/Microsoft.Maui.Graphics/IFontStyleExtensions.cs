namespace Microsoft.Maui.Graphics
{
	public static class IFontStyleExtensions
	{
		public static string GetSvgWeight(this IFontStyle style)
		{
			if (style == null)
				return null;

			if (style.Weight == FontUtils.Normal)
				return "normal";

			if (style.Weight == FontUtils.Regular)
				return "normal";

			if (style.Weight == FontUtils.Bold)
				return "bold";

			return style.Weight.ToInvariantString();
		}

		public static string GetSvgStyle(this IFontStyle style)
		{
			if (style == null)
				return null;

			if (style.StyleType == FontSlant.Italic)
				return "italic";

			if (style.StyleType == FontSlant.Oblique)
				return "oblique";

			return "normal";
		}
	}
}
