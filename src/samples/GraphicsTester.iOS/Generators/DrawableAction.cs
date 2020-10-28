using Elevenworks.Actions;
using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public class GenerateAction : IContextualAction
	{
		private readonly AbstractGenerator _generator;
		private readonly EWDrawable _drawable;
		private readonly float _width;
		private readonly float _height;

		public GenerateAction ()
		{
		}
		
		public GenerateAction (AbstractGenerator generator, EWDrawable drawable, float width = 500, float height = 500)
		{
			this._generator = generator;
			this._drawable = drawable;
			this._width = width;
			this._height = height;
		}

		public void Invoke (object context)
		{
			var file = _generator.Generate (_drawable, _width, _height);
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

