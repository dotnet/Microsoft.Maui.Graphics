﻿using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class RadialGradientInCircle : AbstractScenario
    {
        public RadialGradientInCircle() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.StrokeLocation = EWStrokeLocation.CENTER;

            var paint = new EWPaint
            {
                PaintType = EWPaintType.RADIAL_GRADIENT,
                StartColor = StandardColors.White,
                EndColor = StandardColors.Black
            };

            canvas.SetFillPaint(paint, 200, 200, 300, 200);
            canvas.FillOval(100, 100, 200, 200);

            canvas.SetFillPaint(paint, 250, 500, 100, 500);
            canvas.FillOval(100, 400, 200, 200);

            canvas.RestoreState();
        }
    }
}