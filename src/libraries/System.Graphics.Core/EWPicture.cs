namespace System.Graphics
{
    public interface EWPicture
    {
        void Draw(ICanvas canvas, float zoom, float ppu);

        float X { get; }

        float Y { get; }

        float Width { get; }

        float Height { get; }
    }
}