namespace System.Graphics
{
    public interface EWDrawable
    {
        void Draw(EWCanvas canvas, EWRectangle dirtyRect);
    }
}