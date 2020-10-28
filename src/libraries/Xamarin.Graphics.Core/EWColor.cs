using System;
using System.Globalization;
using System.IO;

namespace Xamarin.Graphics
{
    public class EWColor
    {
        private const int IndexRed = 0;
        private const int IndexGreen = 1;
        private const int IndexBlue = 2;
        private const int IndexAlpha = 3;
        
        public static readonly EWColor TRANSPARENT_COLOR = new EWColor(0f, 0f, 0f, 0f);
        public static readonly EWColor BARELY_TRANSPARENT_COLOR = new EWColor(1f, 1f, 1f, .001f);

        private readonly float[] _components;

        public EWColor(string colorAsHex)
        {
            //Remove # if present
            if (colorAsHex.IndexOf('#') != -1)
            {
                colorAsHex = colorAsHex.Replace("#", "");
            }

            int vRed = 0;
            int vGreen = 0;
            int vBlue = 0;
            int vAlpha = 255;

            if (colorAsHex.Length == 6)
            {
                //#RRGGBB
                vRed = int.Parse(colorAsHex.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vGreen = int.Parse(colorAsHex.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vBlue = int.Parse(colorAsHex.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            }
            else if (colorAsHex.Length == 3)
            {
                //#RGB
                vRed = int.Parse(colorAsHex[0] + colorAsHex[0].ToInvariantString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vGreen = int.Parse(colorAsHex[1] + colorAsHex[1].ToInvariantString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vBlue = int.Parse(colorAsHex[2] + colorAsHex[2].ToInvariantString(), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            }
            else if (colorAsHex.Length == 8)
            {
                //#RRGGBBAA
                vRed = int.Parse(colorAsHex.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vGreen = int.Parse(colorAsHex.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vBlue = int.Parse(colorAsHex.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
                vAlpha = int.Parse(colorAsHex.Substring(6, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            }

            _components = new float[4];
            _components[IndexRed] = vRed / 255f;
            _components[IndexGreen] = vGreen / 255f;
            _components[IndexBlue] = vBlue / 255f;
            _components[IndexAlpha] = vAlpha / 255f;
        }

        public EWColor(float grayScale, float alpha = 1)
        {
            _components = new float[4];

            _components[0] = grayScale;
            _components[1] = grayScale;
            _components[2] = grayScale;
            _components[3] = alpha;
        }
        
        public EWColor(float[] components)
        {
            _components = new float[4];

            _components[0] = components[0];
            _components[1] = components[1];
            _components[2] = components[2];

            if (components.Length > 3)
            {
                _components[3] = components[3];
            }
            else
            {
                _components[3] = 1;
            }
        }

        public EWColor(int red, int green, int blue, int alpha)
        {
            _components = new float[4];
            _components[IndexRed] = red / 255f;
            _components[IndexGreen] = green / 255f;
            _components[IndexBlue] = blue / 255f;
            _components[IndexAlpha] = alpha / 255f;
        }

        public EWColor(int red, int green, int blue) : this(red, green, blue, 255)
        {
        }

        public EWColor(float red, float green, float blue) : this(red, green, blue, 1)
        {
        }

        public EWColor(float red, float green, float blue, float alpha)
        {
            _components = new float[4];
            _components[IndexRed] = red;
            _components[IndexGreen] = green;
            _components[IndexBlue] = blue;
            _components[IndexAlpha] = alpha;
        }

        public EWColor(EWColor color) : this(color.Red, color.Green, color.Blue, color.Alpha)
        {
        }

        public float Red => _components[IndexRed];

        public float Blue => _components[IndexBlue];

        public float Green => _components[IndexGreen];

        public float Alpha => _components[IndexAlpha];

        public float[] Components => _components;

        public EWColor GetTransparentCopy(float alpha)
        {
            if (Math.Abs(alpha - Alpha) < Geometry.Epsilon)
            {
                return this;
            }

            var components = new float[4];
            components[IndexRed] = _components[IndexRed];
            components[IndexGreen] = _components[IndexGreen];
            components[IndexBlue] = _components[IndexBlue];
            components[IndexAlpha] = alpha;
            return new EWColor(components);
        }

        public override bool Equals(object obj)
        {
            if (obj is EWColor compareTo)
            {
                return Math.Abs(compareTo.Red - Red) < Geometry.Epsilon && Math.Abs(compareTo.Blue - Blue) < Geometry.Epsilon && Math.Abs(compareTo.Green - Green) < Geometry.Epsilon &&
                       Math.Abs(compareTo.Alpha - Alpha) < Geometry.Epsilon;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return ((int) Red ^ (int) Blue) ^ ((int) Green ^ (int) Alpha);
        }

        public string ToHexString()
        {
            return "#" + ToHexString(Red) + ToHexString(Green) + ToHexString(Blue);
        }

        public string ToHexStringIncludingAlpha()
        {
            if (Math.Abs(Alpha - 1) > Geometry.Epsilon)
            {
                return ToHexString() + ToHexString(Alpha);
            }

            return ToHexString();
        }

        public static string ToHexString(float r, float g, float b)
        {
            return "#" + ToHexString(r) + ToHexString(g) + ToHexString(b);
        }

        public static string ToHexString(float r, float g, float b, float a)
        {
            return "#" + ToHexString(r) + ToHexString(g) + ToHexString(b) + ToHexString(a);
        }

        private static string ToHexString(float aValue)
        {
            var vValue = (int) (255f * aValue);
            string vStringValue = vValue.ToString("X");
            if (vStringValue.Length == 1)
            {
                return "0" + vStringValue;
            }

            return vStringValue;
        }

        public override string ToString()
        {
            return $"[EWColor: Red={Red}, Green={Green}, Blue={Blue}, Alpha={Alpha}]";
        }

        public void ToHSL(out float h, out float s, out float l)
        {
            float r = _components[IndexRed] / 255.0f;
            float g = _components[IndexGreen] / 255.0f;
            float b = _components[IndexBlue] / 255.0f;

            h = 0;
            s = 0;

            float v = Math.Max(r, g);
            v = Math.Max(v, b);
            float m = Math.Min(r, g);
            m = Math.Min(m, b);
            l = (m + v) / 2.0f;
            if (l <= 0.0)
            {
                return;
            }

            float vm = v - m;
            s = vm;
            if (s > 0.0)
            {
                s /= (l <= 0.5f) ? (v + m) : (2.0f - v - m);
            }

            else
            {
                return;
            }

            float r2 = (v - r) / vm;
            float g2 = (v - g) / vm;
            float b2 = (v - b) / vm;
            if (Math.Abs(r - v) < Geometry.Epsilon)
            {
                h = (Math.Abs(g - m) < Geometry.Epsilon ? 5.0f + b2 : 1.0f - g2);
            }

            else if (Math.Abs(g - v) < Geometry.Epsilon)
            {
                h = (Math.Abs(b - m) < Geometry.Epsilon ? 1.0f + r2 : 3.0f - b2);
            }
            else
            {
                h = (Math.Abs(r - m) < Geometry.Epsilon ? 3.0f + g2 : 5.0f - r2);
            }

            h /= 6.0f;
        }

        public void FromHSL(float[] aValues)
        {
            float h = aValues[0];
            float s = aValues[1];
            float l = aValues[2];
            float r;
            float g;
            float b;

            if (Math.Abs(l - 0) < Geometry.Epsilon)
            {
                r = g = b = 0;
            }
            else
            {
                if (Math.Abs(s - 0) < Geometry.Epsilon)
                {
                    r = g = b = l;
                }
                else
                {
                    float temp2 = ((l <= 0.5f) ? l * (1.0f + s) : l + s - (l * s));
                    float temp1 = 2.0f * l - temp2;

                    var t3 = new[] {h + 1.0f / 3.0f, h, h - 1.0f / 3.0f};
                    var clr = new float[] {0, 0, 0};
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                        {
                            t3[i] += 1.0f;
                        }

                        if (t3[i] > 1)
                        {
                            t3[i] -= 1.0f;
                        }

                        if (6.0 * t3[i] < 1.0f)
                        {
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0f;
                        }
                        else if (2.0f * t3[i] < 1.0f)
                        {
                            clr[i] = temp2;
                        }
                        else if (3.0f * t3[i] < 2.0f)
                        {
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0f / 3.0f) - t3[i]) * 6.0f);
                        }
                        else
                        {
                            clr[i] = temp1;
                        }
                    }

                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            _components[IndexRed] = r;
            _components[IndexGreen] = g;
            _components[IndexBlue] = b;
        }

        public EWColor FromHSLA(float h, float s, float l, float a = 1)
        {
            float r;
            float g;
            float b;

            if (Math.Abs(l - 0) < Geometry.Epsilon)
            {
                r = g = b = 0;
            }
            else
            {
                if (Math.Abs(s - 0) < Geometry.Epsilon)
                {
                    r = g = b = l;
                }
                else
                {
                    float temp2 = ((l <= 0.5f) ? l * (1.0f + s) : l + s - (l * s));
                    float temp1 = 2.0f * l - temp2;

                    var t3 = new[] {h + 1.0f / 3.0f, h, h - 1.0f / 3.0f};
                    var clr = new float[] {0, 0, 0};
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                        {
                            t3[i] += 1.0f;
                        }

                        if (t3[i] > 1)
                        {
                            t3[i] -= 1.0f;
                        }

                        if (6.0 * t3[i] < 1.0f)
                        {
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0f;
                        }
                        else if (2.0f * t3[i] < 1.0f)
                        {
                            clr[i] = temp2;
                        }
                        else if (3.0f * t3[i] < 2.0f)
                        {
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0f / 3.0f) - t3[i]) * 6.0f);
                        }
                        else
                        {
                            clr[i] = temp1;
                        }
                    }

                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }

            return new EWColor(r, g, b, a);
        }

        public EWPaint AsPaint()
        {
            var paint = new EWPaint
            {
                PaintType = EWPaintType.SOLID, 
                StartColor = this
            };
            return paint;
        }

        public string ToParsableString()
        {
            if (Alpha == 1)
            {
                return ToHexString();
            }

            var writer = new StringWriter();
            writer.Write(ToHexString());
            writer.Write(",");
            writer.Write(Alpha.ToString(CultureInfo.InvariantCulture));
            return writer.ToString();
        }

        public static EWColor Parse(string aValue)
        {
            try
            {
                var values = aValue.Split(new[] {','});

                var color = new EWColor(values[0]);
                if (values.Length > 1)
                {
                    float alpha = float.Parse(values[1], CultureInfo.InvariantCulture);
                    if (!(Math.Abs(alpha - 1) < Geometry.Epsilon))
                    {
                        color = color.GetTransparentCopy(alpha);
                    }
                }

                return color;
            }
            catch (Exception exc)
            {
#if DEBUG
                Logger.Debug(exc);
#endif
                return StandardColors.White;
            }
        }

        public static EWColor Parse(string aValue, EWColor defaultColor)
        {
            try
            {
                var values = aValue.Split(new[] {','});

                var color = new EWColor(values[0]);
                if (values.Length > 1)
                {
                    float alpha = float.Parse(values[1], CultureInfo.InvariantCulture);
                    if (!(Math.Abs(alpha - 1) < Geometry.Epsilon))
                    {
                        color = color.GetTransparentCopy(alpha);
                    }
                }

                return color;
            }
            catch (Exception)
            {
                return defaultColor;
            }
        }
        
        public EWColor Lighten()
        {
            ToHSL(out var h, out var s, out var b);
            return FromHSLA(h, s, (float) Math.Min(b * 1.3, 1.0), Alpha);
        }

        public EWColor Darken()
        {
            ToHSL(out var h, out var s, out var b);
            return FromHSLA(h, s, b * .75f, Alpha);
        }
    }
}