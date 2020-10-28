using System;
using System.Windows.Forms;

namespace Elevenworks.Graphics
{
    public partial class GDIGraphicsView : UserControl
    {
        private readonly EWRectangle dirtyRect = new EWRectangle();
        private GDIGraphicsRenderer renderer;
        private EWDrawable drawable;

        public GDIGraphicsView()
        {
            DoubleBuffered = true;
            Renderer = null;
            Drawable = null;
        }

        public GDIGraphicsView(EWDrawable drawable = null, GDIGraphicsRenderer renderer = null)
        {
            DoubleBuffered = true;
            Drawable = drawable;
            Renderer = renderer;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do nothing
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            renderer.Draw(e.Graphics, e.ClipRectangle.AsEWRectangle());
        }

        public bool Dirty
        {
            get => renderer.Dirty;
            set => renderer.Dirty = value;
        }

        public EWColor BackgroundColor
        {
            get => renderer.BackgroundColor;
            set => renderer.BackgroundColor = value;
        }

        public GDIGraphicsRenderer Renderer
        {
            get => renderer;

            set
            {
                if (renderer != null)
                {
                    renderer.Drawable = null;
                    renderer.GraphicsView = null;
                    renderer.Dispose();
                }

                renderer = value ?? new GDIDirectGraphicsRenderer()
                {
                    BackgroundColor = StandardColors.White
                };

                renderer.GraphicsView = this;
                renderer.Drawable = drawable;
            }
        }

        public EWDrawable Drawable
        {
            get => drawable;
            set
            {
                drawable = value;
                if (renderer != null)
                {
                    renderer.Drawable = drawable;
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            renderer.SizeChanged(Width, Height);
        }
    }
}