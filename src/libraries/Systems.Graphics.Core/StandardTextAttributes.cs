namespace Xamarin.Graphics
{
    public class StandardTextAttributes : ITextAttributes
    {
        public string FontName { get; set; }

        public float FontSize { get; set; }

        public EwHorizontalAlignment HorizontalAlignment { get; set; }

        public float Margin { get; set; }

        public EWColor TextFontColor { get; set; }

        public EwVerticalAlignment VerticalAlignment { get; set; }
    }
}