using System.IO;
using System.Threading.Tasks;

namespace Elevenworks.Graphics
{
    public interface EWPictureReader
    {
        EWPicture Read(byte[] data, string hash = null);
    }

    public static class EWPictureReaderExtensions
    {
        public static EWPicture Read(this EWPictureReader target, Stream stream, string hash = null)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream == null)
            {
                memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
            }

            var bytes = memoryStream.ToArray();
            return target.Read(bytes, hash);
        }

        public static async Task<EWPicture> ReadAsync(this EWPictureReader target, Stream stream, string hash = null)
        {
            var memoryStream = stream as MemoryStream;
            if (memoryStream == null)
            {
                memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
            }

            var bytes = memoryStream.ToArray();
            return target.Read(bytes, hash);
        }
    }
}