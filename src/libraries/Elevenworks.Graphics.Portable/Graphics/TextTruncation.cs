using System;

namespace Elevenworks.Graphics
{
    public static class TextTruncation
    {
        /// <summary>
        /// Get's a truncated string with an ellipsis at the end of the string
        /// if the string is wider than the specified width.
        /// 
        /// Note: If there is more than one line in the string, it will evaluate
        /// the first line of the string only.
        /// </summary>
        /// <returns>The truncated string.</returns>
        /// <param name="service">Service.</param>
        /// <param name="value">Value.</param>
        /// <param name="width">Width.</param>
        /// <param name="fontName">Font name.</param>
        /// <param name="fontSize">Font size.</param>
        public static string GetTruncatedString(
            this IGraphicsService service,
            string value,
            float width,
            string fontName,
            float fontSize)
        {
            if (value == null)
                return null;

            if (width <= 0)
                return null;

            var index = value.IndexOf("\n", StringComparison.Ordinal);
            if (index >= 0)
                value = value.Substring(0, index - 1);

            var path = new EWPath();
            path.AppendRectangle(0, 0, width, fontSize * 2);

            var size = service.GetStringSize(value, fontName, fontSize);
            var actualWidth = size.Width;

            // If the string will already fit into the space allotted then go ahead and return
            if (actualWidth <= width)
                return value;

            // Since the string is longer, we need to start truncating it until we find a value that fits
            var candidate = value;
            var endIndex = value.Length - 1;

            while (actualWidth > width)
            {
                endIndex -= 1;
                candidate = $"{value.Substring(0, endIndex)}…";
                size = service.GetStringSize(candidate, fontName, fontSize);
                actualWidth = size.Width;
            }

            return candidate;
        }
    }
}