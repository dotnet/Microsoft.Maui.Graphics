﻿using System.Collections.Generic;
using System.Linq;
using AppKit;
using CoreGraphics;

namespace Elevenworks.Graphics
{
    public class MMFontRegistry
    {
        private static MMFontRegistry _instance = new MMFontRegistry();

        private readonly Dictionary<string, CGFont> _customFonts = new Dictionary<string, CGFont>();
        private readonly string _systemFontName;

        protected MMFontRegistry()
        {
            var font = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
            _systemFontName = font.FontName;
            font.Dispose();
        }

        public static MMFontRegistry Instance => _instance ?? (_instance = new MMFontRegistry());

        public Dictionary<string, CGFont>.ValueCollection CustomFonts => _customFonts.Values;

        public void RegisterFont(CGFont font)
        {
            _customFonts.Add(font.FullName, font);
        }

        public bool IsCustomFont(string name)
        {
            return name != null && _customFonts.ContainsKey(name);
        }

        public CGFont GetCustomFont(string name)
        {
            if (name == null) return CGFont.CreateWithFontName(_systemFontName);

            if (_customFonts.TryGetValue(name, out var font))
            {
                return font;
            }

            return CGFont.CreateWithFontName(_systemFontName);
        }

        public void ClearCustomFonts()
        {
            var keys = _customFonts.Keys.ToArray();

            foreach (var key in keys)
            {
                var font = _customFonts[key];
                _customFonts.Remove(key);
                font.Dispose();
            }
        }
    }
}