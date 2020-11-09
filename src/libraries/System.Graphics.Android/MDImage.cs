using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Android.Graphics;

namespace System.Graphics.Android
{
    public class MDImage : IImage
    {
        private Bitmap _image;

        public MDImage(Bitmap image)
        {
            _image = image;
        }

        public float Width => _image.Width;

        public float Height => _image.Height;

        public IImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            var downsizedImage = _image.Downsize((int) maxWidthOrHeight, disposeOriginal);
            return new MDImage(downsizedImage);
        }

        public IImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            var downsizedImage = _image.Downsize((int) maxWidth, (int) maxHeight, disposeOriginal);
            return new MDImage(downsizedImage);
        }

        public IImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false)
        {
            using (var context = new MDBitmapExportContext((int) width, (int) height))
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

        public Bitmap NativeImage => _image;
        
        public void Save(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case ImageFormat.Jpeg:
                    _image.Compress(Bitmap.CompressFormat.Jpeg, (int) (quality * 100), stream);
                    break;
                default:
                    _image.Compress(Bitmap.CompressFormat.Png, 100, stream);
                    break;
            }
        }

        public async Task SaveAsync(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case ImageFormat.Jpeg:
                    await _image.CompressAsync(Bitmap.CompressFormat.Jpeg, (int) (quality * 100), stream);
                    break;
                default:
                    await _image.CompressAsync(Bitmap.CompressFormat.Png, 100, stream);
                    break;
            }
        }

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref _image, null);
            disp?.Dispose();
        }

        public void Draw(ICanvas canvas, EWRectangle dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.MinX, dirtyRect.MinY, (float) Math.Round(dirtyRect.Width), (float) Math.Round(dirtyRect.Height));
        }
    }

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