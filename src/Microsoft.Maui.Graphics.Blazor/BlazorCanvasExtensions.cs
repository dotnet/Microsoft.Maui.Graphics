using Microsoft.Maui.Graphics.Blazor.Canvas2D;

namespace Microsoft.Maui.Graphics.Blazor
{
	public static class BlazorCanvasExtensions
	{
		public static Canvas2D.LineCap AsCanvasValue(
			this PenLineCap target)
		{
			switch (target)
			{
				case PenLineCap.Flat:
					return Canvas2D.LineCap.Butt;
				case PenLineCap.Round:
					return Canvas2D.LineCap.Round;
				case PenLineCap.Square:
					return Canvas2D.LineCap.Square;
			}

			return Canvas2D.LineCap.Butt;
		}

		public static Canvas2D.LineJoin AsCanvasValue(
			this PenLineJoin target)
		{
			switch (target)
			{
				case PenLineJoin.Miter:
					return Canvas2D.LineJoin.Miter;
				case PenLineJoin.Round:
					return Canvas2D.LineJoin.Round;
				case PenLineJoin.Bevel:
					return Canvas2D.LineJoin.Bevel;
			}

			return Canvas2D.LineJoin.Miter;
		}

		public static string AsCanvasValue(
			this Color color,
			string defaultValue = "black")
		{
			if (color != null)
				return color.ToHex();

			return defaultValue;
		}
	}
}
