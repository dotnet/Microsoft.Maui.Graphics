﻿using System;
using Foundation;
using CoreGraphics;
using System.IO;
using AppKit;

namespace Elevenworks.Graphics
{
    public class MMPdfExportContext : PdfExportContext
    {
        private NSMutableDictionary _documentInfo;
        private NSMutableData _data;
        private CGContextPDF _context;
        private readonly CGCanvas _canvas;
        private bool _closed;
        private bool _pageOpen;

        public MMPdfExportContext(
            float defaultWidth,
            float defaultHeight) : base(defaultWidth, defaultHeight)
        {
            _documentInfo = new NSMutableDictionary();
            _canvas = new CGCanvas(() => CGColorSpace.CreateDeviceRGB());
        }

        protected override void AddPageImpl(float width, float height)
        {
            if (_closed)
                throw new Exception("Unable to add a page because the PDFContext is already closed.");

            if (_data == null)
            {
                _data = new NSMutableData();
                var consumer = new CGDataConsumer(_data);
                _context = new CGContextPDF(consumer, CGRect.Empty, null);
                _context.SetFillColorSpace(CGColorSpace.CreateDeviceRGB());
                _context.SetStrokeColorSpace(CGColorSpace.CreateDeviceRGB());
            }

            if (_pageOpen)
                _context.EndPage();

            _context.BeginPage(new CGRect(0, 0, width, height));
            _context.TranslateCTM(0, height);
            _context.ScaleCTM(1, -1);
            _context.SetLineWidth(1);
            _context.SetFillColor(new CGColor(1, 1));
            _context.SetStrokeColor(new CGColor(0, 1));

            _pageOpen = true;

            _canvas.Context = _context;
        }

        public override void WriteToStream(Stream stream)
        {
            Close();

            if (_data != null)
            {
                using (var inputStream = _data.AsStream())
                {
                    inputStream.CopyTo(stream);
                }
            }
        }

        public NSData Data
        {
            get
            {
                Close();
                return _data;
            }
        }

        public override EWCanvas Canvas => _canvas;

        private void Close()
        {
            if (!_closed)
            {
                try
                {
                    if (_pageOpen)
                        _context.EndPage();

                    _context.Close();
                }
                catch (Exception exc)
                {
                    Logger.Warn(exc);
                }
                finally
                {
                    _closed = true;
                }
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            Close();

            try
            {
                _canvas?.Dispose();
                _context?.Dispose();
            }
            catch (Exception exc)
            {
                Logger.Warn(exc);
            }
        }
    }
}