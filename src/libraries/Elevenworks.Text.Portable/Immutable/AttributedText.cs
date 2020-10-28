using System.Collections.Generic;

namespace Elevenworks.Text.Immutable
{
    public class AttributedText : IAttributedText
    {
        public AttributedText(
            string text,
            IReadOnlyList<IAttributedTextRun> runs,
            bool optimal = false)
        {
            Text = text;
            Runs = runs;
            Optimal = optimal;
        }

        public string Text { get; }

        public IReadOnlyList<IAttributedTextRun> Runs { get; }

        public bool Optimal { get; }
    }
}