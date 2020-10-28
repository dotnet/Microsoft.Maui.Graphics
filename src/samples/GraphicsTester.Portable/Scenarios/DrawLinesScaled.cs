using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawLinesScaled : AbstractScenario
    {
        public DrawLinesScaled() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.StrokeSize = 1;
            canvas.StrokeDashPattern = StandardLines.DOT_DOT;
            canvas.DrawLine(50, 20f, 200, 20f);

            canvas.StrokeSize = 1;
            canvas.StrokeDashPattern = StandardLines.SOLID;
            canvas.DrawLine(50, 30f, 200, 30f);

            canvas.SaveState();

            canvas.Scale(2, 2);

            canvas.StrokeSize = 1;
            canvas.StrokeDashPattern = StandardLines.DOT_DOT;
            canvas.DrawLine(50, 20f, 200, 20f);

            canvas.StrokeSize = 1;
            canvas.StrokeDashPattern = StandardLines.SOLID;
            canvas.DrawLine(50, 30f, 200, 30f);

            canvas.RestoreState();
        }
    }
}