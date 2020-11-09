using Android.Graphics;

namespace System.Graphics.Android
{
    public interface MDGraphicsRenderer : IDisposable
    {
        MDGraphicsView GraphicsView { set; }
        ICanvas Canvas { get; }
        EWDrawable Drawable { get; set; }
        Color BackgroundColor { get; set; }
        void Draw(Canvas androidCanvas, EWRectangle dirtyRect, bool inPanOrZoom);
        void SizeChanged(int width, int height);
        void Detached();
        void Invalidate();
        void Invalidate(float x, float y, float w, float h);
    }
}