using System;
using UIKit;

namespace Elevenworks.Text
{
    public static class UIColorExtensions
    {
        public static string ToHex(this UIColor color)
        {
            if (color == null)
                return null;

            nfloat red, green, blue, alpha;
            color.GetRGBA(out red, out green, out blue, out alpha);

            if (alpha < 1)
                return "#" + ToHexString(red) + ToHexString(green) + ToHexString(blue) + ToHexString(alpha);

            return "#" + ToHexString(red) + ToHexString(green) + ToHexString(blue);
        }

        private static string ToHexString(nfloat value)
        {
            var intValue = (int) (255f * value);
            var stringValue = intValue.ToString("X");
            if (stringValue.Length == 1)
                return "0" + stringValue;

            return stringValue;
        }
    }
}