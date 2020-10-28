using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Graphics
{
    public static class EWPictureReaderExtensions
    {
        public static EWPicture Read(this EWPictureReader target, Stream stream, string hash = null)
        {
            if (!(stream is MemoryStream memoryStream))
            {
                memoryStream = new MemoryStream();
                stream.CopyTo(memoryStream);
            }

            var bytes = memoryStream.ToArray();
            return target.Read(bytes, hash);
        }

        public static async Task<EWPicture> ReadAsync(this EWPictureReader target, Stream stream, string hash = null)
        {
            if (!(stream is MemoryStream memoryStream))
            {
                memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
            }

            var bytes = memoryStream.ToArray();
            return target.Read(bytes, hash);
        }
    }
}