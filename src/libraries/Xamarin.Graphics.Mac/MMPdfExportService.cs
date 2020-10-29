namespace Xamarin.Graphics.Mac
{
    public class MMPdfExportService : IPdfExportService
    {
        public static MMPdfExportService Instance = new MMPdfExportService();

        private MMPdfExportService()
        {
        }

        public PdfExportContext CreateContext(float width = -1, float height = -1)
        {
            return new MMPdfExportContext(width, height);
        }
    }
}