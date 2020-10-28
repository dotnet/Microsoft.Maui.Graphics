using System.IO;

namespace Elevenworks.Graphics
{
    public interface IPdfPictureService
    {
        EWPicture CreatePicture(EWPdfPage pdfPage);
        EWPicture CreatePicture(Stream stream);
    }
}