using System.Windows.Forms.Integration;
using Elevenworks.Graphics.SharpDX.WindowsForms;
using Xamarin.Graphics;

namespace Elevenworks.Graphics.SharpDX.WPF
{
    public class WDGraphicsView : WindowsFormsHost
    {
        private readonly WFGraphicsView _graphicsView;

        public WDGraphicsView() : this(null, null)
        {
        }

        public WDGraphicsView(EWDrawable drawable = null, IGraphicsRenderer renderer = null)
        {
            _graphicsView = new WFGraphicsView(drawable, renderer);
            Child = _graphicsView;
        }

        public WFGraphicsView GraphicsView => _graphicsView;

        public IGraphicsRenderer Renderer
        {
            get => _graphicsView.Renderer;
            set => _graphicsView.Renderer = value;
        }

        public EWDrawable Drawable
        {
            get => _graphicsView.Drawable;
            set => _graphicsView.Drawable = value;
        }

        public EWColor BackgroundColor
        {
            get => _graphicsView.BackgroundColor;
            set => _graphicsView.BackgroundColor = value;
        }

        public void Invalidate()
        {
            if (!_graphicsView.Dirty)
            {
                _graphicsView.Dirty = true;
                Dispatcher.Invoke(_graphicsView.Refresh);
            }
        }
    }
}