using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Elevenworks.Threading;
using Microsoft.Graphics.Canvas;

namespace Elevenworks.Graphics.Win2D
{
    public class W2DGraphicsService : IGraphicsService
    {
        public static W2DGraphicsService Instance = new W2DGraphicsService();

        private static ICanvasResourceCreator _globalCreator;
        private static readonly ThreadLocal<ICanvasResourceCreator> _threadLocalCreator = new ThreadLocal<ICanvasResourceCreator>();

        public static ICanvasResourceCreator GlobalCreator
        {
            get => _globalCreator;
            set => _globalCreator = value;
        }

        public static ICanvasResourceCreator ThreadLocalCreator
        {
            set => _threadLocalCreator.Value = value;
        }

        private static ICanvasResourceCreator Creator
        {
            get
            {
                var value = _threadLocalCreator.Value;
                if (value == null)
                    return _globalCreator;

                return value;
            }

        }

        public string SystemFontName => "Arial";
        public string BoldSystemFontName => "Arial-Bold";

        public List<EWPath> ConvertToPaths(EWPath aPath, string text, ITextAttributes textAttributes, float ppu, float zoom)
        {
            return new List<EWPath>();
        }

        public EWSize GetStringSize(string value, string fontName, float textSize)
        {
            return new EWSize(value.Length * 10, textSize + 2);
        }

        public EWSize GetStringSize(string value, string fontName, float textSize, EWHorizontalAlignment horizontalAlignment, EWVerticalAlignment verticalAlignment)
        {
            return new EWSize(value.Length * 10, textSize + 2);
        }

        public void LayoutText(EWPath path, string text, ITextAttributes textAttributes, LayoutLine callback)
        {
            // Do nothing
        }

        public EWRectangle GetPathBounds(EWPath path)
        {
            return path.GetBoundsByFlattening(0.0005f);
        }

        public EWRectangle GetPathBoundsWhenRotated(EWImmutablePoint center, EWPath path, float angle)
        {
            var rotatedPath = path.Rotate(angle, center);
            return rotatedPath.GetBoundsByFlattening();
        }

        public bool PathContainsPoint(EWPath path, EWImmutablePoint point, float ppu, float zoom, float strokeWidth)
        {
            var flattened = path.GetFlattenedPath();
            return flattened.Contains(point);
        }

        public bool PointIsOnPath(EWPath path, EWImmutablePoint point, float ppu, float zoom, float strokeWidth)
        {
            throw new NotImplementedException();
        }

        public bool PointIsOnPathSegment(EWPath path, int segmentIndex, EWImmutablePoint point, float ppu, float zoom, float strokeWidth)
        {
            throw new NotImplementedException();
        }

        public EWImage LoadImageFromStream(Stream stream, string hash = null, EWImageFormat format = EWImageFormat.Png)
        {
            var creator = Creator;
            if (creator == null)
                throw new Exception("No resource creator has been registered globally or for this thread.");

            var bitmap = AsyncPump.Run(async () => await CanvasBitmap.LoadAsync(creator, stream.AsRandomAccessStream()));
            return new W2DImage(Creator, bitmap);
        }

        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale)
        {
            return null;
        }

        public bool IsRetina => false;
    }
}
