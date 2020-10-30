using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class LineJoins : AbstractScenario
    {
        public LineJoins() : base(720, 1024)
        {
        }

        public override void Draw(ICanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeSize = 10;
            canvas.Translate(-300, -60);

            var path = new EWPath();
            path.MoveTo(350, 120);
            path.LineTo(370, 180);
            path.LineTo(390, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.MITER;
            path = new EWPath();
            path.MoveTo(400, 120);
            path.LineTo(420, 180);
            path.LineTo(440, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.ROUND;
            path = new EWPath();
            path.MoveTo(450, 120);
            path.LineTo(470, 180);
            path.LineTo(490, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.BEVEL;
            path = new EWPath();
            path.MoveTo(500, 120);
            path.LineTo(520, 180);
            path.LineTo(540, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.MITER;
            canvas.MiterLimit = 2;
            path = new EWPath();
            path.MoveTo(550, 120);
            path.LineTo(570, 180);
            path.LineTo(590, 120);
            canvas.DrawPath(path);

            canvas.RestoreState();
        }
    }
}