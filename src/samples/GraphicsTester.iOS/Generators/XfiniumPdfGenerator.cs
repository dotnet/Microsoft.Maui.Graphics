using Elevenworks.Graphics.Xfinium;
using Xfinium.Pdf;
using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public class XfiniumPdfGenerator : AbstractGenerator
	{
		public XfiniumPdfGenerator () : base("pdf")
		{
		}

		protected override void GenerateAtPath (EWDrawable drawable, float width, float height, string path)
		{
			var document = new PdfFixedDocument ();
			var page = document.Pages.Add ();
			page.Width = width;
			page.Height = height;
			var canvas = new PdfCanvas (page);
			drawable.Draw (canvas, new EWRectangle(0,0,width, height));
			document.Save (path);
		}
	}
}

