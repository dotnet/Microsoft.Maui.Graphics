using System;
using System.IO;
using System.Threading.Tasks;

namespace Elevenworks.Graphics
{
    public interface EWPdfPage : EWDrawable, IDisposable, IHasheable
    {
        float Width { get; }
        float Height { get; }
        int PageNumber { get; }

        void Save(Stream stream);
        Task SaveAsync(Stream stream);
    }
}