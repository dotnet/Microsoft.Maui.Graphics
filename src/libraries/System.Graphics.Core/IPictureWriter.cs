using System.IO;
using System.Threading.Tasks;

namespace System.Graphics
{
    public interface IPictureWriter
    {
        void Save(Picture picture, Stream stream);
        Task SaveAsync(Picture picture, Stream stream);
    }
}