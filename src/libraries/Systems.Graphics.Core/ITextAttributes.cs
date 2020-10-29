namespace Xamarin.Graphics
{
    public interface ITextAttributes
    {
        string FontName { get; set; }

        float FontSize { get; set; }

        float Margin { get; set; }

        EWColor TextFontColor { get; set; }

        EwHorizontalAlignment HorizontalAlignment { get; set; }

        EwVerticalAlignment VerticalAlignment { get; set; }
    }
}