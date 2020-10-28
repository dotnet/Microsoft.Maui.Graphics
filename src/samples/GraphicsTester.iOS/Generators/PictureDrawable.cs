using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public class PictureDrawable : EWDrawable
	{
		private EWPicture _picture;

		public PictureDrawable(EWPicture picture = null)
		{
			this._picture = picture;
		}

		public EWPicture Picture
		{
			get => _picture;
			set => _picture = value;
		}

		public void Draw(EWCanvas canvas, EWRectangle dirtyRect)
		{
			canvas.FillColor = StandardColors.White;
			canvas.FillRectangle(dirtyRect);
			canvas.DrawPicture(_picture, 0, 0, _picture.Width, _picture.Height, 1, 1);
		}
	}
}
