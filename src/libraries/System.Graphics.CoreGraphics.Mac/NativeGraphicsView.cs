using AppKit;
using CoreGraphics;
using Foundation;

namespace System.Graphics.CoreGraphics
{
    [Register("NativeGraphicsView")]
    public class NativeGraphicsView : NSView
    {
        private IGraphicsRenderer _renderer;
        private CGColorSpace _colorSpace;
        private IDrawable _drawable;
        private bool _inPanOrZoom;
        private CGRect _lastBounds;
        private Color _backgroundColor;

        public NativeGraphicsView(IDrawable drawable = null, IGraphicsRenderer renderer = null)
        {
            Drawable = drawable;
            Renderer = renderer;
        }

        public NativeGraphicsView(IntPtr handle) : base(handle)
        {
        }

        public bool InPanOrZoom
        {
            get => _inPanOrZoom;
            set => _inPanOrZoom = value;
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public IGraphicsRenderer Renderer
        {
            get => _renderer;

            set
            {
                if (_renderer != null)
                {
                    _renderer.Drawable = null;
                    _renderer.GraphicsView = null;
                    _renderer.Dispose();
                }

                _renderer = value ?? new DirectRenderer();

                _renderer.GraphicsView = this;
                _renderer.Drawable = _drawable;
                var bounds = Bounds;
                _renderer.SizeChanged((float) bounds.Width, (float) bounds.Height);
            }
        }

        public IDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                if (_renderer != null)
                {
                    _renderer.Drawable = _drawable;
                }
            }
        }

        public override void ViewWillMoveToSuperview(NSView newSuperview)
        {
            base.ViewWillMoveToSuperview(newSuperview);

            if (newSuperview == null)
            {
                _renderer.Detached();
            }
        }

        public void InvalidateDrawable()
        {
            _renderer.Invalidate();
        }

        public void InvalidateDrawable(float x, float y, float w, float h)
        {
            _renderer.Invalidate(x, y, w, h);
        }

        public override bool IsFlipped => true;

        public override void DrawRect(CGRect dirtyRect)
        {
            if (_drawable == null) return;

            var nsGraphicsContext = NSGraphicsContext.CurrentContext;
            var coreGraphics = nsGraphicsContext.GraphicsPort;

            if (_colorSpace == null)
                _colorSpace = NSColorSpace.DeviceRGBColorSpace.ColorSpace;

            coreGraphics.SetFillColorSpace(_colorSpace);
            coreGraphics.SetStrokeColorSpace(_colorSpace);
            coreGraphics.SetPatternPhase(PatternPhase);

            if (_backgroundColor != null)
            {
                nsGraphicsContext.GraphicsPort.SetFillColor(_backgroundColor.AsCGColor());
                nsGraphicsContext.GraphicsPort.FillRect(dirtyRect);
            }

            _renderer.Draw(nsGraphicsContext.GraphicsPort, dirtyRect.AsRectangleF(), _inPanOrZoom);
        }

        protected virtual CGSize PatternPhase
        {
            get
            {
                var px = Frame.X;
                var py = Frame.Height + Frame.Y;
                return new CGSize(px, py);
            }
        }

        public override void ViewWillDraw()
        {
            var newBounds = Bounds;
            if (_lastBounds.Width != newBounds.Width || _lastBounds.Height != newBounds.Height)
            {
                _renderer.SizeChanged((float) newBounds.Width, (float) newBounds.Height);
                _renderer.Invalidate();

                _lastBounds = newBounds;
            }
        }
    }
}