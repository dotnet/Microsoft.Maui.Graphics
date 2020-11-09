using System.Windows.Forms;
using SharpDX.Desktop;

namespace System.Graphics.SharpDX.WindowsForms
{
    public class WFGraphicsView : RenderControl
    {
        private readonly EWRectangle _dirtyRect = new EWRectangle();
        private IGraphicsRenderer _renderer;
        private EWDrawable _drawable;

        public WFGraphicsView(EWDrawable drawable = null, IGraphicsRenderer renderer = null)
        {
            Drawable = drawable;
            Renderer = renderer;
        }

        public bool Dirty
        {
            get => _renderer.Dirty;
            set => _renderer.Dirty = value;
        }

        public Color BackgroundColor
        {
            get => _renderer.BackgroundColor;
            set => _renderer.BackgroundColor = value;
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

                _renderer = value ?? new WFDirectGraphicsRenderer();

                _renderer.GraphicsView = this;
                _renderer.Drawable = _drawable;
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
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_drawable == null) return;

            var clipRect = e.ClipRectangle;
            _dirtyRect.X1 = clipRect.X;
            _dirtyRect.Y1 = clipRect.Y;
            _dirtyRect.Width = clipRect.Width;
            _dirtyRect.Height = clipRect.Height;

            _renderer.Draw(_dirtyRect);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            _renderer.SizeChanged(Width, Height);
        }
    }
}