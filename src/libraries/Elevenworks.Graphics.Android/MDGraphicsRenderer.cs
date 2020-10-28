using System;
using Android.Graphics;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public interface MDGraphicsRenderer : IDisposable
    {
        MDGraphicsView GraphicsView { set; }
        EWCanvas Canvas { get; }
        EWDrawable Drawable { get; set; }
        EWColor BackgroundColor { get; set; }
        void Draw(Canvas androidCanvas, EWRectangle dirtyRect, bool inPanOrZoom);
        void SizeChanged(int width, int height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}