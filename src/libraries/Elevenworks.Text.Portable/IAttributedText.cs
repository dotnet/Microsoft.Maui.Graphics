using System.Collections.Generic;

namespace Elevenworks.Text
{
    public interface IAttributedText
    {
        string Text { get; }
        IReadOnlyList<IAttributedTextRun> Runs { get; }
        bool Optimal { get; }
    }
}