﻿namespace System.Graphics
{
    public class StandardTextAttributes : ITextAttributes
    {
        public string FontName { get; set; }

        public float FontSize { get; set; }

        public HorizontalAlignment HorizontalAlignment { get; set; }

        public float Margin { get; set; }

        public Color TextFontColor { get; set; }

        public VerticalAlignment VerticalAlignment { get; set; }
    }
}