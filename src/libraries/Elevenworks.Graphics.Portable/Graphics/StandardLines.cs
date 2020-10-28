using System.Reflection;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public static class StandardLines
    {
        private static string[] _names;
        private static float[][] _styles;

        public static readonly float[] SOLID = null;
        public static readonly float[] DOT_DOT = {1, 1};
        public static readonly float[] DOTTED = {2, 2};
        public static readonly float[] DASHED = {4, 4};
        public static readonly float[] LONG_DASHES = {8, 4};
        public static readonly float[] EXTRA_LONG_DASHES = {16, 4};
        public static readonly float[] DASHED_DOT = {4, 4, 1, 4};
        public static readonly float[] DASHED_DOT_DOT = {4, 4, 1, 4, 1, 4};
        public static readonly float[] LONG_DASHES_DOT = {8, 4, 2, 4};
        public static readonly float[] EXTRA_LONG_DASHES_DOT = {16, 4, 8, 4};

        public static string[] GetStyleNames()
        {
            if (_names == null)
            {
                GetStyles();
            }

            return _names;
        }

        public static float[][] GetStyles()
        {
            if (_styles == null)
            {
                var type = typeof(StandardLines);
                var fields = type.GetRuntimeFields();
                var styleList = new List<float[]>();
                var nameList = new List<string>();

                foreach (var field in fields)
                {
                    if (field.IsStatic)
                    {
                        if (field.GetValue(null) is float[] value)
                        {
                            styleList.Add(value);
                            nameList.Add(field.Name);
                        }
                    }
                }

                _styles = new float[styleList.Count + 1][];
                _styles[0] = null;

                _names = new string[styleList.Count + 1];
                _names[0] = "SOLID";

                for (var i = 0; i < styleList.Count; i++)
                {
                    _styles[i + 1] = styleList[i];
                    _names[i + 1] = nameList[i];
                }
            }

            return _styles;
        }

        public static string GetStyleName(float[] style)
        {
            var type = typeof(StandardLines);
            var fields = type.GetRuntimeFields();

            foreach (var field in fields)
            {
                if (field.IsStatic)
                {
                    var vValue = field.GetValue(null) as float[];
                    if (style == null && vValue == null)
                    {
                        return field.Name;
                    }

                    if (style != null && vValue != null && style.Length == vValue.Length)
                    {
                        var match = true;
                        for (var i = 0; i < style.Length && match; i++)
                        {
                            // ReSharper disable CompareOfFloatsByEqualityOperator
                            match = style[i] == vValue[i];
                        }
                        // ReSharper restore CompareOfFloatsByEqualityOperator

                        if (match)
                        {
                            return field.Name;
                        }
                    }
                }
            }

            return null;
        }

        public static int GetStyleIndex(float[] style)
        {
            var type = typeof(StandardLines);
            var fields = type.GetRuntimeFields();

            var index = 0;
            foreach (var field in fields)
            {
                if (field.IsStatic)
                {
                    var value = field.GetValue(null) as float[];
                    if (style == null && value == null)
                    {
                        return index;
                    }

                    if (style != null && value != null && style.Length == value.Length)
                    {
                        var match = true;
                        for (var i = 0; i < style.Length && match; i++)
                        {
                            // ReSharper disable CompareOfFloatsByEqualityOperator
                            match = style[i] == value[i];
                        }
                        // ReSharper restore CompareOfFloatsByEqualityOperator

                        if (match)
                        {
                            return index;
                        }
                    }

                    index++;
                }
            }

            return 0;
        }

        public static float[] GetStyleByName(string name)
        {
            var type = typeof(StandardLines);
            var field = type.GetRuntimeField(name);
            if (field != null)
            {
                return field.GetValue(null) as float[];
            }

            return null;
        }

        public static bool LineStylesAreEqual(float[] style1, float[] style2)
        {
            if (style1 == style2)
            {
                return true;
            }

            if (style1 == null && style2 != null)
            {
                return false;
            }

            if (style1 != null && style2 == null)
            {
                return false;
            }

            // ReSharper disable PossibleNullReferenceException
            if (style1.Length == style2.Length)
                // ReSharper restore PossibleNullReferenceException
            {
                for (var i = 0; i < style1.Length; i++)
                {
                    // ReSharper disable CompareOfFloatsByEqualityOperator
                    if (style1[i] != style2[i])
                    {
                        // ReSharper restore CompareOfFloatsByEqualityOperator
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}