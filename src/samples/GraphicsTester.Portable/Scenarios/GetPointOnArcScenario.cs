using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class GetPointOnArcScenario : AbstractScenario
    {
        public GetPointOnArcScenario() : base(720, 1024)
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

            canvas.StrokeColor = StandardColors.Grey;
            canvas.DrawLine(500, 100, 500, 300);
            canvas.DrawLine(400, 200, 600, 200);

            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawArc(400, 100, 200, 200, 10, 45, false, false);

            var point1 = Geometry.GetPointOnArc(400, 100, 200, 200, 10, 45, false, 0);
            var point2 = Geometry.GetPointOnArc(400, 100, 200, 200, 10, 45, false, .5f);
            var point3 = Geometry.GetPointOnArc(400, 100, 200, 200, 10, 45, false, 1);

            canvas.FillCircle(point1, 2);
            canvas.FillCircle(point2, 2);
            canvas.FillCircle(point3, 2);

            canvas.DrawString("0", point1.X + 10, point1.Y - 10, EWHorizontalAlignment.LEFT);
            canvas.DrawString(".5", point2.X + 10, point2.Y - 10, EWHorizontalAlignment.LEFT);
            canvas.DrawString("1", point3.X + 10, point3.Y - 10, EWHorizontalAlignment.LEFT);
        }
    }
}