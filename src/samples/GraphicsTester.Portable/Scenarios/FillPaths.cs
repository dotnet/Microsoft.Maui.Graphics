using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class FillPaths : AbstractScenario
    {
        public FillPaths() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            FillQuadraticSegment(canvas);
            FillCubicSegment(canvas);
            FillArcSegment(canvas);
            FillPie(canvas);
            FillCubicAndQuad(canvas);
            FillSubPaths(canvas);
        }

        private void FillQuadraticSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(150.5f, 50.5f, 50, 50);
            var path = new EWPath(150.5f, 50.5f);
            path.QuadTo(150.5f, 100.5f, 200.5f, 100.5f);
            canvas.FillColor = StandardColors.Black;
            canvas.FillPath(path);
        }

        private void FillCubicSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(250.5f, 50.5f, 50, 50);
            var path = new EWPath(250.5f, 50.5f);
            path.CurveTo(250.5f, 100.5f, 300.5f, 50.5f, 300.5f, 100.5f);
            canvas.FillColor = StandardColors.Black;
            canvas.FillPath(path);
        }

        private void FillArcSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(350.5f, 50.5f, 50, 50);
            var path = new EWPath();
            path.AddArc(350.5f, 50.5f, 400.5f, 100.5f, 45f, 135, false);
            canvas.FillColor = StandardColors.Black;
            canvas.FillPath(path);
        }

        private void FillPie(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(350.5f, 150.5f, 50, 50);
            var path = new EWPath();
            path.AddArc(350.5f, 150.5f, 400.5f, 200.5f, 45f, 135, false);
            path.LineTo(375.5f, 200.5f);
            path.Close();
            canvas.FillColor = StandardColors.Black;
            canvas.FillPath(path);
        }

        private void FillCubicAndQuad(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(250.5f, 150.5f, 50, 50);
            var path = new EWPath(250.5f, 150.5f);
            path.CurveTo(250.5f, 200.5f, 300.5f, 150.5f, 300.5f, 200.5f);
            path.QuadTo(300.5f, 150.5f, 250.5f, 150.5f);
            path.Close();
            canvas.FillColor = StandardColors.Black;
            canvas.FillPath(path);
        }

        private void FillSubPaths(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(150.5f, 150.5f, 50, 50);
            var path = new EWPath();
            path.AppendRectangle(175.5f, 150.5f, 25, 50);
            path.AppendOval(175.5f, 150.5f, 25, 50);
            canvas.FillColor = StandardColors.Black;
            canvas.FillPath(path);
        }
    }
}