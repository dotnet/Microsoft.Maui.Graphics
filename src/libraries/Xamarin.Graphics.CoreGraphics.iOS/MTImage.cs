using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Xamarin.Graphics.CoreGraphics
{
    public class MTImage : EWImage
    {
        private UIImage _image;

        public MTImage(UIImage image)
        {
            _image = image;
        }

        public float Width => (float) (_image?.Size.Width ?? 0);

        public float Height => (float) (_image?.Size.Height ?? 0);

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            var scaledImage = _image.ScaleImage(maxWidthOrHeight, maxWidthOrHeight, disposeOriginal);
            return new MTImage(scaledImage);
        }

        public EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            var scaledImage = _image.ScaleImage(maxWidth, maxHeight, disposeOriginal);
            return new MTImage(scaledImage);
        }

        public EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false)
        {
            using (var context = new MTBitmapExportContext((int) width, (int) height, 1))
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

        public UIImage NativeImage => _image;
        
        public void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            var data = CreateData(format, quality);
            data.AsStream().CopyTo(stream);
        }

        public async Task SaveAsync(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            var data = CreateData(format, quality);
            await data.AsStream().CopyToAsync(stream);
        }

        private NSData CreateData(EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            NSData data;
            switch (format)
            {
                case EWImageFormat.Jpeg:
                    data = _image.AsJPEG(quality);
                    break;
                default:
                    data = _image.AsPNG();
                    break;
            }

            if (data == null)
            {
                throw new Exception($"Unable to write the image in the {format} format.");
            }

            return data;
        }

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref _image, null);
            disp?.Dispose();
        }

        public void Draw(EWCanvas canvas, EWRectangle dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.MinX, dirtyRect.MinY, (float)Math.Round(dirtyRect.Width), (float)Math.Round(dirtyRect.Height));
        }
    }

    public static class MTImageExtensions
    {
        public static UIImage AsUIImage(this EWImage image)
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