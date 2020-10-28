﻿using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class ArcDirection : AbstractScenario
    {
        public readonly bool includeOvals;

        public ArcDirection(bool includeOvals = false) : base(720, 1024)
        {
            this.includeOvals = includeOvals;
        }

        private void DrawArc(EWCanvas canvas, float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            if (includeOvals)
            {
                canvas.StrokeColor = StandardColors.LightGrey;
                canvas.DrawOval(x, y, width, height);
            }

            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawArc(x, y, width, height, startAngle, endAngle, clockwise, closed);

            var path = new EWPath();
            path.AddArc(x, y + 400, x + width, y + 400 + width, startAngle, endAngle, clockwise);
            canvas.DrawPath(path);
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            DrawArc(canvas, 100, 100, 80, 80, 45, 300, true, false);
            DrawArc(canvas, 200, 100, 80, 80, 45, 300, false, false);
            DrawArc(canvas, 300, 100, 80, 80, -315, 300, true, false);
            DrawArc(canvas, 400, 100, 80, 80, -315, 300, false, false);

            DrawArc(canvas, 100, 200, 80, 80, 45, -60, true, false);
            DrawArc(canvas, 200, 200, 80, 80, 45, -60, false, false);
            DrawArc(canvas, 300, 200, 80, 80, -315, -60, true, false);
            DrawArc(canvas, 400, 200, 80, 80, -315, -60, false, false);

            DrawArc(canvas, 100, 300, 80, 80, 270, 45, true, false);
            DrawArc(canvas, 200, 300, 80, 80, 270, 45, false, false);
            DrawArc(canvas, 300, 300, 80, 80, -90, 45, true, false);
            DrawArc(canvas, 400, 300, 80, 80, -90, 45, false, false);

            canvas.RestoreState();
        }

        public override string ToString()
        {
            if (includeOvals)
            {
                return "ArcDirection (Including Background Ovals)";
            }

            return base.ToString();
        }
    }
}