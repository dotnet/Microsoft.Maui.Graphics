using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;

namespace Elevenworks.Graphics
{
    public class SkiaImage : EWImage
    {
        private SKBitmap _image;

        public SkiaImage(SKBitmap image)
        {
            _image = image;
        }

        public float Width => _image.Width;

        public float Height => _image.Height;

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            // todo: implement
            /*
         var downsizedImage = image.Downsize ((int)maxWidthOrHeight, disposeOriginal);
         return new MDImage (downsizedImage);
            */
            return null;
        }

        public EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            /*
         var downsizedImage = image.Downsize ((int)maxWidth, (int)maxHeight, disposeOriginal);
         return new MDImage (downsizedImage);
            */
            return null;
        }

        public EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false)
        {
            using (var context = new SkiaBitmapExportContext((int) width, (int) height, 1))
            {
                var fx = width / Width;
                var fy = height / Height;

                var w = Width;
                var h = Height;

                var x = 0f;
                var y = 0f;

                if (resizeMode == ResizeMode.Fit)
                {
                    if (fx < fy)
                    {
                        w *= fx;
                        h *= fx;
                    }
                    else
                    {
                        w *= fy;
                        h *= fy;
                    }

                    x = (width - w) / 2;
                    y = (height - h) / 2;
                }
                else if (resizeMode == ResizeMode.Bleed)
                {
                    if (fx > fy)
                    {
                        w *= fx;
                        h *= fx;
                    }
                    else
                    {
                        w *= fy;
                        h *= fy;
                    }

                    x = (width - w) / 2;
                    y = (height - h) / 2;
                }
                else
                {
                    w = width;
                    h = height;
                }

                context.Canvas.DrawImage(this, x, y, w, h);
                return context.Image;
            }
        }

        public SKBitmap NativeImage => _image;

        public void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            // todo: implement me

            /*    
         switch (format)
         {
            case EWImageFormat.Jpeg:
               image.Compress (Bitmap.CompressFormat.Jpeg, (int)(quality * 100), stream);
               break;
            default:
               image.Compress (Bitmap.CompressFormat.Png, 100, stream);
               break;
         }
            */
        }

        public async Task SaveAsync(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            // todo: implement me

            /*
         switch (format)
         {
            case EWImageFormat.Jpeg:
               await image.CompressAsync (Bitmap.CompressFormat.Jpeg, (int)(quality * 100), stream);
               break;
            default:
               await image.CompressAsync (Bitmap.CompressFormat.Png, 100, stream);
               break;
         }
            */
        }

        public void Dispose()
        {
            var previousValue = Interlocked.Exchange(ref _image, null);
            previousValue?.Dispose();
        }

        public void Draw(EWCanvas canvas, EWRectangle dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.MinX, dirtyRect.MinY, (float)Math.Round(dirtyRect.Width), (float)Math.Round(dirtyRect.Height));
        }
    }

    public static class SkiaImageExtensions
    {
        public static SKBitmap AsBitmap(this EWImage image)
        {
            if (image is SkiaImage skiaImage)
                return skiaImage.NativeImage;

            if (image != null)
                Logger.Warn("SkiaImageExtensions.AsBitmap: Unable to get SKBitmap from EWImage. Expected an image of type SkiaImage however an image of type {0} was received.", image.GetType());

            return null;
        }
    }
}