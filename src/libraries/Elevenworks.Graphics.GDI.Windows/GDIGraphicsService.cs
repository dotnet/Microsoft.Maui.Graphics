using System.Collections.Generic;
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

        public List<EWPath> ConvertToPaths(EWPath path, string text, ITextAttributes textAttributes, float ppu, float zoom)
        {
            Logger.Debug("Not implemented");
            return new List<EWPath>();
        }

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

        public void LayoutText(EWPath path, string text, ITextAttributes attributes, LayoutLine callback)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            var lineSpacing = attributes.FontSize * .5f;
            var lineSizes = new List<EWSize>();
            var blockHeight = 0f;

            var bounds = path.Bounds;
            var point = new EWPoint(bounds.Point1);

            var lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];

                if (i > 0)
                    blockHeight += lineSpacing;

                var size = GetStringSize(line, attributes.FontName, attributes.FontSize);
                lineSizes.Add(size);

                blockHeight += size.Height;
            }

            switch (attributes.VerticalAlignment)
            {
                case EWVerticalAlignment.CENTER:
                    point.Y = bounds.MinY + (bounds.Height - blockHeight) / 2;
                    break;
                case EWVerticalAlignment.BOTTOM:
                    point.X = bounds.MaxY - attributes.Margin - blockHeight;
                    break;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var size = lineSizes[i];
                point.Y += attributes.FontSize;

                switch (attributes.HorizontalAlignment)
                {
                    case EWHorizontalAlignment.CENTER:
                        point.X = bounds.MinX + (bounds.Width - size.Width) / 2;
                        break;
                    case EWHorizontalAlignment.RIGHT:
                        point.X = bounds.MaxX - attributes.Margin - size.Width;
                        break;
                    default:
                        point.X = bounds.MinX + attributes.Margin;
                        break;
                }

                callback(point, attributes, line, 0, 0, 0);

                // Line spacing
                point.Y += lineSpacing;
            }
        }

        public EWRectangle GetPathBounds(EWPath aPath)
        {
            //Note: Can't use the GDI method, as it returns the bounds including the handles.
            /*
            var graphicsPath = aPath.NativePath as GraphicsPath;
            if (graphicsPath == null)
            {
                graphicsPath = aPath.AsGDIPath(1);
                aPath.NativePath = graphicsPath;
            }

            var bounds = graphicsPath.GetBounds();
            return bounds.AsEWRectangle();*/

            return aPath.GetBoundsByFlattening();
        }

        public EWRectangle GetPathBoundsWhenRotated(EWImmutablePoint aCenter, EWPath aPath, float aAngle)
        {
            var rotated = aPath.Rotate(aAngle, aCenter);
            return rotated.GetBoundsByFlattening();
        }

        public bool PathContainsPoint(EWPath aPath, EWImmutablePoint aPoint, float ppu, float aZoom, float aStrokeWidth)
        {
            //TODO: Make this faster.
            var flattened = aPath.GetFlattenedPath();
            return Geometry.AlreadyFlattenedPathContains(flattened, aPoint);
        }

        public bool PointIsOnPath(EWPath aPath, EWImmutablePoint aPoint, float ppu, float aZoom, float aStrokeWidth)
        {
            Logger.Debug("Not implemented");
            return false;
        }

        public bool PointIsOnPathSegment(EWPath aPath, int aIndex, EWImmutablePoint aPoint, float ppu, float aZoom, float aStrokeWidth)
        {
            Logger.Debug("Not implemented");
            return false;
        }

        public EWImage LoadImageFromStream(Stream stream, string hash = null, EWImageFormat format = EWImageFormat.Png)
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