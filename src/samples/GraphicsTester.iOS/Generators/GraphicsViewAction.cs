using Elevenworks.Actions;
using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public class GraphicsViewAction : IContextualAction
	{
		private readonly EWDrawable _drawable;

		public GraphicsViewAction ()
		{
		}
		
		public GraphicsViewAction (EWPicture picture)
		{
			_drawable = new PictureDrawable (picture);
		}

		public void Invoke (object context)
		{
			if (context is DetailViewController detailViewController)
			{
				detailViewController.ShowDrawable(_drawable);
			}
		}

		public bool IsApplicable (object context)
		{
			return true;
		}
	}
}

