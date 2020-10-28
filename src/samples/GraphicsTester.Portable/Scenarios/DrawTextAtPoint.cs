using System;
using System.Reflection;
using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawTextAtPoint : AbstractScenario
    {
        public DrawTextAtPoint()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.StrokeColor = StandardColors.Blue;
            canvas.StrokeSize = 1f;
            canvas.FontColor = StandardColors.Red;
            canvas.FontSize = 12f;

            canvas.DrawLine(50, 50, 250, 50);
            canvas.DrawString("Red - Align Left", 50, 50, EWHorizontalAlignment.LEFT);

            canvas.DrawLine(50, 100, 250, 100);
            canvas.DrawString("Red - Align Center", 150, 100, EWHorizontalAlignment.CENTER);

            canvas.DrawLine(50, 150, 250, 150);
            canvas.DrawString("Red - Align Right", 250, 150, EWHorizontalAlignment.RIGHT);

            canvas.SaveState();
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawString("Red - Shadowed", 50, 200, EWHorizontalAlignment.LEFT);
            canvas.RestoreState();

            var blurrableCanvas = canvas as BlurrableCanvas;
            if (blurrableCanvas != null)
            {
                canvas.SaveState();
                blurrableCanvas.SetBlur(EWCanvas.DefaultShadowBlur);
                canvas.DrawString("Red - Shadowed", 50, 250, EWHorizontalAlignment.LEFT);
                canvas.RestoreState();
            }

            canvas.SaveState();
            canvas.SetToBoldSystemFont();
            canvas.DrawString("Bold System Font", 50, 350, EWHorizontalAlignment.LEFT);
            canvas.SetToSystemFont();
            canvas.DrawString("System Font", 50, 400, EWHorizontalAlignment.LEFT);
            canvas.RestoreState();
        }
    }
}