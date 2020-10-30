using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Microsoft.Graphics.Canvas;

namespace System.Graphics.Win2D
{
    internal class W2DImage : EWImage
    {
        private readonly ICanvasResourceCreator _creator;
        private CanvasBitmap _bitmap;

        public W2DImage(ICanvasResourceCreator creator, CanvasBitmap bitmap)
        {
            _creator = creator;
            _bitmap = bitmap;
        }

        public CanvasBitmap NativeImage => _bitmap;

        public void Dispose()
        {
            var bitmap = Interlocked.Exchange(ref _bitmap, null);
            bitmap?.Dispose();
        }

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            if (Width > maxWidthOrHeight || Height > maxWidthOrHeight)
            {
                using (var memoryStream = new InMemoryRandomAccessStream())
                {
                    Save(memoryStream.AsStreamForWrite());
                    memoryStream.Seek(0);

                    // ReSharper disable once AccessToDisposedClosure
                    var newBitmap = AsyncPump.Run(async () => await CanvasBitmap.LoadAsync(_creator, memoryStream, 96));
                    using (var memoryStream2 = new InMemoryRandomAccessStream())
                    {
                        // ReSharper disable once AccessToDisposedClosure
                        AsyncPump.Run(async () => await newBitmap.SaveAsync(memoryStream2, CanvasBitmapFileFormat.Png));
                        
                        memoryStream2.Seek(0);
                        var newImage = W2DGraphicsService.Instance.LoadImageFromStream(memoryStream2.AsStreamForRead());
                        if (disposeOriginal)
                            _bitmap.Dispose();

                        return newImage;
                    }
                }
            }

            return this;
        }

        public EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit,
            bool disposeOriginal = false)
        {
            throw new NotImplementedException();
        }

        public float Width => (float)_bitmap.Size.Width;

        public float Height => (float)_bitmap.Size.Height;

        public void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case EWImageFormat.Jpeg:
                    AsyncPump.Run(async () => await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Jpeg, quality));
                    break;
                default:
                    AsyncPump.Run(async () => await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Png));
                    break;
            }
        }

        public async Task SaveAsync(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1)
        {
            switch (format)
            {
                case EWImageFormat.Jpeg:
                    await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Jpeg, quality);
                    break;
                default:
                    await _bitmap.SaveAsync(stream.AsRandomAccessStream(), CanvasBitmapFileFormat.Png);
                    break;
            }
        }

        public void Draw(ICanvas canvas, EWRectangle dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.MinX, dirtyRect.MinY, Math.Abs(dirtyRect.Width), Math.Abs(dirtyRect.Height));
        }
    }
}