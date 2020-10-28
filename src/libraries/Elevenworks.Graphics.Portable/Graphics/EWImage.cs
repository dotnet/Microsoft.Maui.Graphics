using System;
using System.IO;
using System.Threading.Tasks;

namespace Elevenworks.Graphics
{
    public enum ResizeMode
    {
        Fit,
        Bleed,
        Stretch
    }

    public interface EWImage : EWDrawable, IDisposable
    {
        float Width { get; }
        float Height { get; }
        EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false);
        EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false);
        EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false);
        void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1);
        Task SaveAsync(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1);
    }

    public static class EWImageExtensions
    {
        public static byte[] AsBytes(this EWImage target, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            if (target == null)
                return null;

            using (var stream = new MemoryStream())
            {
                target.Save(stream, format, quality);
                return stream.ToArray();
            }
        }

        public static Stream AsStream(this EWImage target, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            if (target == null)
                return null;

            var stream = new MemoryStream();
            target.Save(stream, format, quality);
            stream.Position = 0;

            return stream;
        }

        public static async Task<byte[]> AsBytesAsync(this EWImage target, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            if (target == null)
                return null;

            using (var stream = new MemoryStream())
            {
                await target.SaveAsync(stream, format, quality);
                return stream.ToArray();
            }
        }
        
        public static string AsBase64(this EWImage target, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            if (target == null)
                return null;

            var bytes = target.AsBytes(format, quality);
            return Convert.ToBase64String(bytes);
        }

        public static EWPaint AsPaint(this EWImage target)
        {
            if (target == null)
                return null;

            return new EWPaint {Image = target};
        }

        public static void SetFillImage(this EWCanvas canvas, EWImage image)
        {
            if (canvas != null)
            {
                var paint = image.AsPaint();
                if (paint != null)
                {
                    canvas.SetFillPaint(paint, 0, 0, 0, 0);
                }
                else
                {
                    canvas.FillColor = StandardColors.White;
                }
            }
        }
    }
}