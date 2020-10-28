using System;

namespace Elevenworks.Graphics
{
    public class StandardTextAttributes : ITextAttributes
    {
        public string FontName { get; set; }

        public float FontSize { get; set; }

        public EWHorizontalAlignment HorizontalAlignment { get; set; }

        public float Margin { get; set; }

        public EWColor TextFontColor { get; set; }

        public EWVerticalAlignment VerticalAlignment { get; set; }
    }
}