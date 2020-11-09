using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

namespace System.Graphics.Android
{
    public class MDGraphicsView : View
    {
        private readonly EWRectangle _dirtyRect = new EWRectangle();
        private MDGraphicsRenderer _renderer;
        private EWDrawable _drawable;
        private int _width, _height;
        private bool _inPanOrZoom;

        public MDGraphicsView(Context context, IAttributeSet attrs, EWDrawable drawable = null, MDGraphicsRenderer renderer = null) : base(context, attrs)
        {
            Drawable = drawable;
            Renderer = renderer;
            SetLayerType(LayerType.Software, null);
        }

        public MDGraphicsView(Context context, EWDrawable drawable = null, MDGraphicsRenderer renderer = null) : base(context)
        {
            Drawable = drawable;
            Renderer = renderer;
            SetLayerType(LayerType.Software, null);
        }

        public bool InPanOrZoom
        {
            get => _inPanOrZoom;
            set => _inPanOrZoom = value;
        }

        public MDGraphicsRenderer Renderer
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

                _renderer = value;

                if (_renderer == null)
                {
                    _renderer = new MDDirectRenderer(Context);
                }

                _renderer.GraphicsView = this;
                _renderer.Drawable = _drawable;
                _renderer.SizeChanged(_width, _height);
            }
        }

        public Color BackgroundColor
        {
            get => _renderer.BackgroundColor;
            set => _renderer.BackgroundColor = value;
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

        public override void Draw(Canvas androidCanvas)
        {
            if (_drawable == null) return;

            _dirtyRect.Width = Width;
            _dirtyRect.Height = Height;
            _renderer.Draw(androidCanvas, _dirtyRect, _inPanOrZoom);
        }

        protected override void OnSizeChanged(int width, int height, int oldWidth, int oldHeight)
        {
            base.OnSizeChanged(width, height, oldWidth, oldHeight);
            _renderer.SizeChanged(width, height);
            _width = width;
            _height = height;
        }

        protected override void OnDetachedFromWindow()
        {
            _renderer.Detached();
        }

        public void InvalidateDrawable()
        {
            _renderer.Invalidate();
        }

        public void InvalidateDrawable(float x, float y, float w, float h)
        {
            _renderer.Invalidate(x, y, w, h);
        }
    }
}