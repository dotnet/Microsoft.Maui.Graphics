using System.Drawing;
using System.IO;

namespace Elevenworks.Graphics
{
    public static class GDIImageExtensions
    {
        public static Bitmap AsBitmap(this EWImage image)
        {
            if (image is GDIImage dxImage)
                return dxImage.NativeImage;

            if (image is VirtualImage virtualImage)
                using (var stream = new MemoryStream(virtualImage.Bytes))
                    return new Bitmap(stream);

            if (image != null)
            {
                Logger.Warn(
                    "GDIImageExtensions.AsBitmap: Unable to get Bitmap from EWImage. Expected an image of type GDIImage however an image of type {0} was received.",
                    image.GetType());
            }

            return null;
        }
    }
}