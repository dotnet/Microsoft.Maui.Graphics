namespace Elevenworks.Graphics
{
    public interface EWPicture : IHasheable
    {
        void Draw(EWCanvas canvas, float zoom, float ppu);

        float X { get; }

        float Y { get; }

        float Width { get; }

        float Height { get; }
    }

    public static class EWPictureExtensions
    {
        public static EWRectangle GetBounds(this EWPicture target)
        {
            if (target == null) return null;

            return new EWRectangle(target.X, target.Y, target.Width, target.Height);
        }
    }
}