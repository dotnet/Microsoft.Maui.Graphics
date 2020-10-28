using System.IO;

namespace Xamarin.Graphics
{
    public interface IPdfRenderService
    {
        EWPdfPage CreatePage(Stream stream, int pageNumber = -1);
    }
}