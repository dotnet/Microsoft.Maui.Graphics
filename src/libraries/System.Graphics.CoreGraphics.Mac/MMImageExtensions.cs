using AppKit;

namespace System.Graphics.CoreGraphics
{
    public static class MMImageExtensions
    {
        public static NSImage AsNSImage(this IImage image)
        {
            if (image is MMImage macImage)
                return macImage.NativeImage;

            if (image != null)
                Logger.Warn("MMImageExtensions.AsNSImage: Unable to get NSImage from EWImage. Expected an image of type MMImage however an image of type {0} was received.", image.GetType());

            return null;
        }
    }
}