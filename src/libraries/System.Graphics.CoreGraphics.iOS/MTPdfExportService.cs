namespace System.Graphics.CoreGraphics
{
    public class MTPdfExportService : IPdfExportService
    {
        public static MTPdfExportService Instance = new MTPdfExportService();

        private MTPdfExportService()
        {
        }

        public PdfExportContext CreateContext(float width = -1, float height = -1)
        {
            return new MTPdfExportContext(width, height);
        }
    }
}