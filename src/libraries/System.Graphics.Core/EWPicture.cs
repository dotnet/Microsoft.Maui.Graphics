namespace System.Graphics
{
    public interface EWPicture
    {
        void Draw(EWCanvas canvas, float zoom, float ppu);

        float X { get; }

        float Y { get; }

        float Width { get; }

        float Height { get; }
    }
}