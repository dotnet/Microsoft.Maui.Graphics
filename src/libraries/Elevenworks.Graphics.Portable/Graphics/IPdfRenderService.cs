using System.IO;

namespace Elevenworks.Graphics
{
    public interface IPdfRenderService
    {
        EWPdfPage CreatePage(Stream stream, int pageNumber = -1);
    }
}