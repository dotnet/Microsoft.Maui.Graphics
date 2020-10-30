using CoreGraphics;
using CoreText;
using Foundation;

namespace System.Graphics.CoreGraphics
{
    public static class CGCanvasExtensions
    {
        public static void SetFillColor(this CGContext context, EWColor color, EWColor defaultColor = null)
        {
            if (color != null)
            {
                context.SetFillColor(color.Red, color.Green, color.Blue, color.Alpha);
            }
            else
            {
                context.SetFillColor(1, 1, 1, 1); // White
            }
        }

        public static void SetStrokeColor(this CGContext context, EWColor color, EWColor defaultColor = null)
        {
            if (color != null)
            {
                context.SetStrokeColor(color.Red, color.Green, color.Blue, color.Alpha);
            }
            else
            {
                context.SetStrokeColor(0, 0, 0, 1); // White
            }
        }

        public static CGColor AsCGColor(this EWColor color)
        {
            return new CGColor(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static void AddRoundedRectangle(this CGContext context, float x, float y, float width, float height, float cornerRadius)
        {
            double actualRadius = cornerRadius;

            var rect = new CGRect(x, y, width, height);

            if (actualRadius > rect.Width)
            {
                actualRadius = rect.Width / 2;
            }

            if (actualRadius > rect.Height)
            {
                actualRadius = rect.Height / 2;
            }

            var minx = rect.X;
            var miny = rect.Y;
            var maxx = minx + rect.Width;
            var maxy = miny + rect.Height;
            var midx = minx + (rect.Width / 2);
            var midy = miny + (rect.Height / 2);

            context.MoveTo(minx, midy);
            context.AddArcToPoint(minx, miny, midx, miny, (float) actualRadius);
            context.AddArcToPoint(maxx, miny, maxx, midy, (float) actualRadius);
            context.AddArcToPoint(maxx, maxy, midx, maxy, (float) actualRadius);
            context.AddArcToPoint(minx, maxy, minx, midy, (float) actualRadius);
            context.ClosePath();
        }

        public static void DrawCenteredString(this CGContext context, string value, float x, float y, float width, float height, string fontName, float fontSize)
        {
            if (string.IsNullOrEmpty(value) || width == 0 || height == 0)
            {
                return;
            }

            context.SaveState();
            context.TranslateCTM(0, height);
            context.ScaleCTM(1, -1f);

            context.TextMatrix = CGAffineTransform.MakeIdentity();

            // Initialize a rectangular path.
            var vPath = new CGPath();
            vPath.AddRect(new CGRect(x, y * -1, width, height));

            var vAttributedString = new NSMutableAttributedString(value);

            // Create a color and add it as an attribute to the string.
            var vAttributes = new CTStringAttributes {ForegroundColor = new CGColor(1f, 1f, 1f, 1f)};

            fontSize = fontSize < 10 ? 10 : fontSize;
            // Load the font

#if MONOMAC
            CTFont vFont = MMFontService.Instance.LoadFont(fontName, fontSize);
#else
            CTFont vFont = MTFontService.Instance.LoadFont(fontName, fontSize);
#endif

            vAttributes.Font = vFont;

            // Set the horizontal alignment
            var vParagraphSettings = new CTParagraphStyleSettings {Alignment = CTTextAlignment.Center};

            var vParagraphStyle = new CTParagraphStyle(vParagraphSettings);
            vAttributes.ParagraphStyle = vParagraphStyle;

            // Set the attributes for the complete length of the string
            vAttributedString.SetAttributes(vAttributes, new NSRange(0, value.Length));

            // Create the framesetter with the attributed string.
            var vFrameSetter = new CTFramesetter(vAttributedString);

            // Create the frame and draw it into the graphics context
            var vFrame = vFrameSetter.GetFrame(new NSRange(0, 0), vPath, null);

            double vHeight = 0;
            foreach (var vLine in vFrame.GetLines())
            {
                vLine.GetTypographicBounds(out var vAscent, out var vDescent, out var vLeading);

                vHeight += vAscent;
                vHeight -= vDescent;
                vHeight += vLeading;

                vHeight += vLine.GetImageBounds(context).Height;
            }

            double vSpace = -1 * (height - vHeight);
            var ty = (float) Math.Round(vSpace / 2);
            context.TranslateCTM(0, ty);

            vFrame.Draw(context);

            context.RestoreState();

            vFrame.Dispose();
            vFrameSetter.Dispose();
            vAttributedString.Dispose();
            vParagraphStyle.Dispose();
            //vFont.Dispose();
            vPath.Dispose();
        }
    }
}