using System.IO;
using System.Threading;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using Factory = SharpDX.Direct2D1.Factory;
using FactoryType = SharpDX.Direct2D1.FactoryType;
using SharpDXTextAlignment = SharpDX.DirectWrite.TextAlignment;

namespace Microsoft.Maui.Graphics.SharpDX
{
	public class DXGraphicsService : IGraphicsService
	{
		private const float Dip = 96f / 72f;

		public static Factory SharedFactory = new Factory(FactoryType.MultiThreaded);

		public static readonly ThreadLocal<Factory> CurrentFactory = new ThreadLocal<Factory>();
		public static readonly ThreadLocal<RenderTarget> CurrentTarget = new ThreadLocal<RenderTarget>();
		public static ImagingFactory2 FactoryImaging = new ImagingFactory2();

		public static global::SharpDX.DirectWrite.Factory FactoryDirectWrite = new global::SharpDX.DirectWrite.Factory(global::SharpDX.DirectWrite.FactoryType.Shared);

		public static readonly DXGraphicsService Instance = new DXGraphicsService();

		private DXGraphicsService()
		{
		}

		public string SystemFontName => "Arial";
		public string BoldSystemFontName => "Arial-Bold";

		public SizeF GetStringSize(string value, string fontName, float textSize)
		{
			if (value == null) return new SizeF();

			float fontSize = textSize;
			float factor = 1;
			while (fontSize > 14)
			{
				fontSize /= 14;
				factor *= 14;
			}

			if (fontName == null)
				fontName = "Arial";

			var size = new SizeF();

			var textFormat = new TextFormat(FactoryDirectWrite, fontName, fontSize);
			textFormat.TextAlignment = SharpDXTextAlignment.Leading;
			textFormat.ParagraphAlignment = ParagraphAlignment.Near;

			var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512, 512);
			size.Width = textLayout.Metrics.Width;
			size.Height = textLayout.Metrics.Height;

			size.Width *= factor;
			size.Height *= factor;

			return size;
		}

		public SizeF GetStringSize(
			string value,
			string fontName,
			float textSize,
			TextAlignment horizontalAlignment,
			TextAlignment verticalAlignment)
		{
			if (value == null) return new SizeF();

			float fontSize = textSize;
			float factor = 1;
			while (fontSize > 14)
			{
				fontSize /= 14;
				factor *= 14;
			}

			var size = new SizeF();

			var textFormat = new TextFormat(FactoryDirectWrite, SystemFontName, FontWeight.Regular, FontStyle.Normal,
				fontSize);
			if (horizontalAlignment == TextAlignment.Start)
			{
				textFormat.TextAlignment = SharpDXTextAlignment.Leading;
			}
			else if (horizontalAlignment == TextAlignment.Center)
			{
				textFormat.TextAlignment = SharpDXTextAlignment.Center;
			}
			else if (horizontalAlignment == TextAlignment.End)
			{
				textFormat.TextAlignment = SharpDXTextAlignment.Trailing;
			}
			//else if (horizontalAlignment == TextAlignment.Justified)
			//{
			//	textFormat.TextAlignment = SharpDXTextAlignment.Justified;
			//}

			if (verticalAlignment == TextAlignment.Start)
			{
				textFormat.ParagraphAlignment = ParagraphAlignment.Near;
			}
			else if (verticalAlignment == TextAlignment.Center)
			{
				textFormat.ParagraphAlignment = ParagraphAlignment.Center;
			}
			else if (verticalAlignment == TextAlignment.End)
			{
				textFormat.ParagraphAlignment = ParagraphAlignment.Far;
			}

			var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512f, 512f, Dip, false);
			size.Width = textLayout.Metrics.Width;
			size.Height = textLayout.Metrics.Height;


			size.Width *= factor;
			size.Height *= factor;

			return size;
		}

		public IImage LoadImageFromStream(Stream stream, Graphics.ImageFormat format = Graphics.ImageFormat.Png)
		{
			var bitmap = CurrentTarget.Value.LoadBitmap(stream);
			return new DXImage(bitmap);
		}

		public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
		{
			return new DXBitmapExportContext(width, height, displayScale, 72, false);
		}

		public RectangleF GetPathBounds(PathF path)
        {
            if (path.NativePath is PathGeometry nativePath)
            {
                return nativePath.GetBounds().AsEWRectangle();
            }

            if (CurrentFactory.Value != null && path.Closed)
            {
                if (CurrentFactory.Value != SharedFactory)
                {
                    nativePath = path.AsDxPath(CurrentFactory.Value);
                    if (nativePath != null)
                    {
                        path.NativePath = nativePath;
                        return nativePath.GetBounds().AsEWRectangle();
                    }
                }
            }

            var bounds = path.GetBoundsByFlattening();
            return bounds;
        }
	}
}
