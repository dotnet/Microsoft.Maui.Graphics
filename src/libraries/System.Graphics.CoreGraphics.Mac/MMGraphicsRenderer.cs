using CoreGraphics;

namespace System.Graphics.CoreGraphics
{
    public interface MMGraphicsRenderer : IDisposable
    {
        MMGraphicsView GraphicsView { set; }
        ICanvas Canvas { get; }
        IDrawable Drawable { get; set; }
        void Draw(CGContext nativeCanvas, EWRectangle dirtyRect, bool inPanOrZoom);
        void SizeChanged(float width, float height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}