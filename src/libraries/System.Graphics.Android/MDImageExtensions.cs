using Android.Graphics;

namespace System.Graphics.Android
{
    public static class MDImageExtensions
    {
        public static Bitmap AsBitmap(this IImage image)
        {
            if (image is MDImage mdimage)
            {
                return mdimage.NativeImage;
            }

            if (image != null)
            {
                Logger.Warn("MDImageExtensions.AsBitmap: Unable to get Bitmap from EWImage. Expected an image of type MDImage however an image of type {0} was received.", image.GetType());
            }

            return null;
        }
    }
}