using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawRectangles : AbstractScenario
    {
        public DrawRectangles() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.DrawRectangle(50.5f, 20.5f, 150, 5);

            canvas.SaveState();

            DrawRectanglesOfDifferentSizesAndColors(canvas);
            DrawRectanglesWithDashesOfDifferentSizes(canvas);
            DrawRectanglesWithAlpha(canvas);
            DrawShadowedRect(canvas);
            DrawRectanglesWithDifferentStrokeLocations(canvas);

            canvas.RestoreState();

            canvas.DrawRectangle(50.5f, 30.5f, 150, 5);
        }

        private static void DrawShadowedRect(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 5;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawRectangle(50.5f, 400.5f, 200, 50);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.StrokeColor = StandardColors.CornflowerBlue;
            canvas.StrokeSize = 5;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawRectangle(50.5f, 460.5f, 200, 50);
            canvas.RestoreState();
        }

        private static void DrawRectanglesWithDashesOfDifferentSizes(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.StrokeDashPattern = DASHED;
                canvas.DrawRectangle(50f, 200f + i * 30, 150, 20);
                canvas.DrawRectangle(250.5f, 200.5f + i * 30, 150, 20);
            }

            canvas.StrokeDashPattern = SOLID;
        }

        private static void DrawRectanglesOfDifferentSizesAndColors(EWCanvas canvas)
        {
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawRectangle(50, 50 + i * 30, 150, 20);
                canvas.DrawRectangle(250.5f, 50.5f + i * 30, 150, 20);
            }

            canvas.StrokeColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawRectangle(450.5f, 50.5f + i * 30, 150, 20);
            }
        }

        private static void DrawRectanglesWithAlpha(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.DrawRectangle(450f, 200f + i * 30, 150, 20);
            }

            canvas.Alpha = 1;
        }

        private static void DrawRectanglesWithDifferentStrokeLocations(EWCanvas canvas)
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
                canvas.DrawRectangle(50.5f, 500.5f + i * 40, 150, 20);
            }

            canvas.StrokeLocation = EWStrokeLocation.OUTSIDE;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawRectangle(250.5f, 500.5f + i * 40, 150, 20);
            }

            canvas.StrokeLocation = EWStrokeLocation.CENTER;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawRectangle(450.5f, 500.5f + i * 40, 150, 20);
            }
        }
    }
}