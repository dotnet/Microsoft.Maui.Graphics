﻿using Android.Graphics;

namespace Elevenworks.Text
{
    public static class CGColorExtensions
    {
        public static Color? ToColor(this int[] color)
        {
            if (color == null)
                return null;

            return new Color(color[0], color[1], color[2], color[3]);
        }
    }
}