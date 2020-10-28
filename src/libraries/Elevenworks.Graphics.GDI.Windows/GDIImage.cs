using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

namespace Elevenworks.Graphics
{
    internal class GDIImage : EWImage
    {
        private Bitmap bitmap;
        private string hash;

        public GDIImage(Bitmap bitmap, string hash = null)
        {
            this.bitmap = bitmap;
            this.hash = hash;
        }

        public Bitmap NativeImage => bitmap;

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref bitmap, null);
            if (disp != null) disp.Dispose();
        }

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            if (Width > maxWidthOrHeight || Height > maxWidthOrHeight)
            {
                float factor = 1;

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

                var copy = new Bitmap(bitmap, w, h);
                var newImage = new GDIImage(copy);

                if (disposeOriginal)
                {
                    bitmap.Dispose();
                }

                return newImage;
            }

            return this;
        }

        public EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public float Width => bitmap.Size.Width;

        public float Height => bitmap.Size.Height;

        public string Hash
        {
            get
            {
                if (hash == null)
                {
                    string thehash = this.CreateHash();
                    return Interlocked.Exchange(ref hash, thehash);
                }

                return hash;
            }

            set => hash = value;
        }

        public void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            try
            {
                if (format == EWImageFormat.Jpeg)
                {
                    var jgpEncoder = GetEncoder(ImageFormat.Jpeg);
                    var myEncoder = Encoder.Quality;
                    var myEncoderParameters = new EncoderParameters(1);
                    var myEncoderParameter = new EncoderParameter(myEncoder, (long) (quality * 100));
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    bitmap.Save(stream, jgpEncoder, myEncoderParameters);
                }
                else
                {
                    bitmap.Save(stream, ImageFormat.Png);
                }
            }
            catch (Exception exc)
            {
                Logger.Warn(exc);
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
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

    public static class GDIImageExtensions
    {
        public static Bitmap AsBitmap(this EWImage image)
        {
            var dxImage = image as GDIImage;
            if (dxImage != null)
            {
                return dxImage.NativeImage;
            }

            var virtualImage = image as VirtualImage;
            if (virtualImage != null)
            {
                using (var stream = new MemoryStream(virtualImage.Bytes))
                {
                    return new Bitmap(stream);
                }
            }

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