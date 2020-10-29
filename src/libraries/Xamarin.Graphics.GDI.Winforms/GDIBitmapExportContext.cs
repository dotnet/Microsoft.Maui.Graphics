﻿using System.IO;
using System.Drawing;
using Xamarin.Graphics;
using Xamarin.Graphics.GDI;

namespace Elevenworks.Graphics
{
    public class GDIBitmapExportContext : BitmapExportContext
    {
        private readonly bool _disposeBitmap;
        private Bitmap _bitmap;
        private GDICanvas _canvas;

        public GDIBitmapExportContext(int width, int height, float displayScale = 1, int dpi = 72, bool disposeBitmap = true) : base(width, height, dpi)
        {
            _disposeBitmap = disposeBitmap;
            _bitmap = new Bitmap(width, height);

            _canvas = new GDICanvas
            {
                Graphics = System.Drawing.Graphics.FromImage(_bitmap),
                DisplayScale = displayScale
            };
        }

        public override EWCanvas Canvas => _canvas;

        public override void Dispose()
        {
            _canvas = null;

            if (_bitmap != null && _disposeBitmap)
            {
                _bitmap.Dispose();
                _bitmap = null;
            }

            base.Dispose();
        }

        public override EWImage Image => new GDIImage(_bitmap);

        public override void WriteToStream(Stream stream)
        {
            Image.Save(stream, EWImageFormat.Png);
        }
    }
}