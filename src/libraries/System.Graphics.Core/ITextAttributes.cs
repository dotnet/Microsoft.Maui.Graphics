namespace System.Graphics
{
    public interface ITextAttributes
    {
        string FontName { get; set; }

        float FontSize { get; set; }

        float Margin { get; set; }

        Color TextFontColor { get; set; }

        EwHorizontalAlignment HorizontalAlignment { get; set; }

        EwVerticalAlignment VerticalAlignment { get; set; }
    }
}