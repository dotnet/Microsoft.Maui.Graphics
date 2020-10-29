using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Xamarin.Graphics.CoreGraphics
{
    public static class NSImageExtensions
    {
        public static void Draw(this NSImage target, float x, float y)
        {
            Draw(target, new CGPoint(x, y));
        }

        public static void Draw(this NSImage target, CGPoint point)
        {
            target.Draw(new CGRect(point, target.Size), new CGRect(CGPoint.Empty, target.Size), NSCompositingOperation.SourceOver, 1, true, null);
        }

        public static void Draw(this NSImage target, CGPoint point, float alpha)
        {
            target.Draw(new CGRect(point, target.Size), new CGRect(CGPoint.Empty, target.Size), NSCompositingOperation.SourceOver, alpha, true, null);
        }

        public static NSData AsPng(this NSImage target)
        {
            var representations = target.Representations();
            if (representations != null && representations.Length > 0 && representations[0] is NSBitmapImageRep)
            {
                var rep = (NSBitmapImageRep) representations[0];
                return rep.RepresentationUsingTypeProperties(NSBitmapImageFileType.Png, new NSMutableDictionary());
            }
            else
            {
                var rect = new CGRect();
                var cgimage = target.AsCGImage(ref rect, null, null);
                var bitmap = new NSBitmapImageRep(cgimage);
                return bitmap.RepresentationUsingTypeProperties(NSBitmapImageFileType.Png, new NSMutableDictionary());
            }
        }

        public static NSData AsBmp(this NSImage target)
        {
            var representations = target.Representations();
            if (representations[0] is NSBitmapImageRep rep)
            {
                return rep.RepresentationUsingTypeProperties(NSBitmapImageFileType.Bmp, new NSMutableDictionary());
            }

            var rect = new CGRect();
            var cgimage = target.AsCGImage(ref rect, null, null);
            var bitmap = new NSBitmapImageRep(cgimage);
            return bitmap.RepresentationUsingTypeProperties(NSBitmapImageFileType.Bmp, new NSMutableDictionary());
        }

        public static NSImage ScaleImage(this NSImage target, float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            var origWidth = target.Size.Width;
            var origHeight = target.Size.Height;

            var scale = origWidth / maxWidth;
            var targetHeight = origHeight / scale;
            var targetWidth = origWidth / scale;

            if (targetHeight > maxHeight)
            {
                scale = targetHeight / maxHeight;
                targetHeight = targetHeight / scale;
                targetWidth = targetWidth / scale;
            }

            return ScaleImage(target, new CGSize(targetWidth, targetHeight), disposeOriginal);
        }

        public static NSImage ScaleImage(this NSImage target, CGSize size, bool disposeOriginal = false)
        {
            CGBitmapContext context;

            var width = (int) size.Width;
            var height = (int) size.Height;

            try
            {
                var colorspace = CGColorSpace.CreateDeviceRGB();
                context = new CGBitmapContext(IntPtr.Zero, width, height, 8, 4 * width, colorspace, CGBitmapFlags.PremultipliedFirst);
                context.SetStrokeColorSpace(colorspace);
                context.SetFillColorSpace(colorspace);
            }
            catch (Exception exc)
            {
                throw new Exception(
                    string.Format(
                        "Unable to allocate memory to scale the image to the size {0},{1}.",
                        size.Width,
                        size.Height),
                    exc);
            }

            try
            {
                context.DrawImage(new CGRect(new CGPoint(0, 0), size), target.CGImage);
                var cgimage = context.ToImage();
                var downsizedImage = new NSImage(cgimage, size);
                return downsizedImage;
            }
            finally
            {
                context.Dispose();

                if (disposeOriginal)
                    target?.Dispose();
            }
        }
    }
}