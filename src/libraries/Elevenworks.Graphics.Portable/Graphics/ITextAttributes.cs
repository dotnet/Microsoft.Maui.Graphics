namespace Elevenworks.Graphics
{
    public interface ITextAttributes
    {
        string FontName { get; set; }

        float FontSize { get; set; }

        float Margin { get; set; }

        EWColor TextFontColor { get; set; }

        EWHorizontalAlignment HorizontalAlignment { get; set; }

        EWVerticalAlignment VerticalAlignment { get; set; }
    }
}