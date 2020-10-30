using Android.Content;
using Android.Graphics;

namespace System.Graphics.Android
{
    public class MDDirectRenderer : MDGraphicsRenderer
    {
        private readonly MDCanvas _canvas;
        private readonly ScalingCanvas _scalingCanvas;
        private EWDrawable _drawable;
        private MDGraphicsView _graphicsView;
        private EWColor _backgroundColor;

        public MDDirectRenderer(Context context)
        {
            _canvas = new MDCanvas(context);
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

        public MDGraphicsView GraphicsView
        {
            set => _graphicsView = value;
        }

        public EWColor BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public void Draw(Canvas androidCanvas, EWRectangle dirtyRect, bool inPanOrZoom)
        {
            _canvas.Canvas = androidCanvas;

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

        public void SizeChanged(int width, int height)
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

        public void Invalidate(float x, float y, float w, float h)
        {
            _graphicsView?.Invalidate();
        }
    }
}