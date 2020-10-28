using System.IO;
using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public abstract class AbstractGenerator
	{
		private readonly string _extension;

		protected AbstractGenerator (string extension)
		{
			this._extension = extension;
		}

		public string Extension => this._extension;

		public string Generate(EWDrawable drawable, float width, float height)
		{
			var filename = $"{drawable.GetType().Name}.{Extension}";
			var path = Path.Combine (Path.GetTempPath(), filename);
			GenerateAtPath (drawable, width, height, path);
			return path;
		}

		protected abstract void GenerateAtPath (EWDrawable drawable, float width, float height, string path);
	}
}

