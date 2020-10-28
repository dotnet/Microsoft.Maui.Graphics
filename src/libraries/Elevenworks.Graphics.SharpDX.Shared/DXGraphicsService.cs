﻿using System.IO;
using System.Threading;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using Factory = SharpDX.Direct2D1.Factory;
using FactoryType = SharpDX.Direct2D1.FactoryType;

namespace Elevenworks.Graphics.SharpDX
{
    public class DXGraphicsService : IGraphicsService
    {
        private const float Dip = 96f / 72f;

        public static Factory SharedFactory = new Factory(FactoryType.MultiThreaded);

        public static readonly ThreadLocal<Factory> CurrentFactory = new ThreadLocal<Factory>();
        public static readonly ThreadLocal<RenderTarget> CurrentTarget = new ThreadLocal<RenderTarget>();
        public static ImagingFactory2 FactoryImaging = new ImagingFactory2();

        public static global::SharpDX.DirectWrite.Factory FactoryDirectWrite = new global::SharpDX.DirectWrite.Factory(global::SharpDX.DirectWrite.FactoryType.Shared);

        public static readonly DXGraphicsService Instance = new DXGraphicsService();

        private DXGraphicsService()
        {
        }

        public bool IsRetina => false;

        public string SystemFontName => "Arial";
        public string BoldSystemFontName => "Arial-Bold";

        public EWSize GetStringSize(string value, string fontName, float textSize)
        {
            if (value == null) return new EWSize();

            float fontSize = textSize;
            float factor = 1;
            while (fontSize > 14)
            {
                fontSize /= 14;
                factor *= 14;
            }

            if (fontName == null)
                fontName = "Arial";

            var size = new EWSize();

            var textFormat = new TextFormat(FactoryDirectWrite, fontName, fontSize);
            textFormat.TextAlignment = TextAlignment.Leading;
            textFormat.ParagraphAlignment = ParagraphAlignment.Near;

            var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512, 512);
            size.Width = textLayout.Metrics.Width;
            size.Height = textLayout.Metrics.Height;

            size.Width *= factor;
            size.Height *= factor;

            return size;
        }

        public EWSize GetStringSize(
            string value,
            string fontName,
            float textSize,
            EwHorizontalAlignment horizontalAlignment,
            EwVerticalAlignment verticalAlignment)
        {
            if (value == null) return new EWSize();

            float fontSize = textSize;
            float factor = 1;
            while (fontSize > 14)
            {
                fontSize /= 14;
                factor *= 14;
            }

            var size = new EWSize();

            var textFormat = new TextFormat(FactoryDirectWrite, SystemFontName, FontWeight.Regular, FontStyle.Normal,
                fontSize);
            if (horizontalAlignment == EwHorizontalAlignment.Left)
            {
                textFormat.TextAlignment = TextAlignment.Leading;
            }
            else if (horizontalAlignment == EwHorizontalAlignment.Center)
            {
                textFormat.TextAlignment = TextAlignment.Center;
            }
            else if (horizontalAlignment == EwHorizontalAlignment.Right)
            {
                textFormat.TextAlignment = TextAlignment.Trailing;
            }
            else if (horizontalAlignment == EwHorizontalAlignment.Justified)
            {
                textFormat.TextAlignment = TextAlignment.Justified;
            }

            if (verticalAlignment == EwVerticalAlignment.Top)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Near;
            }
            else if (verticalAlignment == EwVerticalAlignment.Center)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Center;
            }
            else if (verticalAlignment == EwVerticalAlignment.Bottom)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Far;
            }

            var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512f, 512f, Dip, false);
            size.Width = textLayout.Metrics.Width;
            size.Height = textLayout.Metrics.Height;


            size.Width *= factor;
            size.Height *= factor;

            return size;
        }

        public EWImage LoadImageFromStream(Stream stream, EWImageFormat format = EWImageFormat.Png)
        {
            var bitmap = CurrentTarget.Value.LoadBitmap(stream);
            return new DXImage(bitmap);
        }

        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return new DXBitmapExportContext(width, height, displayScale, 72, false);
        }
    }
}