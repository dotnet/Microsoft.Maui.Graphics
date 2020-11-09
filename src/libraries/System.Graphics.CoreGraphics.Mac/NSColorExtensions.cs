using AppKit;

namespace System.Graphics.CoreGraphics
{
    public static class NSColorExtensions
    {
        public static Color AsColor(this NSColor color)
        {
            var convertedColorspace = color.UsingColorSpace(NSColorSpace.GenericRGBColorSpace);
            convertedColorspace.GetRgba(out var red, out var green, out var blue, out var alpha);
            return new Color((float) red, (float) green, (float) blue, (float) alpha);
        }
    }
}