﻿using System.Graphics;

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
            canvas.DrawString("Red - Align Left", 50, 50, EwHorizontalAlignment.Left);

            canvas.DrawLine(50, 100, 250, 100);
            canvas.DrawString("Red - Align Center", 150, 100, EwHorizontalAlignment.Center);

            canvas.DrawLine(50, 150, 250, 150);
            canvas.DrawString("Red - Align Right", 250, 150, EwHorizontalAlignment.Right);

            canvas.SaveState();
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawString("Red - Shadowed", 50, 200, EwHorizontalAlignment.Left);
            canvas.RestoreState();

            var blurrableCanvas = canvas as BlurrableCanvas;
            if (blurrableCanvas != null)
            {
                canvas.SaveState();
                blurrableCanvas.SetBlur(EWCanvas.DefaultShadowBlur);
                canvas.DrawString("Red - Shadowed", 50, 250, EwHorizontalAlignment.Left);
                canvas.RestoreState();
            }

            canvas.SaveState();
            canvas.SetToBoldSystemFont();
            canvas.DrawString("Bold System Font", 50, 350, EwHorizontalAlignment.Left);
            canvas.SetToSystemFont();
            canvas.DrawString("System Font", 50, 400, EwHorizontalAlignment.Left);
            canvas.RestoreState();
        }
    }
}