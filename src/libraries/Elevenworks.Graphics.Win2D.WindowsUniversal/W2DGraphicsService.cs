using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Graphics.Canvas;
using Xamarin.Graphics;

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

        public EWSize GetStringSize(string value, string fontName, float textSize, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment)
        {
            return new EWSize(value.Length * 10, textSize + 2);
        }

        public EWImage LoadImageFromStream(Stream stream, EWImageFormat format = EWImageFormat.Png)
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
