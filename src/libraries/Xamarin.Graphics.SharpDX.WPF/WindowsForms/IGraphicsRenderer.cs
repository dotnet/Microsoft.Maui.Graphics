using System;
using Xamarin.Graphics;

namespace Elevenworks.Graphics.SharpDX.WindowsForms
{
    public interface IGraphicsRenderer : IDisposable
    {
        WFGraphicsView GraphicsView { set; }

        EWCanvas Canvas { get; }

        EWColor BackgroundColor { get; set; }

        EWDrawable Drawable { get; set; }

        void Draw(EWRectangle dirtyRect);

        void SizeChanged(int width, int height);

        void Detached();

        bool Dirty { get; set; }

        void Invalidate();

        void Invalidate(float x, float y, float w, float h);

        void StartServiceContext();

        void EndServiceContext();
    }
}