using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class FillRoundedRectangles : AbstractScenario
    {
        public FillRoundedRectangles()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            FillRoundedRectanglesOfDifferentSizesAndColors(canvas);
            FillRoundedRectanglesWithAlpha(canvas);
            FillShadowedRect(canvas);
            FillRoundedRectWithZeroAndLargeRadius(canvas);
        }

        private static void FillShadowedRect(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.FillColor = StandardColors.Black;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.FillRoundedRectangle(50.5f, 400.5f, 200, 50, 10);
            canvas.RestoreState();
        }

        private static void FillRoundedRectanglesOfDifferentSizesAndColors(EWCanvas canvas)
        {
            canvas.FillColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.FillRoundedRectangle(50, 50 + i * 30, 150, 20, 5);
            }

            canvas.FillColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.FillRoundedRectangle(250.5f, 50.5f + i * 30, 150, 20, 5);
            }
        }

        private static void FillRoundedRectanglesWithAlpha(EWCanvas canvas)
        {
            canvas.FillColor = StandardColors.Black;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.FillRoundedRectangle(450f, 200f + i * 30, 150, 20, 5);
            }

            canvas.Alpha = 1;
        }

        private void FillRoundedRectWithZeroAndLargeRadius(EWCanvas canvas)
        {
            canvas.FillColor = StandardColors.Blue;
            canvas.FillRoundedRectangle(250.5f, 700.5f, 150, 20, 0);
            canvas.FillRoundedRectangle(450.5f, 700.5f, 150, 20, 50);
        }
    }
}