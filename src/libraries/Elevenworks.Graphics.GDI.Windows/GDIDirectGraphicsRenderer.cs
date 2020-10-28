using System;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public class GDIDirectGraphicsRenderer : GDIGraphicsRenderer
    {
        private readonly GDICanvas _canvas = new GDICanvas();
        private EWDrawable _drawable;
        private EWColor _backgroundColor;
        private GDIGraphicsView _owner;
        private bool _dirty;

        public GDIGraphicsView GraphicsView
        {
            set => _owner = value;
        }

        public bool Dirty
        {
            get => _dirty;
            set => _dirty = value;
        }

        public EWCanvas Canvas => _canvas;

        public EWDrawable Drawable
        {
            get => _drawable;
            set
            {
                _drawable = value;
                Invalidate();
            }
        }

        public EWColor BackgroundColor
        {
            get => _backgroundColor;
            set => _backgroundColor = value;
        }

        public void Draw(System.Drawing.Graphics graphics, EWRectangle dirtyRect)
        {
            try
            {
                _dirty = false;
                _canvas.Graphics = graphics;
                if (_backgroundColor != null)
                {
                    _canvas.FillColor = _backgroundColor;
                    _canvas.FillRectangle(dirtyRect);
                    _canvas.FillColor = StandardColors.White;
                }

                _drawable?.Draw(_canvas, dirtyRect);
            }
            catch (Exception exc)
            {
                Logger.Warn(exc);
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

        public void Invalidate()
        {
            _dirty = true;
            _owner?.Refresh();
        }

        public void Invalidate(float x, float y, float w, float h)
        {
            _dirty = true;
            _owner?.Refresh();
        }

        public void Dispose()
        {
            // Do noathing
        }
    }
}