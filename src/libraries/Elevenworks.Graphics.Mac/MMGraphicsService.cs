using System;
using System.IO;
using System.Collections.Generic;
using CoreGraphics;
using CoreText;
using Foundation;
using AppKit;

namespace Elevenworks.Graphics
{
    public class MMGraphicsService : IGraphicsService
    {
        public static MMGraphicsService Instance = new MMGraphicsService();

        private readonly string _boldSystemFontName;
        private readonly string _systemFontName;

        private MMGraphicsService()
        {
            var vFont = NSFont.SystemFontOfSize(NSFont.SystemFontSize);
            _systemFontName = vFont.FontName;
            vFont.Dispose();

            var vBoldFont = NSFont.BoldSystemFontOfSize(NSFont.SystemFontSize);
            _boldSystemFontName = vBoldFont.FontName;
            vBoldFont.Dispose();
        }

        public EWImage LoadImageFromStream(Stream stream, EWImageFormat formatHint = EWImageFormat.Png)
        {
            if (stream == null)
                return null;

            var previous = NSApplication.CheckForIllegalCrossThreadCalls;
            NSApplication.CheckForIllegalCrossThreadCalls = false;
            var data = NSData.FromStream(stream);
            var image = new NSImage(data);
            NSApplication.CheckForIllegalCrossThreadCalls = previous;
            return new MMImage(image);
        }

        #region GraphicsPlatform Members

        public string SystemFontName => _systemFontName;
        public string BoldSystemFontName => _boldSystemFontName;

        public string GetFont(string aFontName, bool aBold, bool aItalic)
        {
            if (MMFontRegistry.Instance.IsCustomFont(aFontName))
            {
                return aFontName;
            }

            try
            {
                var vFontName = aFontName ?? _systemFontName;
                var vFont = NSFont.FromFontName(vFontName, NSFont.SystemFontSize);

                var vTraits = NSFontManager.SharedFontManager.AvailableMembersOfFontFamily(vFontName);
                var names = new List<string>();
                var values = new List<string>();

                Array.ForEach(
                    vTraits,
                    vSubarray =>
                    {
                        var vArray = (NSArray) ObjCRuntime.Runtime.GetNSObject(vSubarray.Handle);
                        var vValue = (NSString) ObjCRuntime.Runtime.GetNSObject(vArray.ValueAt(0));
                        var vName = (NSString) ObjCRuntime.Runtime.GetNSObject(vArray.ValueAt(1));

                        names.Add(vName.ToString());
                        values.Add(vValue.ToString());
                    });

                vFont.Dispose();

                for (var i = 0; i < names.Count; i++)
                {
                    var vStyle = names[i];
                    var vName = values[i];

                    var vIsBold = vStyle.Contains("Bold");
                    var vIsItalic = vStyle.Contains("Italic");

                    if (vIsBold == aBold && vIsItalic == aItalic)
                    {
                        return vName;
                    }
                }
            }
            catch (Exception exc)
            {
#if DEBUG
                Logger.Debug(exc);
#endif
            }

            return aFontName;
        }

        public string GetFontName(string aFontName)
        {
            if (MMFontRegistry.Instance.IsCustomFont(aFontName))
            {
                return aFontName;
            }

            try
            {
                var vFontName = aFontName ?? _systemFontName;
                var vFont = NSFont.FromFontName(vFontName, NSFont.SystemFontSize);
                var vName = vFont.FamilyName;
                vFont.Dispose();
                return vName;
            }
            catch (Exception exc)
            {
#if DEBUG
                Logger.Debug(exc);
#endif
                return aFontName;
            }
        }

        public string GetFontWeight(string aFontName)
        {
            var vFontName = aFontName ?? _systemFontName;
            var vFont = new CTFont(vFontName, NSFont.SystemFontSize);
            var vStyle = vFont.GetName(CTFontNameKey.Style);
            vFont.Dispose();

            if (vStyle != null)
            {
                if (vStyle.Contains("Bold"))
                {
                    return "bold";
                }
            }

            return "normal";
        }

        public string GetFontStyle(string aFontName)
        {
            var vFontName = aFontName ?? _systemFontName;
            var vFont = new CTFont(vFontName, NSFont.SystemFontSize);
            var vStyle = vFont.GetName(CTFontNameKey.Style);
            vFont.Dispose();

            if (vStyle != null)
            {
                if (vStyle.Contains("Italic"))
                {
                    return "italic";
                }
            }

            return "normal";
        }

