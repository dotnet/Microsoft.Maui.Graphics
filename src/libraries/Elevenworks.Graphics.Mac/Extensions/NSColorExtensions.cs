using System;
using AppKit;

namespace Elevenworks.Graphics
{
    public static class NSColorExtensions
    {
        public static EWColor AsEWColor(this NSColor color)
        {
            var convertedColorspace = color.UsingColorSpace(NSColorSpace.GenericRGBColorSpace);
            convertedColorspace.GetRgba(out var red, out var green, out var blue, out var alpha);
            return new EWColor((float) red, (float) green, (float) blue, (float) alpha);
        }
    }
}