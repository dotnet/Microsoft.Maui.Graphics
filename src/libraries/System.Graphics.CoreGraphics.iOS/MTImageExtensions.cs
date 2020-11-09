using UIKit;

namespace System.Graphics.CoreGraphics
{
    public static class MTImageExtensions
    {
        public static UIImage AsUIImage(this IImage image)
        {
            if (image is MTImage mtimage)
            {
                return mtimage.NativeImage;
            }

            if (image != null)
            {
                Logger.Warn("MTImageExtensions.AsUIImage: Unable to get UIImage from EWImage. Expected an image of type MTImage however an image of type {0} was received.", image.GetType());
            }

            return null;
        }
    }
}