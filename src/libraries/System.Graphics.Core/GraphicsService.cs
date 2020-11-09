using System.IO;

namespace System.Graphics
{
    public delegate void LayoutLine(EWPoint aPoint, ITextAttributes aTextual, string aText, float aAscent, float aDescent, float aLeading);

    public interface IGraphicsService
    {
        string SystemFontName { get; }
        string BoldSystemFontName { get; }
        
        EWSize GetStringSize(string value, string fontName, float textSize);
        EWSize GetStringSize(string value, string fontName, float textSize, HorizontalAlignment horizontalAlignment, VerticalAlignment verticalAlignment);
        
        IImage LoadImageFromStream(Stream stream, ImageFormat format = ImageFormat.Png);
        BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1);

        bool IsRetina { get; }
    }

    public static class GraphicServiceExtensions
    {
        public static IImage LoadImageFromBytes(this IGraphicsService target, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return target.LoadImageFromStream(stream);
            }
        }
    }
}