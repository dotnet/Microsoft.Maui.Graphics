namespace System.Graphics
{
    public static class PatternExtensions
    {
        public static EWPaint AsPaint(this IPattern target)
        {
            return AsPaint(target, Colors.Black);
        }

        public static EWPaint AsPaint(this IPattern target, Color foregroundColor)
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
    }
}