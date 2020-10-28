using System;
using System.IO;
using System.Threading.Tasks;

namespace Elevenworks.Graphics
{
    public static class EWPictureWriterExtensions
    {
        public static byte[] SaveAsBytes(this EWPictureWriter target, EWPicture picture)
        {
            if (target == null || picture == null)
                return null;

            using (var stream = new MemoryStream())
            {
                target.Save(picture, stream);
                return stream.ToArray();
            }
        }

        public static async Task<byte[]> SaveAsBytesAsync(this EWPictureWriter target, EWPicture picture)
        {
            if (target == null || picture == null)
                return null;

            using (var stream = new MemoryStream())
            {
                await target.SaveAsync(picture, stream);
                return stream.ToArray();
            }
        }

        public static string SaveAsBase64(this EWPictureWriter target, EWPicture picture)
        {
            if (target == null)
                return null;

            var bytes = target.SaveAsBytes(picture);
            return Convert.ToBase64String(bytes);
        }

        public static Stream SaveAsStream(this EWPictureWriter target, EWPicture picture)
        {
            if (target == null)
                return null;

            var bytes = target.SaveAsBytes(picture);
            return new MemoryStream(bytes);
        }
    }
}