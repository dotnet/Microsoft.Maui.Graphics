﻿using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawLines : AbstractScenario
    {
        public DrawLines() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.DrawLine(50, 20.5f, 200, 20.5f);

            canvas.SaveState();

            DrawLinesOfDifferentSizesAndColors(canvas);
            DrawDashedLinesOfDifferentSizes(canvas);
            DrawLinesWithLineCaps(canvas);
            DrawLinesWithAlpha(canvas);
            DrawShadowedLine(canvas);

            canvas.RestoreState();

            canvas.DrawLine(50, 30.5f, 200, 30.5f);
        }

        private static void DrawShadowedLine(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 10;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawLine(50, 400, 200, 400);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.StrokeColor = StandardColors.Salmon;
            canvas.StrokeSize = 10;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawLine(50, 450, 200, 450);
            canvas.RestoreState();
        }

        private static void DrawLinesWithLineCaps(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 20;
            canvas.StrokeDashPattern = null;
            canvas.StrokeLineCap = EWLineCap.BUTT;
            canvas.DrawLine(50, 250, 200, 250);
            canvas.StrokeLineCap = EWLineCap.ROUND;
            canvas.DrawLine(50, 300, 200, 300);
            canvas.StrokeLineCap = EWLineCap.SQUARE;
            canvas.DrawLine(50, 350, 200, 350);

            canvas.StrokeColor = StandardColors.Blue;
            canvas.StrokeSize = 1;
            canvas.StrokeLineCap = EWLineCap.BUTT;
            canvas.DrawLine(50, 250, 200, 250);
            canvas.DrawLine(50, 300, 200, 300);
            canvas.DrawLine(50, 350, 200, 350);
        }

        private static void DrawDashedLinesOfDifferentSizes(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.StrokeDashPattern = StandardLines.DASHED;
                canvas.DrawLine(50, 100 + i * 10, 200, 100 + i * 10);
                canvas.DrawLine(250, 100.5f + i * 10, 400, 100.5f + i * 10);
            }
        }

        private static void DrawLinesOfDifferentSizesAndColors(EWCanvas canvas)
        {
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawLine(50, 50 + i * 10, 200, 50 + i * 10);
                canvas.DrawLine(250, 50.5f + i * 10, 400, 50.5f + i * 10);
            }

            canvas.StrokeColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawLine(450, 50.5f + i * 10, 600, 50.5f + i * 10);
            }
        }

        private static void DrawLinesWithAlpha(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.DrawLine(250, 250f + i * 10, 400, 250f + i * 10);
            }

            canvas.Alpha = 1;
        }
    }
}