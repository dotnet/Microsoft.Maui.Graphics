using System.IO;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public delegate void LayoutLine(EWPoint aPoint, ITextAttributes aTextual, string aText, float aAscent, float aDescent, float aLeading);

    public interface IGraphicsService
    {
        string SystemFontName { get; }
        string BoldSystemFontName { get; }
        
        EWSize GetStringSize(string value, string fontName, float textSize);
        EWSize GetStringSize(string value, string fontName, float textSize, EWHorizontalAlignment horizontalAlignment, EWVerticalAlignment verticalAlignment);
        
        EWImage LoadImageFromStream(Stream stream, EWImageFormat format = EWImageFormat.Png);
        BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1);

        bool IsRetina { get; }
    }

    public static class GraphicServiceExtensions
    {
        public static EWImage LoadImageFromBytes(this IGraphicsService target, byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return target.LoadImageFromStream(stream);
            }
        }
    }
}