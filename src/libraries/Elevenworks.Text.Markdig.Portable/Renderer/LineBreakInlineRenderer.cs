using Markdig.Syntax.Inlines;

namespace Elevenworks.Text.Markdig.Renderer
{
    public class LineBreakInlineRenderer : AttributedTextObjectRenderer<LineBreakInline>
    {
        protected override void Write(AttributedTextRenderer renderer, LineBreakInline obj)
        {
            renderer.WriteLine();
        }
    }
}