using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class FillArcs : AbstractScenario
    {
        public readonly bool includeOvals;

        public FillArcs(bool includeOvals = false)
            : base(720, 1024)
        {
            this.includeOvals = includeOvals;
        }

        public override void Draw(ICanvas canvas, float zoom, float ppu)
        {
            if (includeOvals)
            {
                canvas.FillColor = StandardColors.LightGrey;
                canvas.FillOval(50.5f, 10.5f, 150, 15);
                canvas.FillOval(250.5f, 10.5f, 150, 15);
                canvas.FillColor = StandardColors.Black;
            }

            canvas.FillColor = StandardColors.Black;
            canvas.FillArc(50.5f, 10.5f, 150, 15, 90, 270, false);
            canvas.FillArc(250.5f, 10.5f, 150, 15, 90, 270, false);

            canvas.SaveState();

            if (includeOvals)
            {
                OvalFillArcsOfDifferentSizesAndColors(canvas);
                OvalFillShadowedRect(canvas);
            }

            FillArcsOfDifferentSizesAndColors(canvas);
            FillArcsWithAlpha(canvas);
            FillShadowedRect(canvas);

            canvas.RestoreState();

            if (includeOvals)
            {
                canvas.FillColor = StandardColors.LightGrey;
                canvas.FillOval(50.5f, 30.5f, 150, 15);
                canvas.FillOval(250.5f, 30.5f, 150, 15);
                canvas.FillColor = StandardColors.Black;
            }

            canvas.FillColor = StandardColors.Black;
            canvas.FillArc(50.5f, 30.5f, 150, 15, 90, 270, true);
            canvas.FillArc(250.5f, 30.5f, 150, 15, 90, 270, true);
        }

        private static void OvalFillShadowedRect(ICanvas canvas)
        {
            canvas.FillColor = StandardColors.LightGrey;
            canvas.FillOval(50.5f, 400.5f, 200, 50);
            canvas.FillColor = StandardColors.Black;
        }

        private static void FillShadowedRect(ICanvas canvas)
        {
            canvas.SaveState();
            canvas.FillColor = StandardColors.Black;
            canvas.SetShadow(CanvasDefaults.DefaultShadowOffset, CanvasDefaults.DefaultShadowBlur, CanvasDefaults.DefaultShadowColor);
            canvas.FillArc(50.5f, 400.5f, 200, 50, 90, 270, true);

            canvas.RestoreState();
        }

        private static void OvalFillArcsOfDifferentSizesAndColors(ICanvas canvas)
        {
            canvas.FillColor = StandardColors.LightGrey;
            for (int i = 1; i < 5; i++)
            {
                canvas.FillOval(50, 50 + i * 30, 150, 20);
                canvas.FillOval(250.5f, 50.5f + i * 30, 150, 20);
            }

            for (int i = 1; i < 5; i++)
            {
                canvas.FillOval(450.5f, 50.5f + i * 30, 150, 20);
            }

            canvas.FillColor = StandardColors.Black;
        }

        private static void FillArcsOfDifferentSizesAndColors(ICanvas canvas)
        {
            canvas.FillColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.FillArc(50, 50 + i * 30, 150, 20, 45, 180, false);
                canvas.FillArc(250.5f, 50.5f + i * 30, 150, 20, 45, 180, false);
            }

            canvas.FillColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.FillArc(450.5f, 50.5f + i * 30, 150, 20, 45, 180, false);
            }
        }

        private static void FillArcsWithAlpha(ICanvas canvas)
        {
            canvas.FillColor = StandardColors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.FillArc(450f, 200f + i * 30, 150, 20, 180, 0, true);
            }

            canvas.Alpha = 1;
        }

        public override string ToString()
        {
            if (includeOvals)
            {
                return "FillArcs (Including Background Ovals)";
            }

            return base.ToString();
        }
    }
}