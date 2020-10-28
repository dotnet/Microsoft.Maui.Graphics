namespace Xamarin.Graphics
{
    public class PaintPattern : EWPattern
    {
        public EWPattern Wrapped { get; }
        public EWPaint Paint { get; set; }

        public float Width => Wrapped?.Width ?? 0;
        public float Height => Wrapped?.Height ?? 0;
        public float StepX => Wrapped?.StepX ?? 0;
        public float StepY => Wrapped?.StepY ?? 0;

        public PaintPattern(EWPattern pattern)
        {
            Wrapped = pattern;
        }

        public void Draw(EWCanvas canvas)
        {
            if (Paint != null)
            {
                if (Paint.BackgroundColor != null && Paint.BackgroundColor.Alpha > 1)
                {
                    canvas.FillColor = Paint.BackgroundColor;
                    canvas.FillRectangle(0, 0, Width, Height);
                }

                canvas.StrokeColor = Paint.ForegroundColor;
                canvas.FillColor = Paint.ForegroundColor;
            }
            else
            {
                canvas.StrokeColor = StandardColors.Black;
                canvas.FillColor = StandardColors.Black;
            }

            Wrapped.Draw(canvas);
        }
    }
}