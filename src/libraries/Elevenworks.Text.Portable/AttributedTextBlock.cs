namespace Elevenworks.Text
{
    public class AttributedTextBlock
    {
        public string Text { get; }
        public ITextAttributes Attributes { get; }

        public AttributedTextBlock(string text, ITextAttributes attributes)
        {
            Text = text;
            Attributes = attributes;
        }

        public override string ToString()
        {
            return string.Format("[AttributedTextBlock: Text={0}, Attributes={1}]", Text, Attributes);
        }
    }
}