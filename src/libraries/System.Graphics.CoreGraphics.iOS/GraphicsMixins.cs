using UIKit;

namespace System.Graphics.CoreGraphics
{
    public static class GraphicsMixins
    {
        public static UIColor AsUIColor(this EWColor color)
        {
            if (color != null)
            {
                return UIColor.FromRGBA(color.Red, color.Green, color.Blue, color.Alpha);
            }

            return UIColor.White;
        }

        public static EWColor AsEWColor(this UIColor color)
        {
            if (color != null)
            {
                color.GetRGBA(out var red, out var green, out var blue, out var alpha);
                return new EWColor((float) red, (float) green, (float) blue, (float) alpha);
            }

            return new EWColor(1f, 1f, 1f, 1f); // White
        }


        public static UIBezierPath AsUIBezierPath(this EWPath target)
        {
            if (target == null)
                return null;

            return UIBezierPath.FromPath(target.AsCGPath());
        }
    }
}