using System;
using Markdig.Renderers;
using Markdig.Syntax;

namespace Elevenworks.Text.Markdig.Renderer
{
    public abstract class AttributedTextObjectRenderer<T>
        : MarkdownObjectRenderer<AttributedTextRenderer, T> where T : MarkdownObject
    {
    }
}