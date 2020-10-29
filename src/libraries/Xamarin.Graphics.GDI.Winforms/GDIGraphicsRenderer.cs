using System;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public interface GDIGraphicsRenderer : IDisposable
    {
        GDIGraphicsView GraphicsView { set; }

        EWCanvas Canvas { get; }

        EWColor BackgroundColor { get; set; }

        EWDrawable Drawable { get; set; }

        void Draw(System.Drawing.Graphics graphics, EWRectangle dirtyRect);

        void SizeChanged(int width, int height);

        void Detached();

        bool Dirty { get; set; }

        void Invalidate();

        void Invalidate(float x, float y, float w, float h);
    }
}