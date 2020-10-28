using System.IO;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public interface IPdfExportService
    {
        PdfExportContext CreateContext(float width = -1, float height = -1);
    }
}