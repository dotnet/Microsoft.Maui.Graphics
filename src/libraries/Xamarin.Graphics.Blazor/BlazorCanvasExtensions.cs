using Elevenworks.Graphics.Blazor.Canvas2D;
using Xamarin.Graphics;

namespace Elevenworks.Graphics.Blazor
{
    public static class BlazorCanvasExtensions
    {
        public static LineCap AsCanvasValue(
            this EWLineCap target)
        {
            switch (target)
            {
                case EWLineCap.BUTT:
                    return LineCap.Butt;
                case EWLineCap.ROUND:
                    return LineCap.Round;
                case EWLineCap.SQUARE:
                    return LineCap.Square;
            }

            return LineCap.Butt;
        }

        public static LineJoin AsCanvasValue(
            this EWLineJoin target)
        {
            switch (target)
            {
                case EWLineJoin.MITER:
                    return LineJoin.Miter;
                case EWLineJoin.ROUND:
                    return LineJoin.Round;
                case EWLineJoin.BEVEL:
                    return LineJoin.Bevel;
            }

            return LineJoin.Miter;
        }

        public static string AsCanvasValue(
            this EWColor color,
            string defaultValue = "black")
        {
            if (color != null)
                return color.ToHexString();

            return defaultValue;
        }
    }
}
