using Elevenworks.Text.Markdig.Renderer;
using Markdig;
using Xamarin.Text;

namespace Elevenworks.Text.Markdig
{
    public class MarkdownAttributedTextReader
    {
        public static IAttributedText Read(string text)
        {
            //var html = Markdown.ToHtml(text);
            //var document = Markdown.Parse(text);

            var renderer = new AttributedTextRenderer();
            var builder = new MarkdownPipelineBuilder().UseEmphasisExtras();
            var pipeline = builder.Build();
            Markdown.Convert(text, renderer, pipeline);
            return renderer.GetAttributedText();
        }
    }
}