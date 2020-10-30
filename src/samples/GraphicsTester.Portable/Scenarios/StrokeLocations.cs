using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class StrokeLocations : AbstractScenario
    {
        public StrokeLocations()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 10;
            canvas.Translate(0, -350);

            //
            // EWStrokeLocation.CENTER
            //
            canvas.StrokeLocation = EWStrokeLocation.CENTER;
            canvas.StrokeSize = 10;
            canvas.DrawRectangle(50, 400, 100, 50);
            canvas.DrawOval(200, 400, 100, 50);
            canvas.DrawRoundedRectangle(350, 400, 100, 50, 25);

            var path = new EWPath();
            path.MoveTo(550, 400);
            path.LineTo(500, 450);
            path.LineTo(600, 450);
            path.Close();
            canvas.DrawPath(path);

            //
            // EWStrokeLocation.INSIDE
            //
            canvas.StrokeLocation = EWStrokeLocation.INSIDE;
            canvas.DrawRectangle(50, 500, 100, 50);
            canvas.DrawOval(200, 500, 100, 50);
            canvas.DrawRoundedRectangle(350, 500, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 500);
            path.LineTo(500, 550);
            path.LineTo(600, 550);
            path.Close();
            canvas.DrawPath(path);

            //
            // EWStrokeLocation.OUTSIDE
            //
            canvas.StrokeLocation = EWStrokeLocation.OUTSIDE;
            canvas.DrawRectangle(50, 600, 100, 50);
            canvas.DrawOval(200, 600, 100, 50);
            canvas.DrawRoundedRectangle(350, 600, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 600);
            path.LineTo(500, 650);
            path.LineTo(600, 650);
            path.Close();
            canvas.DrawPath(path);

            canvas.RestoreState();
        }
    }
}