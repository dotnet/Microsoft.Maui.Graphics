using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public abstract class AbstractScenario : EWPicture
	{
		private float _x;
		private float _y;
		private float _width;
		private float _height;
		private string _hash;

		public float X
		{
			get => this._x;
			set => _x = value;
		}

		public float Y
		{
			get => this._y;
			set => _y = value;
		}

		public float Width
		{
			get => this._width;
			set => _width = value;
		}

		public float Height
		{
			get => this._height;
			set => _height = value;
		}

		public AbstractScenario (float x, float y, float width, float height)
		{
			this._x = x;
			this._y = y;
			this._width = width;
			this._height = height;
		}
		
		public AbstractScenario (float width, float height)
		{
			this._width = width;
			this._height = height;
		}

		public virtual void Draw (EWCanvas canvas, float zoom, float ppu)
		{
			// Do nothing by default
		}

		public string Hash
		{
			get => _hash;
			set => _hash = value;
		}
	}
}

