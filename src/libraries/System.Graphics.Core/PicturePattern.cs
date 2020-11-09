namespace System.Graphics
{
    public class PicturePattern : AbstractPattern
    {
        private readonly Picture _picture;

        public PicturePattern(Picture picture, float stepX, float stepY) : base(picture.Width, picture.Height, stepX, stepY)
        {
            _picture = picture;
        }

        public PicturePattern(Picture picture) : base(picture.Width, picture.Height)
        {
            _picture = picture;
        }

        public override void Draw(ICanvas canvas)
        {
            _picture.Draw(canvas);
        }
    }
}