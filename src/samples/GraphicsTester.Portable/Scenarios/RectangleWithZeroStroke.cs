using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class RectangleWithZeroStroke : AbstractScenario
    {
        public RectangleWithZeroStroke() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.FillColor = StandardColors.CornflowerBlue;
            canvas.FillRectangle(50, 50, 100, 100);

            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 0;
            canvas.DrawRectangle(50, 50, 100, 100);

            canvas.RestoreState();
        }
    }
}