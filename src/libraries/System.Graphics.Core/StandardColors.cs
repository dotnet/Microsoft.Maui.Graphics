using System.Linq;
using System.Reflection;

namespace System.Graphics
{
    public static class StandardColors
    {
        private static string[] _names;
        private static EWColor[] _colors;

        public static readonly EWColor Black = new EWColor("#000000");
        public static readonly EWColor Navy = new EWColor("#000080");
        public static readonly EWColor DarkBlue = new EWColor("#00008B");
        public static readonly EWColor MediumBlue = new EWColor("#0000CD");
        public static readonly EWColor Blue = new EWColor("#0000FF");
        public static readonly EWColor DarkGreen = new EWColor("#006400");
        public static readonly EWColor Green = new EWColor("#008000");
        public static readonly EWColor Teal = new EWColor("#008080");
        public static readonly EWColor DarkCyan = new EWColor("#008B8B");
        public static readonly EWColor DeepSkyBlue = new EWColor("#00BFFF");
        public static readonly EWColor DarkTurquoise = new EWColor("#00CED1");
        public static readonly EWColor MediumSpringGreen = new EWColor("#00FA9A");
        public static readonly EWColor Lime = new EWColor("#00FF00");
        public static readonly EWColor SpringGreen = new EWColor("#00FF7F");
        public static readonly EWColor Aqua = new EWColor("#00FFFF");
        public static readonly EWColor Cyan = new EWColor("#00FFFF");
        public static readonly EWColor MidnightBlue = new EWColor("#191970");
        public static readonly EWColor DodgerBlue = new EWColor("#1E90FF");
        public static readonly EWColor LightSeaGreen = new EWColor("#20B2AA");
        public static readonly EWColor ForestGreen = new EWColor("#228B22");
        public static readonly EWColor SeaGreen = new EWColor("#2E8B57");
        public static readonly EWColor DarkSlateGrey = new EWColor("#2F4F4F");
        public static readonly EWColor LimeGreen = new EWColor("#32CD32");
        public static readonly EWColor MediumSeaGreen = new EWColor("#3CB371");
        public static readonly EWColor Turquoise = new EWColor("#40E0D0");
        public static readonly EWColor RoyalBlue = new EWColor("#4169E1");
        public static readonly EWColor SteelBlue = new EWColor("#4682B4");
        public static readonly EWColor DarkSlateBlue = new EWColor("#483D8B");
        public static readonly EWColor MediumTurquoise = new EWColor("#48D1CC");
        public static readonly EWColor Indigo = new EWColor("#4B0082");
        public static readonly EWColor DarkOliveGreen = new EWColor("#556B2F");
        public static readonly EWColor CadetBlue = new EWColor("#5F9EA0");
        public static readonly EWColor CornflowerBlue = new EWColor("#6495ED");
        public static readonly EWColor MediumAquaMarine = new EWColor("#66CDAA");
        public static readonly EWColor DimGrey = new EWColor("#696969");
        public static readonly EWColor SlateBlue = new EWColor("#6A5ACD");
        public static readonly EWColor OliveDrab = new EWColor("#6B8E23");
        public static readonly EWColor SlateGrey = new EWColor("#708090");
        public static readonly EWColor LightSlateGrey = new EWColor("#778899");
        public static readonly EWColor MediumSlateBlue = new EWColor("#7B68EE");
        public static readonly EWColor LawnGreen = new EWColor("#7CFC00");
        public static readonly EWColor Chartreuse = new EWColor("#7FFF00");
        public static readonly EWColor Aquamarine = new EWColor("#7FFFD4");
        public static readonly EWColor Maroon = new EWColor("#800000");
        public static readonly EWColor Purple = new EWColor("#800080");
        public static readonly EWColor Olive = new EWColor("#808000");
        public static readonly EWColor Grey = new EWColor("#808080");
        public static readonly EWColor SkyBlue = new EWColor("#87CEEB");
        public static readonly EWColor LightSkyBlue = new EWColor("#87CEFA");
        public static readonly EWColor BlueViolet = new EWColor("#8A2BE2");
        public static readonly EWColor DarkRed = new EWColor("#8B0000");
        public static readonly EWColor DarkMagenta = new EWColor("#8B008B");
        public static readonly EWColor SaddleBrown = new EWColor("#8B4513");
        public static readonly EWColor DarkSeaGreen = new EWColor("#8FBC8F");
        public static readonly EWColor LightGreen = new EWColor("#90EE90");
        public static readonly EWColor MediumPurple = new EWColor("#9370D8");
        public static readonly EWColor DarkViolet = new EWColor("#9400D3");
        public static readonly EWColor PaleGreen = new EWColor("#98FB98");
        public static readonly EWColor DarkOrchid = new EWColor("#9932CC");
        public static readonly EWColor YellowGreen = new EWColor("#9ACD32");
        public static readonly EWColor Sienna = new EWColor("#A0522D");
        public static readonly EWColor Brown = new EWColor("#A52A2A");
        public static readonly EWColor DarkGrey = new EWColor("#A9A9A9");
        public static readonly EWColor LightBlue = new EWColor("#ADD8E6");
        public static readonly EWColor GreenYellow = new EWColor("#ADFF2F");
        public static readonly EWColor PaleTurquoise = new EWColor("#AFEEEE");
        public static readonly EWColor LightSteelBlue = new EWColor("#B0C4DE");
        public static readonly EWColor PowderBlue = new EWColor("#B0E0E6");
        public static readonly EWColor FireBrick = new EWColor("#B22222");
        public static readonly EWColor DarkGoldenRod = new EWColor("#B8860B");
        public static readonly EWColor MediumOrchid = new EWColor("#BA55D3");
        public static readonly EWColor RosyBrown = new EWColor("#BC8F8F");
        public static readonly EWColor DarkKhaki = new EWColor("#BDB76B");
        public static readonly EWColor Silver = new EWColor("#C0C0C0");
        public static readonly EWColor MediumVioletRed = new EWColor("#C71585");
        public static readonly EWColor IndianRed = new EWColor("#CD5C5C");
        public static readonly EWColor Peru = new EWColor("#CD853F");
        public static readonly EWColor Chocolate = new EWColor("#D2691E");
        public static readonly EWColor Tan = new EWColor("#D2B48C");
        public static readonly EWColor LightGrey = new EWColor("#D3D3D3");
        public static readonly EWColor PaleVioletRed = new EWColor("#D87093");
        public static readonly EWColor Thistle = new EWColor("#D8BFD8");
        public static readonly EWColor Orchid = new EWColor("#DA70D6");
        public static readonly EWColor GoldenRod = new EWColor("#DAA520");
        public static readonly EWColor Crimson = new EWColor("#DC143C");
        public static readonly EWColor Gainsboro = new EWColor("#DCDCDC");
        public static readonly EWColor Plum = new EWColor("#DDA0DD");
        public static readonly EWColor BurlyWood = new EWColor("#DEB887");
        public static readonly EWColor LightCyan = new EWColor("#E0FFFF");
        public static readonly EWColor Lavender = new EWColor("#E6E6FA");
        public static readonly EWColor DarkSalmon = new EWColor("#E9967A");
        public static readonly EWColor Violet = new EWColor("#EE82EE");
        public static readonly EWColor PaleGoldenRod = new EWColor("#EEE8AA");
        public static readonly EWColor LightCoral = new EWColor("#F08080");
        public static readonly EWColor Khaki = new EWColor("#F0E68C");
        public static readonly EWColor AliceBlue = new EWColor("#F0F8FF");
        public static readonly EWColor HoneyDew = new EWColor("#F0FFF0");
        public static readonly EWColor Azure = new EWColor("#F0FFFF");
        public static readonly EWColor SandyBrown = new EWColor("#F4A460");
        public static readonly EWColor Wheat = new EWColor("#F5DEB3");
        public static readonly EWColor Beige = new EWColor("#F5F5DC");
        public static readonly EWColor WhiteSmoke = new EWColor("#F5F5F5");
        public static readonly EWColor MintCream = new EWColor("#F5FFFA");
        public static readonly EWColor GhostWhite = new EWColor("#F8F8FF");
        public static readonly EWColor Salmon = new EWColor("#FA8072");
        public static readonly EWColor AntiqueWhite = new EWColor("#FAEBD7");
        public static readonly EWColor Linen = new EWColor("#FAF0E6");
        public static readonly EWColor LightGoldenRodYellow = new EWColor("#FAFAD2");
        public static readonly EWColor OldLace = new EWColor("#FDF5E6");
        public static readonly EWColor Red = new EWColor("#FF0000");
        public static readonly EWColor Fuchsia = new EWColor("#FF00FF");
        public static readonly EWColor Magenta = new EWColor("#FF00FF");
        public static readonly EWColor DeepPink = new EWColor("#FF1493");
        public static readonly EWColor OrangeRed = new EWColor("#FF4500");
        public static readonly EWColor Tomato = new EWColor("#FF6347");
        public static readonly EWColor HotPink = new EWColor("#FF69B4");
        public static readonly EWColor Coral = new EWColor("#FF7F50");
        public static readonly EWColor DarkOrange = new EWColor("#FF8C00");
        public static readonly EWColor LightSalmon = new EWColor("#FFA07A");
        public static readonly EWColor Orange = new EWColor("#FFA500");
        public static readonly EWColor LightPink = new EWColor("#FFB6C1");
        public static readonly EWColor Pink = new EWColor("#FFC0CB");
        public static readonly EWColor Gold = new EWColor("#FFD700");
        public static readonly EWColor PeachPuff = new EWColor("#FFDAB9");
        public static readonly EWColor NavajoWhite = new EWColor("#FFDEAD");
        public static readonly EWColor Moccasin = new EWColor("#FFE4B5");
        public static readonly EWColor Bisque = new EWColor("#FFE4C4");
        public static readonly EWColor MistyRose = new EWColor("#FFE4E1");
        public static readonly EWColor BlanchedAlmond = new EWColor("#FFEBCD");
        public static readonly EWColor PapayaWhip = new EWColor("#FFEFD5");
        public static readonly EWColor LavenderBlush = new EWColor("#FFF0F5");
        public static readonly EWColor SeaShell = new EWColor("#FFF5EE");
        public static readonly EWColor Cornsilk = new EWColor("#FFF8DC");
        public static readonly EWColor LemonChiffon = new EWColor("#FFFACD");
        public static readonly EWColor FloralWhite = new EWColor("#FFFAF0");
        public static readonly EWColor Snow = new EWColor("#FFFAFA");
        public static readonly EWColor Yellow = new EWColor("#FFFF00");
        public static readonly EWColor LightYellow = new EWColor("#FFFFE0");
        public static readonly EWColor Ivory = new EWColor("#FFFFF0");
        public static readonly EWColor White = new EWColor("#FFFFFF");
        public static readonly EWColor Transparent = new EWColor("#00000000");

