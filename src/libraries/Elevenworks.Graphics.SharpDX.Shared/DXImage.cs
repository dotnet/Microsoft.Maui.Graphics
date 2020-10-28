using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SharpDX.Direct2D1;

namespace Elevenworks.Graphics.SharpDX
{
    public class DXImage : EWImage
    {
        private Bitmap _bitmap;
        private string _hash;

        public DXImage(Bitmap bitmap, string hash = null)
        {
            _bitmap = bitmap;
            _hash = hash;
        }

        public Bitmap NativeImage => _bitmap;

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref _bitmap, null);
            disp?.Dispose();
        }

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            if (Width > maxWidthOrHeight || Height > maxWidthOrHeight)
            {
                float factor;

                if (Width > Height)
                {
                    factor = maxWidthOrHeight / Width;
                }
                else
                {
                    factor = maxWidthOrHeight / Height;
                }

                var w = (int) Math.Round(factor * Width);
                var h = (int) Math.Round(factor * Height);

                using (var memoryStream = new MemoryStream())
                {
                    Save(memoryStream);
                    memoryStream.Position = 0;

                    global::SharpDX.WIC.Bitmap wicBitmap = WicBitmapExtensions.LoadFromStream(memoryStream);
                    using (var memoryStream2 = new MemoryStream())
                    {
                        wicBitmap.WritePngToStream(memoryStream2, w, h);
                        memoryStream2.Position = 0;
                        var newImage = DXGraphicsService.Instance.LoadImageFromStream(memoryStream2);
                        if (disposeOriginal)
                        {
                            _bitmap.Dispose();
                        }

                        return newImage;
                    }
                }
            }

            return this;
        }

        public EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit,
            bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public float Width => _bitmap.Size.Width;

        public float Height => _bitmap.Size.Height;

        public string Hash
        {
            get
            {
                if (_hash == null)
                {
                    string thehash = this.CreateHash();
                    return Interlocked.Exchange(ref _hash, thehash);
                }

                return _hash;
            }

            set => _hash = value;
        }

        public void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case EWImageFormat.Jpeg:
                    _bitmap.EncodeImage(ImageFormat.Jpeg, stream);
                    break;
                default:
                    _bitmap.EncodeImage(ImageFormat.Png, stream);
                    break;
            }
        }

        public Task SaveAsync(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            return Task.Factory.StartNew(() => Save(stream, format, quality));
        }

        public void Draw(EWCanvas canvas, EWRectangle dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.MinX, dirtyRect.MinY, Math.Abs(dirtyRect.Width), Math.Abs(dirtyRect.Height));
        }
    }

    public static class DXImageExtensions
    {
        public static Bitmap AsBitmap(this EWImage image)
        {
            if (image is DXImage dxImage)
            {
                return dxImage.NativeImage;
            }

            if (image is VirtualImage virtualImage)
            {
                using (var stream = new MemoryStream(virtualImage.Bytes))
                {
                    return DXGraphicsService.CurrentTarget.Value.LoadBitmap(stream);
                }
            }

            if (image != null)
            {
                Logger.Warn(
                    "DXImageExtensions.AsBitmap: Unable to get Bitmap from EWImage. Expected an image of type DXImage however an image of type {0} was received.",
                    image.GetType());
            }

            return null;
        }
    }
}