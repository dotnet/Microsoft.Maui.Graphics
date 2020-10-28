using System;
using SkiaSharp;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public class WDSkiaDirectRenderer : ISkiaGraphicsRenderer
    {
        private readonly SkiaCanvas _canvas;
        private readonly ScalingCanvas _scalingCanvas;
        private EWDrawable _drawable;
        private WDSkiaGraphicsView _graphicsView;
        private EWColor _backgroundColor;

        public WDSkiaDirectRenderer()
        {
            _canvas = new SkiaCanvas();
            _scalingCanvas = new ScalingCanvas(_canvas);
        }

        public EWCanvas Canvas => _scalingCanvas;

        public EWDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                Invalidate();
            }
        }

        public WDSkiaGraphicsView GraphicsView
        {
            set => _graphicsView = value;
        }

        public EWColor BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public void Draw(
            SKCanvas skiaCanvas,
            EWRectangle dirtyRect)
        {
            _canvas.Canvas = skiaCanvas;

            try
            {
                if (_backgroundColor != null)
                {
                    _canvas.FillColor = _backgroundColor;
                    _canvas.FillRectangle(dirtyRect);
                    _canvas.FillColor = StandardColors.White;
                }

                _drawable.Draw(_scalingCanvas, dirtyRect);
            }
            catch (Exception exc)
            {
                Logger.Error("An unexpected error occurred rendering the drawing.", exc);
            }
            finally
            {
                _canvas.Canvas = null;
                _scalingCanvas.ResetState();
            }
        }

        public void SizeChanged(
            int width,
            int height)
        {
            // Do nothing
        }

        public void Detached()
        {
            // Do nothing
        }

        public void Dispose()
        {
            // Do nothing
        }

        public void Invalidate()
        {
            _graphicsView?.Invalidate();
        }

        public void Invalidate(
            float x,
            float y,
            float w,
            float h)
        {
            _graphicsView?.Invalidate();
        }
    }
}