        public EWSize GetStringSize(string value, string fontName, float fontSize)
        {
            var actualFontName = fontName ?? _systemFontName;
            var nativeString = new NSString(value);
            var font = NSFont.FromFontName(actualFontName, (float) fontSize);
            if (font == null)
                font = NSFont.FromFontName(_systemFontName, (float) fontSize);
            var attributes = new NSMutableDictionary();
            attributes[NSStringAttributeKey.Font] = font;
            var size = nativeString.StringSize(attributes);
            font.Dispose();
            return size.AsEWSize();
        }

        public EWSize GetStringSize(string aString, string aFontName, float aFontSize, EwHorizontalAlignment aHorizontalAlignment, EwVerticalAlignment aVerticalAlignment)
        {
            var fontSize = aFontSize;
            float factor = 1;
            while (fontSize > 10)
            {
                fontSize /= 10;
                factor *= 10;
            }

            var vPath = new CGPath();
            vPath.AddRect(new CGRect(0, 0, 512, 512));
            vPath.CloseSubpath();

            var vAttributedString = new NSMutableAttributedString(aString);

            var vAttributes = new CTStringAttributes();

            // Load the font
            var vFont = MMFontService.Instance.LoadFont(aFontName ?? _systemFontName, (float) fontSize);
            vAttributes.Font = vFont;

            // Set the horizontal alignment
            var vParagraphSettings = new CTParagraphStyleSettings();
            switch (aHorizontalAlignment)
            {
                case EwHorizontalAlignment.Left:
                    vParagraphSettings.Alignment = CTTextAlignment.Left;
                    break;
                case EwHorizontalAlignment.Center:
                    vParagraphSettings.Alignment = CTTextAlignment.Center;
                    break;
                case EwHorizontalAlignment.Right:
                    vParagraphSettings.Alignment = CTTextAlignment.Right;
                    break;
                case EwHorizontalAlignment.Justified:
                    vParagraphSettings.Alignment = CTTextAlignment.Justified;
                    break;
            }

            var vParagraphStyle = new CTParagraphStyle(vParagraphSettings);
            vAttributes.ParagraphStyle = vParagraphStyle;

            // Set the attributes for the complete length of the string
            vAttributedString.SetAttributes(vAttributes, new NSRange(0, aString.Length));

            // Create the framesetter with the attributed string.
            var vFrameSetter = new CTFramesetter(vAttributedString);

            var textBounds = GetTextSize(vFrameSetter, vPath);
            //Logger.Debug("{0} {1}",vSize,aString);

            vFrameSetter.Dispose();
            vAttributedString.Dispose();
            vParagraphStyle.Dispose();
            //vFont.Dispose();
            vPath.Dispose();

            textBounds.Width *= factor;
            textBounds.Height *= factor;

            //vSize.Width = Math.Ceiling(vSize.Width);
            //vSize.Height = Math.Ceiling(vSize.Height);

            return textBounds.Size;
        }

        public static EWRectangle GetTextSize(CTFramesetter frameSetter, CGPath path)
        {
            var frame = frameSetter.GetFrame(new NSRange(0, 0), path, null);

            if (frame != null)
            {
                var textSize = GetTextSize(frame);
                frame.Dispose();
                return textSize;
            }

            return new EWRectangle(0, 0, 0, 0);
        }

        public static EWRectangle GetTextSize(CTFrame frame)
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

            return new EWRectangle(0f, minY, width, Math.Max(0, maxY - minY));
        }

        public EWRectangle GetPathBounds(EWPath path)
        {
            var nativePath = path.NativePath as CGPath;

            if (nativePath == null)
            {
                nativePath = path.AsCGPath();
                path.NativePath = nativePath;
            }

            var bounds = nativePath.PathBoundingBox;
            return bounds.AsEWRectangle();
        }

        public EWRectangle GetPathBoundsWhenRotated(EWImmutablePoint centerOfRotation, EWPath path, float angle)
        {
            var nativePath = path.AsRotatedCGPath(centerOfRotation, 1, 1f, angle);
            var bounds = nativePath.PathBoundingBox;
            nativePath.Dispose();
            return bounds.AsEWRectangle();
        }
        
        #endregion
        
        public CTFont LoadFont(ITextAttributes aTextAttributes)
        {
            return aTextAttributes.FontName == null
                ? MMFontService.Instance.LoadFont(_systemFontName, (float) aTextAttributes.FontSize)
                : MMFontService.Instance.LoadFont(aTextAttributes.FontName, (float) aTextAttributes.FontSize);
        }

        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return new MMBitmapExportContext(width, height, displayScale);
        }

        public bool IsRetina => true;
    }
}