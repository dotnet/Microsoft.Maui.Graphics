using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawHorizontallyCenteredTextWithSimpleApi : AbstractScenario
    {
        public DrawHorizontallyCenteredTextWithSimpleApi()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.FillColor = StandardColors.CornflowerBlue;
            canvas.FontName = "Arial";
            canvas.FontSize = 12f;

            canvas.Translate(0, 10);
            canvas.DrawString("Arial - Left", 100, 10, EWHorizontalAlignment.LEFT);
            canvas.FillCircle(100, 10, 2);

            canvas.Translate(0, 30);
            canvas.DrawString("Arial - Center", 100, 10, EWHorizontalAlignment.CENTER);
            canvas.FillCircle(100, 10, 2);

            canvas.Translate(0, 30);
            canvas.DrawString("Arial - Right", 100, 10, EWHorizontalAlignment.RIGHT);
            canvas.FillCircle(100, 10, 2);

            canvas.SetToSystemFont();

            canvas.Translate(0, 30);
            canvas.DrawString("System - Left", 100, 10, EWHorizontalAlignment.LEFT);
            canvas.FillCircle(100, 10, 2);

            canvas.Translate(0, 30);
            canvas.DrawString("System - Center", 100, 10, EWHorizontalAlignment.CENTER);
            canvas.FillCircle(100, 10, 2);

            canvas.Translate(0, 30);
            canvas.DrawString("System - Right", 100, 10, EWHorizontalAlignment.RIGHT);
            canvas.FillCircle(100, 10, 2);

            canvas.RestoreState();
        }
    }
}