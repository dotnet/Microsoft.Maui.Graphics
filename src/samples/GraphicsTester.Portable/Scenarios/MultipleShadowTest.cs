using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class MultipleShadowTest : AbstractScenario
    {
        public MultipleShadowTest() : base(20, 20)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.SetShadow(
                EWCanvas.DefaultShadowOffset,
                EWCanvas.DefaultShadowBlur,
                EWCanvas.DefaultShadowColor,
                1);

            canvas.FillColor = StandardColors.Blue;
            var path = new EWPath(10, 10);
            path.LineTo(30, 10);
            path.LineTo(20, 30);
            path.Close();
            canvas.FillPath(path);

            canvas.FillColor = StandardColors.Salmon;
            canvas.FillRectangle(100, 100, 100, 100);

            canvas.FillColor = StandardColors.Beige;
            canvas.FillOval(100, 300, 100, 50);

            canvas.RestoreState();
        }
    }
}