using System.IO;
using System.Threading.Tasks;

namespace System.Graphics
{
    public interface EWPdfPage : EWDrawable, IDisposable
    {
        float Width { get; }
        float Height { get; }
        int PageNumber { get; }

        void Save(Stream stream);
        Task SaveAsync(Stream stream);
    }
}