using AppKit;

namespace Xamarin.Graphics.CoreGraphics
{
    public static class GraphicsMixins
    {
        public static EWPaint AsPaint(this NSImage target)
        {
            if (target == null)
                return null;

            var image = new MMImage(target);
            var paint = new EWPaint();
            paint.Image = image;
            return paint;
        }

        public static NSColor AsNSColor(this EWColor color)
        {
            return NSColor.FromDeviceRgba(color.Red, color.Green, color.Blue, color.Alpha);
        }
    }
}