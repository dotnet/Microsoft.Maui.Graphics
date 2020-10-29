using UIKit;
using Foundation;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public static class UIViewExtensions
    {
        public static EWPoint[] GetPointsInView(this UIView target, UIEvent touchEvent)
        {
            var touchSet = touchEvent.TouchesForView(target);
            if (touchSet == null || touchSet.Count == 0)
            {
                return new EWPoint[0];
            }

            var touches = touchSet.ToArray<UITouch>();
            var points = new EWPoint[touches.Length];
            for (int i = 0; i < touches.Length; i++)
            {
                var touch = touches[i];
                var point = touch.LocationInView(target);
                points[i] = new EWPoint((float) point.X, (float) point.Y);
            }

            return points;
        }

        public static EWPoint[] GetPointsInView(this UIView target, NSSet touchSet)
        {
            var touches = touchSet.ToArray<UITouch>();
            var points = new EWPoint[touches.Length];
            for (int i = 0; i < touches.Length; i++)
            {
                var touch = touches[i];
                var point = touch.LocationInView(target);
                points[i] = new EWPoint((float) point.X, (float) point.Y);
            }

            return points;
        }
    }
}