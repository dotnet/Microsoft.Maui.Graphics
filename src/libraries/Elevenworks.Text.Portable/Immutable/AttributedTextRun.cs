namespace Elevenworks.Text.Immutable
{
    public class AttributedTextRun : IAttributedTextRun
    {
        public AttributedTextRun(
            int start,
            int length,
            ITextAttributes attributes)
        {
            Start = start;
            Length = length;
            Attributes = attributes;
        }

        public int Start { get; }

        public int Length { get; }

        public ITextAttributes Attributes { get; }

        public override string ToString()
        {
            return string.Format("[AttributedTextRun: Start={0}, Length={1}, Attributes={2}]", Start, Length, Attributes);
        }
    }
}