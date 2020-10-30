using AppKit;
using CoreGraphics;

namespace System.Graphics.CoreGraphics
{
    public class MMDirectRenderer : MMGraphicsRenderer
    {
        private readonly CGCanvas _canvas;
        private EWDrawable _drawable;
        private MMGraphicsView _graphicsView;

        public MMDirectRenderer()
        {
            var colorspace = NSColorSpace.DeviceRGBColorSpace.ColorSpace;
            _canvas = new CGCanvas(() => colorspace);
        }

        public EWCanvas Canvas => _canvas;

        public EWDrawable Drawable
        {
            get => _drawable;
            set => _drawable = value;
        }

        public MMGraphicsView GraphicsView
        {
            set => _graphicsView = value;
        }

        public void Draw(CGContext coreGraphics, EWRectangle dirtyRect, bool inPanOrZoom)
        {
            _canvas.Context = coreGraphics;

            try
            {
                _drawable.Draw(_canvas, dirtyRect);
            }
            catch (Exception exc)
            {
                Logger.Error("An unexpected error occurred rendering the drawing.", exc);
            }
            finally
            {
                _canvas.Context = null;
            }
        }

        public void SizeChanged(float width, float height)
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
            _graphicsView?.SetNeedsDisplayInRect(_graphicsView.Bounds);
        }

        public void Invalidate(float x, float y, float w, float h)
        {
            _graphicsView?.SetNeedsDisplayInRect(new CGRect(x, y, w, h));
        }
    }
}