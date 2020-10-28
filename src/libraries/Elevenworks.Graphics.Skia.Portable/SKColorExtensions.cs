﻿using SkiaSharp;
using Xamarin.Graphics;

namespace Elevenworks.Graphics
{
    public static class SKColorExtensions
    {
        public static SKColor ToColor(this EWColor target, float alpha = 1)
        {
            var r = (byte) (target.Red * 255f);
            var g = (byte) (target.Green * 255f);
            var b = (byte) (target.Blue * 255f);
            var a = (byte) (target.Alpha * 255f * alpha);

            return new SKColor(r, g, b, a);
        }
    }
}