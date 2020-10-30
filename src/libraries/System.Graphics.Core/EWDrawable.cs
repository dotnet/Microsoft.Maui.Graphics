namespace System.Graphics
{
    public interface EWDrawable
    {
        void Draw(ICanvas canvas, EWRectangle dirtyRect);
    }
}