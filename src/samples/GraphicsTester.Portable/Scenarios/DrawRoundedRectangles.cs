using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawRoundedRectangles : AbstractScenario
    {
        public DrawRoundedRectangles() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.DrawRoundedRectangle(50.5f, 10.5f, 150, 15, 5);

            canvas.SaveState();

            DrawRoundedRectanglesOfDifferentSizesAndColors(canvas);
            DrawRoundedRectanglesWithDashesOfDifferentSizes(canvas);
            DrawRoundedRectanglesWithAlpha(canvas);
            DrawShadowedRect(canvas);
            DrawRoundedRectanglesWithDifferentStrokeLocations(canvas);
            DrawRoundedRectWithZeroAndLargeRadius(canvas);
            canvas.RestoreState();

            canvas.DrawRoundedRectangle(50.5f, 30.5f, 150, 15, 5);
        }

        private static void DrawShadowedRect(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 5;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawRoundedRectangle(50.5f, 400.5f, 200, 50, 10);

            canvas.RestoreState();
        }

        private static void DrawRoundedRectanglesWithDashesOfDifferentSizes(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.StrokeDashPattern = StandardLines.DASHED;
                canvas.DrawRoundedRectangle(50f, 200f + i * 30, 150, 20, 5);
                canvas.DrawRoundedRectangle(250.5f, 200.5f + i * 30, 150, 20, 5);
            }

            canvas.StrokeDashPattern = StandardLines.SOLID;
        }

        private static void DrawRoundedRectanglesOfDifferentSizesAndColors(EWCanvas canvas)
        {
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawRoundedRectangle(50, 50 + i * 30, 150, 20, 5);
                canvas.DrawRoundedRectangle(250.5f, 50.5f + i * 30, 150, 20, 5);
            }

            canvas.StrokeColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawRoundedRectangle(450.5f, 50.5f + i * 30, 150, 20, 5);
            }
        }

        private static void DrawRoundedRectanglesWithAlpha(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.DrawRoundedRectangle(450f, 200f + i * 30, 150, 20, 5);
            }

            canvas.Alpha = 1;
        }

        private static void DrawRoundedRectanglesWithDifferentStrokeLocations(EWCanvas canvas)
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
                canvas.DrawRoundedRectangle(50.5f, 500.5f + i * 40, 150, 20, 5);
            }

            canvas.StrokeLocation = EWStrokeLocation.OUTSIDE;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawRoundedRectangle(250.5f, 500.5f + i * 40, 150, 20, 5);
            }

            canvas.StrokeLocation = EWStrokeLocation.CENTER;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawRoundedRectangle(450.5f, 500.5f + i * 40, 150, 20, 5);
            }
        }

        private void DrawRoundedRectWithZeroAndLargeRadius(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Blue;
            canvas.StrokeSize = 1;
            canvas.DrawRoundedRectangle(250.5f, 700.5f, 150, 20, 0);
            canvas.DrawRoundedRectangle(450.5f, 700.5f, 150, 20, 50);
        }
    }
}