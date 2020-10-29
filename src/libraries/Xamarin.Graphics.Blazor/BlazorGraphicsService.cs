using System.Collections.Generic;
using System.IO;
using Xamarin.Graphics;

namespace Elevenworks.Graphics.Blazor
{
    public class BlazorGraphicsService : IGraphicsService
    {
        public List<EWPath> ConvertToPaths(EWPath aPath, string text, ITextAttributes textual, float ppu, float zoom)
        {
            return new List<EWPath>();
        }

        public EWSize GetStringSize(string value, string fontName, float textSize)
        {
            return new EWSize(value.Length * 10, textSize + 2);
        }

        public EWSize GetStringSize(string value, string fontName, float textSize, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment)
        {
            return new EWSize(value.Length * 10, textSize + 2);
        }

        public void LayoutText(EWPath path, string text, ITextAttributes textAttributes, LayoutLine callback)
        {
            // Do nothing
        }
        
        public EWImage LoadImageFromStream(Stream stream, EWImageFormat format = EWImageFormat.Png)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (stream)
                {
                    stream.CopyTo(memoryStream);
                }

                return new VirtualImage(memoryStream.ToArray(), format);
            }
        }

        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return null;
        }

        public bool IsRetina => false;

        public string SystemFontName => "Arial";
        public string BoldSystemFontName => "Arial-Bold";
    }
}
