using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class TransformRotateFromOrigin : AbstractScenario
    {
        public TransformRotateFromOrigin()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(100, 100, 100, 100);
            canvas.StrokeColor = StandardColors.Black;
            canvas.Rotate(10);
            canvas.DrawRectangle(100, 100, 100, 100);
            canvas.StrokeColor = StandardColors.Salmon;
            canvas.Rotate(10);
            canvas.DrawRectangle(100, 100, 100, 100);
            canvas.StrokeColor = StandardColors.CornflowerBlue;
            canvas.Rotate(10);
            canvas.DrawRectangle(100, 100, 100, 100);
            canvas.RestoreState();

            canvas.StrokeColor = StandardColors.Blue;
            var point = new EWPoint(65, 65);
            for (int i = -3; i < 3; i++)
            {
                var rotated = Geometry.RotatePoint(point, -15 * i);
                canvas.DrawLine(rotated.X - 10, rotated.Y, rotated.X + 10, rotated.Y);
                canvas.DrawLine(rotated.X, rotated.Y - 10, rotated.X, rotated.Y + 10);
            }

            canvas.SaveState();
            canvas.FillColor = StandardColors.Black;
            canvas.FillOval(60, 60, 10, 10);
            canvas.SetShadow(new EWSize(2, 0), 2, StandardColors.Black, 1);
            canvas.StrokeColor = StandardColors.CornflowerBlue;
            canvas.Rotate(15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.Rotate(15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.Rotate(15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.StrokeColor = StandardColors.DarkSeaGreen;
            canvas.Rotate(-60);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.Rotate(-15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.RestoreState();
        }
    }
}