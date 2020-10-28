using System.Collections.Generic;

namespace Elevenworks.Text.Mutable
{
    public class MutableAttributedText : IAttributedText
    {
        private List<IAttributedTextRun> _runs;

        public MutableAttributedText(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public IReadOnlyList<IAttributedTextRun> Runs => _runs;

        public bool Optimal => true;

        public void AddRun(IAttributedTextRun run)
        {
            if (_runs == null)
            {
                _runs = new List<IAttributedTextRun> {run};
                return;
            }

            _runs.Add(run);
            _runs = this.OptimizeRuns();
        }
    }
}