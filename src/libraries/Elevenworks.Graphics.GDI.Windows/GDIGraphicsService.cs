using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Elevenworks.Graphics
{
    public class GDIGraphicsService : IGraphicsService
    {
        public static GDIGraphicsService Instance = new GDIGraphicsService();

        public string SystemFontName => "Arial";
        public string BoldSystemFontName => "Arial-Bold";
        
        public EWSize GetStringSize(string aString, string aFontName, float fontSize)
        {
            var fontEntry = GDIFontManager.GetMapping(aFontName);
            var font = new Font(fontEntry.Name, fontSize * .75f);
            var size = TextRenderer.MeasureText(aString, font);
            font.Dispose();
            return new EWSize(size.Width, size.Height);
        }


        public EWSize GetStringSize(string aString, string aFontName, float fontSize, EWHorizontalAlignment aHorizontalAlignment, EWVerticalAlignment aVerticalAlignment)
        {
            var fontEntry = GDIFontManager.GetMapping(aFontName);
            var font = new Font(fontEntry.Name, fontSize * .75f);
            var size = TextRenderer.MeasureText(aString, font);
            font.Dispose();
            return new EWSize(size.Width, size.Height);
        }

        public EWImage LoadImageFromStream(Stream stream, EWImageFormat format = EWImageFormat.Png)
        {
            var bitmap = new Bitmap(stream);
            return new GDIImage(bitmap);
        }

        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return new GDIBitmapExportContext(width, height, displayScale);
        }

        public bool IsRetina => false;
    }
}