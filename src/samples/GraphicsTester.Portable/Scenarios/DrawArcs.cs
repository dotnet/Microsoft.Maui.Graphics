using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawArcs : AbstractScenario
    {
        public readonly bool includeOvals;

        public DrawArcs(bool includeOvals = false) : base(720, 1024)
        {
            this.includeOvals = includeOvals;
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            if (includeOvals)
            {
                canvas.StrokeColor = StandardColors.LightGrey;
                canvas.DrawOval(50.5f, 10.5f, 150, 15);
                canvas.DrawOval(250.5f, 10.5f, 150, 15);
                canvas.StrokeColor = StandardColors.Black;
            }

            canvas.DrawArc(50.5f, 10.5f, 150, 15, 90, 270, false, false);
            canvas.DrawArc(250.5f, 10.5f, 150, 15, 90, 270, false, true);

            canvas.SaveState();

            if (includeOvals)
            {
                OvalDrawArcsOfDifferentSizesAndColors(canvas);
                OvalDrawArcsWithDashesOfDifferentSizes(canvas);
                OvalDrawShadowedRect(canvas);
                OvalDrawArcsWithDifferentStrokeLocations(canvas);
            }

            DrawArcsOfDifferentSizesAndColors(canvas);
            DrawArcsWithDashesOfDifferentSizes(canvas);
            DrawArcsWithAlpha(canvas);
            DrawShadowedRect(canvas);
            DrawArcsWithDifferentStrokeLocations(canvas);

            canvas.RestoreState();

            if (includeOvals)
            {
                canvas.StrokeColor = StandardColors.LightGrey;
                canvas.DrawOval(50.5f, 30.5f, 150, 15);
                canvas.DrawOval(250.5f, 30.5f, 150, 15);
                canvas.StrokeColor = StandardColors.Black;
            }

            canvas.DrawArc(50.5f, 30.5f, 150, 15, 90, 270, true, false);
            canvas.DrawArc(250.5f, 30.5f, 150, 15, 90, 270, true, true);
        }

        private static void OvalDrawShadowedRect(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.StrokeSize = 5;
            canvas.DrawOval(50.5f, 400.5f, 200, 50);
            canvas.StrokeColor = StandardColors.Black;
        }

        private static void DrawShadowedRect(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 5;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            canvas.DrawArc(50.5f, 400.5f, 200, 50, 90, 270, true, false);

            canvas.RestoreState();
        }

        private static void OvalDrawArcsWithDashesOfDifferentSizes(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawOval(50f, 200f + i * 30, 150, 20);
                canvas.DrawOval(250.5f, 200.5f + i * 30, 150, 20);
            }

            canvas.StrokeColor = StandardColors.Black;
        }

        private static void DrawArcsWithDashesOfDifferentSizes(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.StrokeDashPattern = DASHED;
                canvas.DrawArc(50f, 200f + i * 30, 150, 20, 0, 180, false, false);
                canvas.DrawArc(250.5f, 200.5f + i * 30, 150, 20, 0, 180, false, false);
            }

            canvas.StrokeDashPattern = SOLID;
        }

        private static void OvalDrawArcsOfDifferentSizesAndColors(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawOval(50, 50 + i * 30, 150, 20);
                canvas.DrawOval(250.5f, 50.5f + i * 30, 150, 20);
            }

            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawOval(450.5f, 50.5f + i * 30, 150, 20);
            }

            canvas.StrokeColor = StandardColors.Black;
        }

        private static void DrawArcsOfDifferentSizesAndColors(EWCanvas canvas)
        {
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawArc(50, 50 + i * 30, 150, 20, 45, 180, false, false);
                canvas.DrawArc(250.5f, 50.5f + i * 30, 150, 20, 45, 180, false, false);
            }

            canvas.StrokeColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                canvas.StrokeSize = i;
                canvas.DrawArc(450.5f, 50.5f + i * 30, 150, 20, 45, 180, false, false);
            }
        }

        private static void DrawArcsWithAlpha(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 2;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                canvas.DrawArc(450f, 200f + i * 30, 150, 20, 180, 0, true, true);
            }

            canvas.Alpha = 1;
        }

        private static void OvalDrawArcsWithDifferentStrokeLocations(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
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

            canvas.StrokeColor = StandardColors.Black;
        }

        private static void DrawArcsWithDifferentStrokeLocations(EWCanvas canvas)
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
                canvas.DrawArc(50.5f, 500.5f + i * 40, 150, 20, 0, 180, false, false);
            }

            canvas.StrokeLocation = EWStrokeLocation.OUTSIDE;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawArc(250.5f, 500.5f + i * 40, 150, 20, 0, 180, false, false);
            }

            canvas.StrokeLocation = EWStrokeLocation.CENTER;
            for (int i = 1; i < 4; i++)
            {
                canvas.StrokeSize = i * 2 + 1;
                canvas.DrawArc(450.5f, 500.5f + i * 40, 150, 20, 0, 180, false, false);
            }
        }

        public override string ToString()
        {
            if (includeOvals)
            {
                return "DrawArcs (Including Background Ovals)";
            }

            return base.ToString();
        }
    }
}