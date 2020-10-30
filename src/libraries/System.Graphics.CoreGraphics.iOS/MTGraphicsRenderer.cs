using CoreGraphics;

namespace System.Graphics.CoreGraphics
{
    public interface MTGraphicsRenderer : IDisposable
    {
        MTGraphicsView GraphicsView { set; }
        ICanvas Canvas { get; }
        EWDrawable Drawable { get; set; }
        void Draw(CGContext coreGraphics, EWRectangle dirtyRect, bool inPanOrZoom);
        void SizeChanged(float width, float height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}