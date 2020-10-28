using System.IO;
using System.Threading.Tasks;

namespace Elevenworks.Graphics
{
    public interface EWPictureWriter
    {
        void Save(EWPicture picture, Stream stream);
        Task SaveAsync(EWPicture picture, Stream stream);
    }
}