        public static string[] GetColorNames()
        {
            return _names
                   ?? (_names = typeof(StandardColors)
                       .GetRuntimeFields()
                       .Where(f => f.IsStatic && f.IsPublic)
                       .Select(f => f.Name)
                       .ToArray());
        }
        
        public static EWColor[] GetAllColors()
        {
            return _colors
                   ?? (_colors = typeof(StandardColors)
                       .GetRuntimeFields()
                       .Where(f => f.IsStatic && f.IsPublic)
                       .Select(f => f.GetValue(null) as EWColor)
                       .ToArray());
        }

        public static string GetColorName(EWColor color)
        {
            var fields = typeof(StandardColors).GetRuntimeFields();

            foreach (var field in fields)
            {
                if (field.IsStatic && field.IsPublic)
                {
                    var vValue = field.GetValue(null) as EWColor;
                    if (color == null && vValue == null)
                        return field.Name;

                    if (color != null && vValue != null && color.Equals(vValue))
                        return field.Name;
                }
            }

            return null;
        }

        public static int GetColorIndex(EWColor color)
        {
            var fields = typeof(StandardColors).GetRuntimeFields();

            int index = 0;
            foreach (var field in fields)
            {
                if (field.IsStatic && field.IsPublic)
                {
                    var value = field.GetValue(null) as EWColor;
                    if (color == null && value == null)
                        return index;

                    if (color != null && value != null && color.Equals(value))
                        return index;

                    index++;
                }
            }

            return 0;
        }
        
        public static EWColor GetColorByIndex(int index)
        {
            var colors = GetAllColors();
            if (index >= 0 && index < colors.Length)
                return colors[index];
            return null;
        }

        public static EWColor GetColorByName(string name)
        {
            var fields = typeof(StandardColors).GetRuntimeFields();

            foreach (var field in fields)
                if (field.IsStatic)
                    if (field.Name.EqualsIgnoresCase(name))
                        return field.GetValue(null) as EWColor;

            return null;
        }
    }
}