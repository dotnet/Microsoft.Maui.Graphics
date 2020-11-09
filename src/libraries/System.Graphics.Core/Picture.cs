namespace System.Graphics
{
    public interface Picture
    {
        void Draw(ICanvas canvas);

        float X { get; }

        float Y { get; }

        float Width { get; }

        float Height { get; }
    }
}