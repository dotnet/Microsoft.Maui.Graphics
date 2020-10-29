using Android.Graphics;

namespace Elevenworks.Graphics
{
    public class MDPath : Path
    {
        public override void ArcTo(RectF oval, float startAngle, float sweepAngle)
        {
            base.ArcTo(oval, startAngle, sweepAngle);
        }

        public override void ArcTo(RectF oval, float startAngle, float sweepAngle, bool forceMoveTo)
        {
            base.ArcTo(oval, startAngle, sweepAngle, forceMoveTo);
        }

        public override void Close()
        {
            base.Close();
        }

        public override void LineTo(float x, float y)
        {
            base.LineTo(x, y);
        }

        public override void MoveTo(float x, float y)
        {
            base.MoveTo(x, y);
        }

        public override void QuadTo(float x1, float y1, float x2, float y2)
        {
            base.QuadTo(x1, y1, x2, y2);
        }

        public override void CubicTo(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            base.CubicTo(x1, y1, x2, y2, x3, y3);
        }

        public override void RQuadTo(float dx1, float dy1, float dx2, float dy2)
        {
            base.RQuadTo(dx1, dy1, dx2, dy2);
        }

        public override void RCubicTo(float x1, float y1, float x2, float y2, float x3, float y3)
        {
            base.RCubicTo(x1, y1, x2, y2, x3, y3);
        }

        public override void RLineTo(float dx, float dy)
        {
            base.RLineTo(dx, dy);
        }

        public override void RMoveTo(float dx, float dy)
        {
            base.RMoveTo(dx, dy);
        }
    }
}