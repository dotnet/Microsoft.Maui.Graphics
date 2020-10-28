using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml.Media;
using Xamarin.Graphics;

namespace Elevenworks.Graphics.SharpDX
{
    public static class GraphicsExtensions
    {
        public static EWPoint AsEWPoint(this PointerPoint point, float scale = 1)
        {
            var position = point.Position;
            return new EWPoint((float)position.X * scale, (float)position.Y * scale);
        }

        public static Windows.UI.Color AsColor(this EWColor color)
        {
            if (color != null)
            {
                var uiColor = new Windows.UI.Color
                {
                    R = (byte) (255*color.Red),
                    G = (byte) (255*color.Green), 
                    B = (byte) (255*color.Blue), 
                    A = (byte) (255*color.Alpha)
                };
                return uiColor;
            }

            return Colors.Black;           
        }

        public static Brush AsBrush(this EWColor color)
        {
            if (color != null)
            {
                var uiColor = color.AsColor();
                var brush = new SolidColorBrush(uiColor);
                return brush;
            }

            return null;
        }
    }
}
