using System.Collections.Generic;

namespace Xamarin.Text
{
    public interface IAttributedText
    {
        string Text { get; }
        IReadOnlyList<IAttributedTextRun> Runs { get; }
    }
}