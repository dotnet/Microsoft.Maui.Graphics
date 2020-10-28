using Elevenworks.Graphics;

namespace GraphicsTester.iOS
{
	public class FilledRectangle : AbstractScenario
	{
		public FilledRectangle() : base (720, 1024)
		{

		}
		
		public override void Draw (EWCanvas canvas, float zoom, float ppu)
		{
			canvas.FillColor = StandardColors.CornflowerBlue;
			canvas.FillRectangle (50, 50, 100, 100);

			canvas.SetFillPaint (StandardPatterns.DiagonalUpward.AsPaint (), 0, 0, 0, 0, 0, 0);
			canvas.FillRectangle (100, 100, 100, 100);
		}
	}
}

