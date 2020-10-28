using Elevenworks.Actions;
using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public class GeneratorAction : IContextualAction
	{
		private readonly AbstractGenerator _generator;
		private readonly EWPicture _picture;
		private readonly EWDrawable _drawable;

		public GeneratorAction ()
		{
		}
		
		public GeneratorAction (AbstractGenerator generator, EWPicture picture)
		{
			_generator = generator;
			_picture = picture;
			_drawable = new PictureDrawable (picture);
		}

		public void Invoke (object context)
		{
			var file = _generator.Generate (_drawable, _picture.Width, _picture.Height);
			if (file != null)
			{
				if (context is DetailViewController detailViewController)
				{
					detailViewController.ShowFile (file);
				}
			}
		}

		public bool IsApplicable (object context)
		{
			return true;
		}
	}
}

