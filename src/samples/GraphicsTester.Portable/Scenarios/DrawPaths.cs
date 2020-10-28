using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawPaths : AbstractScenario
    {
        public DrawPaths() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            DrawLineSegment(canvas);
            DrawQuadraticSegment(canvas);
            DrawCubicSegment(canvas);
            DrawArcSegment(canvas);
            DrawPie(canvas);
            DrawCubicAndQuad(canvas);
            DrawSubPaths(canvas);
        }

        private void DrawLineSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(50.5f, 50.5f, 50, 50);
            var path = new EWPath(50.5f, 50.5f);
            path.LineTo(100.5f, 100.5f);
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawPath(path);
        }

        private void DrawQuadraticSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(150.5f, 50.5f, 50, 50);
            var path = new EWPath(150.5f, 50.5f);
            path.QuadTo(150.5f, 100.5f, 200.5f, 100.5f);
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawPath(path);
        }

        private void DrawCubicSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(250.5f, 50.5f, 50, 50);
            var path = new EWPath(250.5f, 50.5f);
            path.CurveTo(250.5f, 100.5f, 300.5f, 50.5f, 300.5f, 100.5f);
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawPath(path);
        }

        private void DrawArcSegment(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(350.5f, 50.5f, 50, 50);
            var path = new EWPath();
            path.AddArc(350.5f, 50.5f, 400.5f, 100.5f, 45f, 135, false);
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawPath(path);
        }

        private void DrawPie(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(350.5f, 150.5f, 50, 50);
            var path = new EWPath();
            path.AddArc(350.5f, 150.5f, 400.5f, 200.5f, 45f, 135, false);
            path.LineTo(375.5f, 200.5f);
            path.Close();
            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeDashPattern = DOTTED;
            canvas.DrawPath(path);
            canvas.StrokeDashPattern = null;
        }

        private void DrawCubicAndQuad(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(250.5f, 150.5f, 50, 50);
            var path = new EWPath(250.5f, 150.5f);
            path.CurveTo(250.5f, 200.5f, 300.5f, 150.5f, 300.5f, 200.5f);
            path.QuadTo(300.5f, 150.5f, 250.5f, 150.5f);
            path.Close();
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawPath(path);
        }

        private void DrawSubPaths(EWCanvas canvas)
        {
            canvas.StrokeColor = StandardColors.LightGrey;
            canvas.DrawRectangle(150.5f, 150.5f, 50, 50);
            var path = new EWPath();
            path.AppendRectangle(175.5f, 150.5f, 25, 50);
            path.AppendOval(175.5f, 150.5f, 25, 50);
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawPath(path);
        }
    }
}