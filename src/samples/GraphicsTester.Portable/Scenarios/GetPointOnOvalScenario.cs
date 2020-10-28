using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class GetPointOnOvalScenario : AbstractScenario
    {
        public GetPointOnOvalScenario() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.DrawCircle(200, 200, 100);

            for (int i = 0; i < 360; i += 45)
            {
                var point = Geometry.GetPointOnOval(100, 100, 200, 200, (float) i);
                canvas.FillColor = StandardColors.CornflowerBlue;
                canvas.FillCircle(point, 2);
                canvas.DrawString(i.ToString(), point.X + 10, point.Y - 10, EWHorizontalAlignment.LEFT);
            }

            canvas.DrawArc(400, 100, 200, 200, 0, 90, false, false);
        }
    }
}