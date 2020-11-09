using SkiaSharp;

namespace System.Graphics.Skia
{
    public interface ISkiaGraphicsRenderer : IDisposable
    {
        WDSkiaGraphicsView GraphicsView { set; }
        ICanvas Canvas { get; }
        EWDrawable Drawable { get; set; }
        Color BackgroundColor { get; set; }
        void Draw(SKCanvas canvas, EWRectangle dirtyRect);
        void SizeChanged(int width, int height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}