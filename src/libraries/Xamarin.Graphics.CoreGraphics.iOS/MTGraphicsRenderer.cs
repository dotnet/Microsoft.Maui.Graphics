using System;
using CoreGraphics;

namespace Xamarin.Graphics.CoreGraphics
{
    public interface MTGraphicsRenderer : IDisposable
    {
        MTGraphicsView GraphicsView { set; }
        EWCanvas Canvas { get; }
        EWDrawable Drawable { get; set; }
        void Draw(CGContext coreGraphics, EWRectangle dirtyRect, bool inPanOrZoom);
        void SizeChanged(float width, float height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}