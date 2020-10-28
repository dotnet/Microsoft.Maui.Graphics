using Xamarin.Forms;

namespace Elevenworks.Graphics
{
    public static class ColorExtensions
    {
        public static Color AsFormsColor(this EWColor color)
        {
            if (color == null)
                return Color.Black;

            return new Color(color.Red, color.Green, color.Blue, color.Alpha);
        }
    }
}