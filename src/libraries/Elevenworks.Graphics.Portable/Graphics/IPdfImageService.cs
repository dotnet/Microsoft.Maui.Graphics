using System.IO;

namespace Elevenworks.Graphics
{
    public interface IPdfImageService
    {
        EWImage CreateImage(EWPdfPage page, int width = -1, int height = -1);
        EWImage CreateImage(Stream stream, int width = -1, int height = -1, int pageNumber = -1);
    }
}