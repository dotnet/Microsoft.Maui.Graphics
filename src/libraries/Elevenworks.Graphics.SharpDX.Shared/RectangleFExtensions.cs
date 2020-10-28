using SharpDX.Mathematics.Interop;

namespace Elevenworks.Graphics.SharpDX
{
    public static class RectangleFExtensions
    {
        public static EWRectangle AsEWRectangle(this RawRectangleF target)
        {
            var width = target.Right - target.Left;
            var height = target.Bottom - target.Top;
            return new EWRectangle(target.Left, target.Top, width, height);
        }
    }
}