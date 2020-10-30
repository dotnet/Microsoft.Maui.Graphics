namespace System.Graphics
{
    public static class EWPictureExtensions
    {
        public static EWRectangle GetBounds(this EWPicture target)
        {
            if (target == null) return null;

            return new EWRectangle(target.X, target.Y, target.Width, target.Height);
        }
    }
}