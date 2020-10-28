using System;

namespace Elevenworks.Graphics
{
    public static class ColorUtils
    {
        public static void HsvToRgb(float h, float s, float v, float[] aColor)
        {
            if (h == 360)
            {
                h = 0;
            }

            if (Math.Abs(s - 0) < Geometry.Epsilon)
            {
                aColor[0] = v;
                aColor[1] = v;
                aColor[2] = v;
                return;
            }

            float r, g, b;

            h /= 60;
            var i = (int) Math.Floor(h);
            float f = h - i;
            float p = v * (1 - s);
            float q = v * (1 - s * f);
            float t = v * (1 - s * (1 - f));

            switch (i)
            {
                case 0:
                    r = v;
                    g = t;
                    b = p;
                    break;
                case 1:
                    r = q;
                    g = v;
                    b = p;
                    break;
                case 2:
                    r = p;
                    g = v;
                    b = t;
                    break;
                case 3:
                    r = p;
                    g = q;
                    b = v;
                    break;
                case 4:
                    r = t;
                    g = p;
                    b = v;
                    break;
                default: // case 5:
                    r = v;
                    g = p;
                    b = q;
                    break;
            }

            aColor[0] = r;
            aColor[1] = g;
            aColor[2] = b;
        }

        public static void RgbToHsv(float r, float g, float b, float[] hsv)
        {
            float h = 0;
            float s;

            float max = (r > g) ? r : g;
            max = (max > b) ? max : b;

            float min = (r < g) ? r : g;
            min = (min < b) ? min : b;

            float v = max;
            // this is the value v
            // Calculate the saturation s
            if (Math.Abs(max - 0) < Geometry.Epsilon)
            {
                s = 0;
                h = 0;
                // h => UNDEFINED
            }

            else
            {
                // Chromatic case: Saturation is not 0, determine hue
                float delta = max - min;
                s = delta / max;

                if (Math.Abs(r - max) < Geometry.Epsilon)
                {
                    // resulting color is between yellow and magenta
                    h = (g - b) / delta;
                }

                else if (Math.Abs(g - max) < Geometry.Epsilon)
                {
                    // resulting color is between cyan and yellow
                    h = 2 + ((b - r) / delta);
                }
                else if (Math.Abs(b - max) < Geometry.Epsilon)
                {
                    // resulting color is between magenta and cyan
                    h = 4 + ((r - g) / delta);
                }

                // convert hue to degrees and make sure it is non-negative
                h *= 60;

                if (h < 0)
                {
                    h += 360;
                }
            }

            if (h < 0 || float.IsNegativeInfinity(h) || float.IsNaN(h))
            {
                h = 0;
            }

            // now assign everything....
            hsv[0] = h;
            hsv[1] = s;
            hsv[2] = v;
        }
    }
}