using System.Drawing;
using UIKit;

namespace System.Graphics.CoreGraphics
{
    public static class UIImageExtensions
    {
        public static UIImage ScaleImage(this UIImage target, float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            float originalWidth = (float) target.Size.Width;
            float originalHeight = (float) target.Size.Height;

            float scale = originalWidth / maxWidth;

            float targetWidth = originalWidth / scale;
            float targetHeight = originalHeight / scale;

            if (targetHeight > maxHeight)
            {
                scale = targetHeight / maxHeight;
                targetHeight = targetHeight / scale;
                targetWidth = targetWidth / scale;
            }

            return ScaleImage(target, new SizeF(targetWidth, targetHeight), disposeOriginal);
        }

        public static UIImage ScaleImage(this UIImage target, SizeF size, bool disposeOriginal = false)
        {
            UIGraphics.BeginImageContext(size);
            target.Draw(new RectangleF(new PointF(0, 0), size));
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            if (disposeOriginal)
            {
                target.Dispose();
            }

            return image;
        }

        public static UIImage NormalizeOrientation(this UIImage target, bool disposeOriginal = false)
        {
            if (target.Orientation == UIImageOrientation.Up)
            {
                return target;
            }

            UIGraphics.BeginImageContextWithOptions(target.Size, false, target.CurrentScale);
            target.Draw(new PointF(0, 0));
            var image = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            if (disposeOriginal)
            {
                target.Dispose();
            }

            return image;
        }
    }
}