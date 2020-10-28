using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Elevenworks.Threading;
using Microsoft.Graphics.Canvas;

namespace Elevenworks.Graphics.Win2D
{
    internal class W2DImage : EWImage
    {
        private readonly ICanvasResourceCreator _creator;
        private CanvasBitmap _bitmap;
        private string _hash;

        public W2DImage(ICanvasResourceCreator creator, CanvasBitmap bitmap, string hash = null)
        {
            _creator = creator;
            _bitmap = bitmap;
            _hash = hash;
        }

        public CanvasBitmap NativeImage => _bitmap;

        public void Dispose()
        {
            var disp = Interlocked.Exchange(ref _bitmap, null);
            disp?.Dispose();
        }

        public EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
        {
            if (Width > maxWidthOrHeight || Height > maxWidthOrHeight)
            {
                /*float factor;

                if (Width > Height)
                {
                    factor = maxWidthOrHeight / Width;
                }
                else
                {
                    factor = maxWidthOrHeight / Height;
                }*/

                //var w = (int)Math.Round(factor * Width);
                //var h = (int)Math.Round(factor * Height);

                // todo: check to see if this actually works.  It appears it was never finished.
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

        public string Hash
        {
            get
            {
                if (_hash == null)
                {
                    string thehash = this.CreateHash();
                    return Interlocked.Exchange(ref _hash, thehash);
                }

                return _hash;
            }

            set => _hash = value;
        }

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

        public void Draw(EWCanvas canvas, EWRectangle dirtyRect)
        {
            canvas.DrawImage(this, dirtyRect.MinX, dirtyRect.MinY, Math.Abs(dirtyRect.Width), Math.Abs(dirtyRect.Height));
        }
    }
}