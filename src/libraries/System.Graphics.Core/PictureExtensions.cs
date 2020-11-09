namespace System.Graphics
{
    public static class PictureExtensions
    {
        public static EWRectangle GetBounds(this Picture target)
        {
            if (target == null) return null;

            return new EWRectangle(target.X, target.Y, target.Width, target.Height);
        }
    }
}