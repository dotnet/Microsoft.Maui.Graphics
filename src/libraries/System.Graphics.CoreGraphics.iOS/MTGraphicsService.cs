using System.Drawing;
using System.IO;
using CoreGraphics;
using CoreText;
using Foundation;
using UIKit;

namespace System.Graphics.CoreGraphics
{
    public class MTGraphicsService : IGraphicsService
    {
        public static readonly MTGraphicsService Instance = new MTGraphicsService();

        private readonly string _systemFontName;

        private MTGraphicsService()
        {
            var systemFont = UIFont.SystemFontOfSize(UIFont.SystemFontSize);
            _systemFontName = systemFont.Name;
            systemFont.Dispose();

            var boldSystemFont = UIFont.BoldSystemFontOfSize(UIFont.SystemFontSize);
            BoldSystemFontName = boldSystemFont.Name;
            boldSystemFont.Dispose();
        }

        public IImage LoadImageFromStream(Stream stream, ImageFormat format = ImageFormat.Png)
        {
            var data = NSData.FromStream(stream);
            var image = UIImage.LoadFromData(data);
            return new MTImage(image);
        }

        #region GraphicsPlatform Members

        public string SystemFontName => _systemFontName;
        public string BoldSystemFontName { get; }

        public SizeF GetStringSize(string value, string fontName, float fontSize)
        {
            if (string.IsNullOrEmpty(value)) return new SizeF();

            var finalFontName = fontName ?? _systemFontName;
            var nsString = new NSString(value);
            UIFont uiFont;  
            if (finalFontName == _systemFontName)
                uiFont = UIFont.SystemFontOfSize(fontSize);
            else if (finalFontName == BoldSystemFontName)
                uiFont = UIFont.BoldSystemFontOfSize(fontSize);
            else
                uiFont = UIFont.FromName(finalFontName, fontSize);

            var size = nsString.StringSize(uiFont);
            uiFont.Dispose();
            return new SizeF((float) size.Width, (float) size.Height);
        }

        public SizeF GetStringSize(
            string value,
            string fontName,
            float fontSize,
            HorizontalAlignment horizontalAlignment,
            VerticalAlignment verticalAlignment)
        {
            float factor = 1;
            while (fontSize > 10)
            {
                fontSize /= 10;
                factor *= 10;
            }

            var path = new CGPath();
            path.AddRect(new Drawing.RectangleF(0, 0, 512, 512));
            path.CloseSubpath();

            var attributedString = new NSMutableAttributedString(value);

            var attributes = new CTStringAttributes();

            // Load the font
            var font = MTFontService.Instance.LoadFont(fontName ?? _systemFontName, fontSize);
            attributes.Font = font;

            // Set the horizontal alignment
            var paragraphSettings = new CTParagraphStyleSettings();
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    paragraphSettings.Alignment = CTTextAlignment.Left;
                    break;
                case HorizontalAlignment.Center:
                    paragraphSettings.Alignment = CTTextAlignment.Center;
                    break;
                case HorizontalAlignment.Right:
                    paragraphSettings.Alignment = CTTextAlignment.Right;
                    break;
                case HorizontalAlignment.Justified:
                    paragraphSettings.Alignment = CTTextAlignment.Justified;
                    break;
            }

            var paragraphStyle = new CTParagraphStyle(paragraphSettings);
            attributes.ParagraphStyle = paragraphStyle;

            // Set the attributes for the complete length of the string
            attributedString.SetAttributes(attributes, new NSRange(0, value.Length));

            // Create the frame setter with the attributed string.
            var frameSetter = new CTFramesetter(attributedString);

            var textBounds = GetTextSize(frameSetter, path);
            //Logger.Debug("{0} {1}",vSize,aString);

            frameSetter.Dispose();
            attributedString.Dispose();
            paragraphStyle.Dispose();
            //font.Dispose();
            path.Dispose();

            textBounds.Width *= factor;
            textBounds.Height *= factor;

            //size.Width = Math.Ceiling(vSize.Width);
            //size.Height = Math.Ceiling(vSize.Height);

            return textBounds.Size;
        }

        private static RectangleF GetTextSize(
            CTFramesetter frameSetter,
            CGPath path)
        {
            var frame = frameSetter.GetFrame(new NSRange(0, 0), path, null);

            if (frame != null)
            {
                var textSize = GetTextSize(frame);
                frame.Dispose();
                return textSize;
            }

            return new RectangleF(0, 0, 0, 0);
        }

        public static RectangleF GetTextSize(CTFrame frame)
        {
            var minY = float.MaxValue;
            var maxY = float.MinValue;
            float width = 0;

            var lines = frame.GetLines();
            var origins = new CGPoint[lines.Length];
            frame.GetLineOrigins(new NSRange(0, 0), origins);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var lineWidth = (float) line.GetTypographicBounds(out var ascent, out var descent, out var leading);

                if (lineWidth > width)
                    width = lineWidth;

                var origin = origins[i];

                minY = (float) Math.Min(minY, origin.Y - ascent);
                maxY = (float) Math.Max(maxY, origin.Y + descent);

                lines[i].Dispose();
            }

            return new RectangleF(0f, minY, width, Math.Max(0, maxY - minY));
        }

        public RectangleF GetPathBounds(PathF path)
        {
            var nativePath = path.NativePath as CGPath;

            if (nativePath == null)
            {
                nativePath = path.AsCGPath();
                path.NativePath = nativePath;
            }

            var bounds = nativePath.PathBoundingBox;
            return bounds.AsRectangleF();
        }

        public RectangleF GetPathBoundsWhenRotated(PointF centerOfRotation, PathF path, float angle)
        {
            var nativePath = path.AsRotatedCGPath(centerOfRotation, 1f, 1f, angle);
            var bounds = nativePath.PathBoundingBox;
            nativePath.Dispose();
            return bounds.AsRectangleF();
        }

        public bool PathContainsPoint(PathF aPath, PointF aPoint, float ppu, float aZoom, float aStrokeWidth)
        {
            var vPath = aPath.NativePath as CGPath;

            if (vPath == null)
            {
                vPath = aPath.AsCGPath();
                aPath.NativePath = vPath;
            }

            return vPath.ContainsPoint(aPoint.AsCGPoint(), aPath.Closed);
        }
        
        #endregion
        
        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return new MTBitmapExportContext(width, height, displayScale);
        }
    }
}