using System.IO;

namespace System.Graphics
{
    public interface IPdfRenderService
    {
        EWPdfPage CreatePage(Stream stream, int pageNumber = -1);
    }
}