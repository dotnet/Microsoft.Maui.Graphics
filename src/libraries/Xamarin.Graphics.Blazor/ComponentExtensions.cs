using Xamarin.Graphics.Blazor.Canvas2D;

namespace Xamarin.Graphics.Blazor
{
    public static class ComponentExtensions
    {
        public static CanvasRenderingContext2D CreateRenderingContext2D(this CanvasComponentBase canvas)
        {
            return new CanvasRenderingContext2D(canvas);
        }
    }
}
