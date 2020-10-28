using System;
using System.Globalization;

namespace Elevenworks
{
    public static class InvarientExtensions
    {
        public static string ToInvariantString(this char target)
        {
            return target.ToString();
        }

        public static string ToInvariantString(this string target)
        {
            return target;
        }

        public static string ToInvariantString(this bool target)
        {
            return target.ToString();
        }

        public static string ToInvariantString(this int target)
        {
            return target.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this float target)
        {
            return target.ToString(CultureInfo.InvariantCulture);
        }

        public static string ToInvariantString(this double target)
        {
            return target.ToString(CultureInfo.InvariantCulture);
        }

        public static bool StartsWithInvarient(this string target, string value)
        {
            return target.StartsWith(value, StringComparison.Ordinal);
        }

        public static bool EndsWithInvarient(this string target, string value)
        {
            return target.EndsWith(value, StringComparison.Ordinal);
        }

        public static int IndexOfInvarient(this string target, char value)
        {
            return target.IndexOf(value);
        }

        public static int IndexOfInvarient(this string target, string value)
        {
            return target.IndexOf(value, StringComparison.Ordinal);
        }

        public static bool EqualsIgnoresCase(this string target, string value)
        {
            return target.Equals(value, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsInvariant(this string target, string value)
        {
            return target.Equals(value, StringComparison.Ordinal);
        }
    }
}