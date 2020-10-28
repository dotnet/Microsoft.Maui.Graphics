using System;
using CoreGraphics;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public interface MMGraphicsRenderer : IDisposable
    {
        MMGraphicsView GraphicsView { set; }
        EWCanvas Canvas { get; }
        EWDrawable Drawable { get; set; }
        void Draw(CGContext nativeCanvas, EWRectangle dirtyRect, bool inPanOrZoom);
        void SizeChanged(float width, float height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}