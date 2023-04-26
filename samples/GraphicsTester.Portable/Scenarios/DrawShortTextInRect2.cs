using Microsoft.Maui.Graphics;

namespace GraphicsTester.Scenarios
{
	public class DrawShortTextInRect2 : AbstractScenario
	{
		public DrawShortTextInRect2()
			: base(800, 1024)
		{
		}

		public override void Draw(ICanvas canvas)
		{
			canvas.StrokeSize = 1;
			canvas.StrokeColor = Colors.Blue;
			canvas.Font = Font.Default;
			canvas.FontSize = 24f;

			const string textShort = "Lorem ipsum dolor sit amet";

			canvas.SaveState();
			canvas.SetShadow(new SizeF(2, 2), 2, Colors.DarkGrey);

			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 3; y++)
				{
					float dx = x * 200;
					float dy = y * 150;

					canvas.DrawRectangle(dx, dy, 190, 140);

					var horizontalAlignment = (HorizontalAlignment) x;
					var verticalAlignment = (VerticalAlignment) y;

					canvas.DrawString(textShort, dx, dy, 190, 140, horizontalAlignment, verticalAlignment);
				}
			}

			canvas.RestoreState();
		}
	}
}
