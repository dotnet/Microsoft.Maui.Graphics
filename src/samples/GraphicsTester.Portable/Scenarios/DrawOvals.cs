using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawOvals : AbstractScenario
    {
        public DrawOvals() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.DrawOval(50.5f, 20.5f, 150, 5);

            canvas.SaveState();

            DrawOvalsOfDifferentSizesAndColors(canvas);
            DrawOvalsWithDashesOfDifferentSizes(canvas);
            DrawOvalsWithAlpha(canvas);
            DrawShadowedRect(canvas);
            DrawOvalsWithDifferentStrokeLocations(canvas);

            canvas.RestoreState();

            canvas.DrawOval(50.5f, 30.5f, 150, 5);
        }

        private static void DrawShadowedRect(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 5;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawOval(50.5f, 400.5f, 200, 50);

            canvas.RestoreState();
        }

        private static void DrawOvalsWithDashesOfDifferentSizes(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.StrokeDashPattern = DASHED;
                canvas.DrawOval(50f, 200f + i * 30, 150, 20);
                canvas.DrawOval(250.5f, 200.5f + i * 30, 150, 20);
            }

            canvas.StrokeDashPattern = SOLID;
        }

        private static void DrawOvalsOfDifferentSizesAndColors(EWCanvas canvas)
        {
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawOval(50, 50 + i * 30, 150, 20);
                canvas.DrawOval(250.5f, 50.5f + i * 30, 150, 20);
            }

            canvas.StrokeColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawOval(450.5f, 50.5f + i * 30, 150, 20);
            }
        }

        private static void DrawOvalsWithAlpha(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.DrawOval(450f, 200f + i * 30, 150, 20);
            }

            canvas.Alpha = 1;
        }

        private static void DrawOvalsWithDifferentStrokeLocations(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Blue;
            canvas.StrokeSize = 1;
            canvas.DrawLine(0, 540.5f, 650, 540.5f);
            canvas.DrawLine(0, 580.5f, 650, 580.5f);
            canvas.DrawLine(0, 620.5f, 650, 620.5f);

            canvas.StrokeColor = StandardColors.ForestGreen;
            canvas.StrokeLocation = EWStrokeLocation.INSIDE;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawOval(50.5f, 500.5f + i * 40, 150, 20);
            }

            canvas.StrokeLocation = EWStrokeLocation.OUTSIDE;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawOval(250.5f, 500.5f + i * 40, 150, 20);
            }

            canvas.StrokeLocation = EWStrokeLocation.CENTER;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawOval(450.5f, 500.5f + i * 40, 150, 20);
            }
        }
    }
}