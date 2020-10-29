namespace Xamarin.Graphics
{
    public interface EWPattern
    {
        float Width { get; }
        float Height { get; }
        float StepX { get; }
        float StepY { get; }
        void Draw(EWCanvas canvas);
    }

    public static class PatternExtensions
    {
        public static EWPaint AsPaint(this EWPattern target)
        {
            return AsPaint(target, StandardColors.Black);
        }

        public static EWPaint AsPaint(this EWPattern target, EWColor foregroundColor)
        {
            if (target != null)
            {
                var paint = new EWPaint
                {
                    Pattern = target,
                    ForegroundColor = foregroundColor,
                    BackgroundColor = null
                };
                return paint;
            }

            return null;
        }

        public static void SetFillPattern(this EWCanvas target, EWPattern pattern)
        {
            SetFillPattern(target, pattern, StandardColors.Black);
        }

        public static void SetFillPattern(
            this EWCanvas target,
            EWPattern pattern,
            EWColor foregroundColor)
        {
            if (target != null)
            {
                if (pattern != null)
                {
                    var paint = pattern.AsPaint(foregroundColor);
                    target.SetFillPaint(paint, 0, 0, 0, 0);
                }
                else
                {
                    target.FillColor = StandardColors.White;
                }
            }
        }
    }
}