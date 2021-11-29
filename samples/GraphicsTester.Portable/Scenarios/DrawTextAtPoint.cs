using Microsoft.Maui.Graphics;

namespace GraphicsTester.Scenarios
{
	public class DrawTextAtPoint : AbstractScenario
	{
		public DrawTextAtPoint()
			: base(720, 1024)
		{
		}

		public override void Draw(ICanvas canvas)
		{
			canvas.StrokeColor = Colors.Blue;
			canvas.StrokeSize = 1f;
			canvas.FontColor = Colors.Red;
			canvas.FontSize = 12f;

			canvas.DrawLine(50, 50, 250, 50);
			canvas.DrawString("Red - Align Start", 50, 50, TextAlignment.Start);

			canvas.DrawLine(50, 100, 250, 100);
			canvas.DrawString("Red - Align Center", 150, 100, TextAlignment.Center);

			canvas.DrawLine(50, 150, 250, 150);
			canvas.DrawString("Red - Align End", 250, 150, TextAlignment.End);

			canvas.SaveState();
			canvas.SetShadow(CanvasDefaults.DefaultShadowOffset, CanvasDefaults.DefaultShadowBlur, CanvasDefaults.DefaultShadowColor);
			canvas.DrawString("Red - Shadowed", 50, 200, TextAlignment.Start);
			canvas.RestoreState();

			var blurrableCanvas = canvas as IBlurrableCanvas;
			if (blurrableCanvas != null)
			{
				canvas.SaveState();
				blurrableCanvas.SetBlur(CanvasDefaults.DefaultShadowBlur);
				canvas.DrawString("Red - Shadowed", 50, 250, TextAlignment.Start);
				canvas.RestoreState();
			}

			canvas.SaveState();
			canvas.SetToBoldSystemFont();
			canvas.DrawString("Bold System Font", 50, 350, TextAlignment.Start);
			canvas.SetToSystemFont();
			canvas.DrawString("System Font", 50, 400, TextAlignment.Start);
			canvas.RestoreState();
		}
	}
}
