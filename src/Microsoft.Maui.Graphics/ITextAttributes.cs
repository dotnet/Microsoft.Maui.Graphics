namespace Microsoft.Maui.Graphics
{
	public interface ITextAttributes
	{
		string FontName { get; set; }

		float FontSize { get; set; }

		float Margin { get; set; }

		Color TextFontColor { get; set; }

		TextAlignment HorizontalAlignment { get; set; }

		TextAlignment VerticalAlignment { get; set; }
	}
}
