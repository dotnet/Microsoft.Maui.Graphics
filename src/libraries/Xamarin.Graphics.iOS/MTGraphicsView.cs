using System;
using System.Drawing;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Xamarin.Graphics.iOS
{
    [Register("MTGraphicsView")]
    public class MTGraphicsView : UIView
    {
        private MTGraphicsRenderer _renderer;
        private CGColorSpace _colorSpace;
        private EWDrawable _drawable;
        private bool _inPanOrZoom;
        private CGRect _lastBounds;

        public MTGraphicsView(RectangleF frame, EWDrawable drawable = null, MTGraphicsRenderer renderer = null) : base(frame)
        {
            Drawable = drawable;
            Renderer = renderer;
            BackgroundColor = UIColor.White;
        }

        public MTGraphicsView(EWDrawable drawable = null, MTGraphicsRenderer renderer = null)
        {
            Drawable = drawable;
            Renderer = renderer;
            BackgroundColor = UIColor.White;
        }

        public MTGraphicsView(IntPtr aPtr) : base(aPtr)
        {
            BackgroundColor = UIColor.White;
        }

        public bool InPanOrZoom
        {
            get => _inPanOrZoom;
            set => _inPanOrZoom = value;
        }

        public MTGraphicsRenderer Renderer
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

                _renderer = value ?? new MTDirectRenderer();

                _renderer.GraphicsView = this;
                _renderer.Drawable = _drawable;
                var bounds = Bounds;
                _renderer.SizeChanged((float) bounds.Width, (float) bounds.Height);
            }
        }

        public EWDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                if (_renderer != null)
                {
                    _renderer.Drawable = _drawable;
                    _renderer.Invalidate();
                }
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

        public override void WillMoveToSuperview(UIView newSuperview)
        {
            base.WillMoveToSuperview(newSuperview);

            if (newSuperview == null)
            {
                _renderer.Detached();
            }
        }

        public override void Draw(CGRect dirtyRect)
        {
            base.Draw(dirtyRect);

            if (_drawable == null) return;

            var coreGraphics = UIGraphics.GetCurrentContext();

            if (_colorSpace == null)
            {
                _colorSpace = CGColorSpace.CreateDeviceRGB();
            }

            coreGraphics.SetFillColorSpace(_colorSpace);
            coreGraphics.SetStrokeColorSpace(_colorSpace);
            coreGraphics.SetPatternPhase(PatternPhase);

            _renderer.Draw(coreGraphics, dirtyRect.AsEWRectangle(), _inPanOrZoom);
        }

        public override CGRect Bounds
        {
            get => base.Bounds;

            set
            {
                var newBounds = value;
                if (_lastBounds.Width != newBounds.Width || _lastBounds.Height != newBounds.Height)
                {
                    base.Bounds = value;
                    _renderer.SizeChanged((float) newBounds.Width, (float) newBounds.Height);
                    _renderer.Invalidate();

                    _lastBounds = newBounds;
                }
            }
        }

        protected virtual CGSize PatternPhase
        {
            get
            {
                var px = Frame.X;
                var py = Frame.Y;
                return new CGSize(px, py);
            }
        }
    }
}