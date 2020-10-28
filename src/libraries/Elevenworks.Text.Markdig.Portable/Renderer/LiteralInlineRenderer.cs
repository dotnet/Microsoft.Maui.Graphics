using System;
using Markdig.Syntax.Inlines;

namespace Elevenworks.Text.Markdig.Renderer
{
    public class LiteralInlineRenderer : AttributedTextObjectRenderer<LiteralInline>
    {
        protected override void Write(AttributedTextRenderer renderer, LiteralInline obj)
        {
            renderer.Write(ref obj.Content);
        }
    }
}