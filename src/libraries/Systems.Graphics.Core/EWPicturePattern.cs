namespace Xamarin.Graphics
{
    public class EWPicturePattern : AbstractPattern
    {
        private readonly EWPicture _picture;

        public EWPicturePattern(EWPicture picture, float stepX, float stepY) : base(picture.Width, picture.Height, stepX, stepY)
        {
            _picture = picture;
        }

        public EWPicturePattern(EWPicture picture) : base(picture.Width, picture.Height)
        {
            _picture = picture;
        }

        public override void Draw(EWCanvas canvas)
        {
            _picture.Draw(canvas, 1, 1);
        }
    }
}