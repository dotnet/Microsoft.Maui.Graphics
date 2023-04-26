#if __MACOS__
using AppKit;
#else
using NSStringAttributeKey = UIKit.UIStringAttributeKey;
#endif
using Microsoft.Maui.Graphics.Text;
using CoreText;
using Foundation;

namespace Microsoft.Maui.Graphics.Platform
{
	public static class AttributedTextExtensions
	{
		public static NSAttributedString AsNSAttributedString(
			this IAttributedText target,
			IFont contextFont = null,
			float contextFontSize = 12f,
			string contextFontColor = null,
			bool coreTextCompatible = false)
		{
			if (target != null)
			{
				var defaultAttributes = GetDefaultAttributes(contextFont, contextFontSize, contextFontColor);
				var attributedString = new NSMutableAttributedString(target.Text, defaultAttributes);

				if (target.Runs != null)
				{
					foreach (var run in target.Runs)
					{
						HandleRun(attributedString, run, contextFont, contextFontSize, contextFontColor, coreTextCompatible);
					}
				}

				return attributedString;
			}

			return null;
		}

		private static void HandleRun(
			NSMutableAttributedString attributedString,
			IAttributedTextRun section,
			IFont contextFont = null,
			float contextFontSize = 12f,
			string contextFontColor = null,
			bool coreTextCompatible = false)
		{
			AddAttributes(attributedString, section.Attributes, section.Start, section.Length, contextFont, contextFontSize, contextFontColor, coreTextCompatible);
		}

		private static CTStringAttributes GetDefaultAttributes(
			IFont contextFont = null,
			float contextFontSize = 12f,
			string contextFontColor = null)
		{
			var ctattributes = new CTStringAttributes();

			var font = contextFont?.ToCTFont(contextFontSize) ?? FontExtensions.GetDefaultCTFont(contextFontSize);

			ctattributes.Font = font;

			if (contextFontColor != null)
				ctattributes.ForegroundColor = contextFontColor.Parse().ToCGColor();

			return ctattributes;
		}

		private static void AddAttributes(
			NSMutableAttributedString attributedString,
			Text.ITextAttributes attributes,
			int start,
			int length,
			IFont contextFont = null,
			float contextFontSize = 12f,
			string contextFontColor = null,
			bool coreTextCompatible = false)
		{
			var ctattributes = new CTStringAttributes();

			var font = contextFont?.ToCTFont(contextFontSize) ?? FontExtensions.GetDefaultCTFont(contextFontSize);

			if (attributes.GetBold() && attributes.GetItalic())
			{
				font = font.WithSymbolicTraits(
					contextFontSize,
					CTFontSymbolicTraits.Bold | CTFontSymbolicTraits.Italic,
					CTFontSymbolicTraits.Bold | CTFontSymbolicTraits.Italic);
			}
			else if (attributes.GetBold())
			{
				font = font.WithSymbolicTraits(contextFontSize, CTFontSymbolicTraits.Bold, CTFontSymbolicTraits.Bold);
			}
			else if (attributes.GetItalic())
			{
				font = font.WithSymbolicTraits(contextFontSize, CTFontSymbolicTraits.Italic, CTFontSymbolicTraits.Italic);
			}

			ctattributes.Font = font;

			if (attributes.GetUnderline())
				ctattributes.UnderlineStyle = CTUnderlineStyle.Single;

			var foreground = attributes.GetForegroundColor();
			if (foreground != null)
				ctattributes.ForegroundColor = foreground.Parse().ToCGColor();
			else
			{
				if (contextFontColor != null)
					ctattributes.ForegroundColor = contextFontColor.Parse().ToCGColor();
				else
					ctattributes.ForegroundColorFromContext = true;
			}

			var background = attributes.GetBackgroundColor();
			if (background != null)
				ctattributes.BackgroundColor = background.Parse().ToCGColor();

			attributedString.AddAttributes(ctattributes, new NSRange(start, length));

			NSMutableDictionary dictionary = null;
#if MONOMAC
#if DEBUG
			var previousCheckStatus = NSApplication.CheckForIllegalCrossThreadCalls;
			NSApplication.CheckForIllegalCrossThreadCalls = false;
#endif

			if (attributes.GetUnorderedList())
			{
				var paragraphStyle = new NSMutableParagraphStyle ();

				var textLists = new NSTextList[1];

				var marker = "{disc}";

				switch (attributes.GetMarker())
				{
					case MarkerType.Hyphen:
						marker = "{hyphen}";
						break;
					case MarkerType.OpenCircle:
						marker = "{circle}";
						break;
				}

				textLists [0] = new NSTextList (marker, NSTextListOptions.PrependEnclosingMarker);
				paragraphStyle.SetTextLists(textLists);

				if (dictionary == null) dictionary = new NSMutableDictionary ();
				dictionary.Add(NSStringAttributeKey.ParagraphStyle, paragraphStyle);
			}

#if DEBUG
			NSApplication.CheckForIllegalCrossThreadCalls = previousCheckStatus;
#endif
#endif

#if MONOMAC
			if (!coreTextCompatible)
			{
				if (attributes.GetSuperscript())
				{
					if (dictionary == null) dictionary = new NSMutableDictionary();
					dictionary.Add(NSStringAttributeKey.Superscript, NSNumber.FromInt32(1));
				}

				if (attributes.GetSubscript())
				{
					if (dictionary == null) dictionary = new NSMutableDictionary();
					dictionary.Add(NSStringAttributeKey.Superscript, NSNumber.FromInt32(-1));
				}
			}
			else
			{
#endif
			if (attributes.GetSuperscript())
			{
				if (dictionary == null) dictionary = new NSMutableDictionary();
				dictionary.Add(NSStringAttributeKey.BaselineOffset, NSNumber.FromFloat(contextFontSize * .5f));
			}

			if (attributes.GetSubscript())
			{
				if (dictionary == null) dictionary = new NSMutableDictionary();
				dictionary.Add(NSStringAttributeKey.BaselineOffset, NSNumber.FromFloat(-contextFontSize * .2f));
			}

#if MONOMAC
		}
#endif

			if (dictionary != null)
				attributedString.AddAttributes(dictionary, new NSRange(start, length));
		}
	}
}
