using AppKit;

namespace Xamarin.Graphics.CoreGraphics
{
    public static class MMGraphicsExtensions
    {
        public static MMImage ToMultiResolutionImage(this EWDrawable drawable, int width, int height)
        {
            if (drawable == null)
                return null;

            var normalImage = drawable.ToImage(width, height);
            var normalNativeImage = normalImage.AsNSImage();
            var normalImageData = normalNativeImage.AsTiff();
            var normalImageRep = (NSBitmapImageRep) NSBitmapImageRep.ImageRepFromData(normalImageData);

            var retinaImage = drawable.ToImage(width * 2, height * 2, 2);
            var retinaNativeImage = retinaImage.AsNSImage();
            var retinaImageData = retinaNativeImage.AsTiff();
            var retinaImageRep = (NSBitmapImageRep) NSBitmapImageRep.ImageRepFromData(retinaImageData);

            var combinedImage = new NSImage {MatchesOnMultipleResolution = true};
            combinedImage.AddRepresentations(new NSImageRep[] {normalImageRep, retinaImageRep});

            return new MMImage(combinedImage);
        }
    }
}