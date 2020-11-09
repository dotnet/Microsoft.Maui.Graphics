﻿using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class MultipleShadowTest : AbstractScenario
    {
        public MultipleShadowTest() : base(20, 20)
        {
        }

        public override void Draw(ICanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.SetShadow(
                CanvasDefaults.DefaultShadowOffset,
                CanvasDefaults.DefaultShadowBlur,
                CanvasDefaults.DefaultShadowColor);

            canvas.FillColor = Colors.Blue;
            var path = new EWPath(10, 10);
            path.LineTo(30, 10);
            path.LineTo(20, 30);
            path.Close();
            canvas.FillPath(path);

            canvas.FillColor = Colors.Salmon;
            canvas.FillRectangle(100, 100, 100, 100);

            canvas.FillColor = Colors.Beige;
            canvas.FillOval(100, 300, 100, 50);

            canvas.RestoreState();
        }
    }
}