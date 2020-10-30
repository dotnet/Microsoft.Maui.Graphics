using System.IO;
using Android.Graphics;

namespace System.Graphics.Android
{
    public class MDBitmapExportContext : BitmapExportContext
    {
        private Bitmap _bitmap;
        private Canvas _androidCanvas;
        private readonly ScalingCanvas _canvas;
        private readonly bool _disposeBitmap;

        public MDBitmapExportContext(int width, int height, float displayScale = 1, int dpi = 72, bool disposeBitmap = true, bool transparent = true) : base(width, height, dpi)
        {
            _bitmap = Bitmap.CreateBitmap(
                width,
                height,
                transparent
                    ? Bitmap.Config.Argb8888
                    : Bitmap.Config.Rgb565);

            _androidCanvas = new Canvas(_bitmap);
            var nativeCanvas = new MDCanvas(null)
            {
                Canvas = _androidCanvas,
            };
            _canvas = new ScalingCanvas(nativeCanvas)
            {
                DisplayScale = displayScale
            };
            _disposeBitmap = disposeBitmap;
        }

        public override ICanvas Canvas => _canvas;

        public override EWImage Image => new MDImage(Bitmap);

        public Bitmap Bitmap => _bitmap;

        public override void Dispose()
        {
            if (_androidCanvas != null)
            {
                _androidCanvas.Dispose();
                _androidCanvas = null;
            }

            if (_bitmap != null && _disposeBitmap)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }

            base.Dispose();
        }

        public override void WriteToStream(Stream stream)
        {
            _bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
        }
    }
}