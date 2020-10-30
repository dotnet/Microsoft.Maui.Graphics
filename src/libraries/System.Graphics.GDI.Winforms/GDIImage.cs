using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Graphics.GDI
{
    internal class GDIImage : EWImage
    {
        private Bitmap _bitmap;

        public GDIImage(Bitmap bitmap)
        {
            this._bitmap = bitmap;
        }

        public Bitmap NativeImage => _bitmap;

        public void Dispose()
        {
            var bitmap = Interlocked.Exchange(ref _bitmap, null);
            bitmap?.Dispose();
        }

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            if (Width > maxWidthOrHeight || Height > maxWidthOrHeight)
            {
                float factor ;

                if (Width > Height)
                    factor = maxWidthOrHeight / Width;
                else
                    factor = maxWidthOrHeight / Height;

                var w = (int) Math.Round(factor * Width);
                var h = (int) Math.Round(factor * Height);

                var copy = new Bitmap(_bitmap, w, h);
                var newImage = new GDIImage(copy);

                if (disposeOriginal)
                    _bitmap.Dispose();

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

        public float Width => _bitmap.Size.Width;

        public float Height => _bitmap.Size.Height;
        
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

                    _bitmap.Save(stream, jgpEncoder, myEncoderParameters);
                }
                else
                {
                    _bitmap.Save(stream, ImageFormat.Png);
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
}