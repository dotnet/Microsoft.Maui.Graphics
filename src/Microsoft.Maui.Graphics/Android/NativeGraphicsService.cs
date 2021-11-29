using System.IO;
using Android.Graphics;
using Android.Text;

namespace Microsoft.Maui.Graphics.Native
{
	public class NativeGraphicsService : IGraphicsService
	{
		public static readonly NativeGraphicsService Instance = new NativeGraphicsService();

		private static string _systemFontName;
		private static string _boldSystemFontName;

		public RectangleF GetPathBounds(PathF path)
		{
			return path.GetBoundsByFlattening();
		}

		public IImage LoadImageFromStream(Stream stream, ImageFormat formatHint = ImageFormat.Png)
		{
			var bitmap = BitmapFactory.DecodeStream(stream);
			return new NativeImage(bitmap);
		}

		public string SystemFontName
		{
			get
			{
				if (_systemFontName == null)
				{
					_systemFontName = NativeFontService.SystemFont;
					_boldSystemFontName = NativeFontService.SystemBoldFont;
				}

				return _systemFontName;
			}
		}

		public string BoldSystemFontName
		{
			get
			{
				if (_boldSystemFontName == null)
				{
					_systemFontName = NativeFontService.SystemFont;
					_boldSystemFontName = NativeFontService.SystemBoldFont;
				}

				return _boldSystemFontName;
			}
		}

		public SizeF GetStringSize(string value, string fontName, float fontSize)
		{
			if (value == null) return new SizeF();

			var textPaint = new TextPaint {TextSize = fontSize};
			textPaint.SetTypeface(NativeFontService.Instance.GetTypeface(fontName));

			var staticLayout = TextLayoutUtils.CreateLayout(value, textPaint, null, Layout.Alignment.AlignNormal);
			var size = staticLayout.GetTextSizeAsSizeF(false);
			staticLayout.Dispose();
			return size;
		}



		public SizeF GetStringSize(string aString, string aFontName, float aFontSize, TextAlignment aHorizontalAlignment, TextAlignment aVerticalAlignment)
		{
			if (aString == null) return new SizeF();

			var vTextPaint = new TextPaint {TextSize = aFontSize};
			vTextPaint.SetTypeface(NativeFontService.Instance.GetTypeface(aFontName));

			Layout.Alignment vAlignment;
			switch (aHorizontalAlignment)
			{
				case TextAlignment.Center:
					vAlignment = Layout.Alignment.AlignCenter;
					break;
				case TextAlignment.End:
					vAlignment = Layout.Alignment.AlignOpposite;
					break;
				default:
					vAlignment = Layout.Alignment.AlignNormal;
					break;
			}

			StaticLayout vLayout = TextLayoutUtils.CreateLayout(aString, vTextPaint, null, vAlignment);
			SizeF vSize = vLayout.GetTextSizeAsSizeF(false);
			vLayout.Dispose();
			return vSize;
		}

		public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
		{
			return new NativeBitmapExportContext(width, height, displayScale, 72, false, true);
		}
	}
}
