using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Maui.Graphics
{
	public class VirtualGraphicsPlatform : IGraphicsService
	{
		public List<PathF> ConvertToPaths(PathF aPath, string text, ITextAttributes textAttributes, float ppu, float zoom)
		{
			return new List<PathF>();
		}

		public SizeF GetStringSize(string value, string fontName, float textSize)
		{
			return new SizeF(value.Length * 10, textSize + 2);
		}

		public SizeF GetStringSize(string value, string fontName, float textSize, TextAlignment horizontalAlignment, TextAlignment verticalAlignment)
		{
			return new SizeF(value.Length * 10, textSize + 2);
		}

		public void LayoutText(PathF path, string text, ITextAttributes textAttributes, LayoutLine callback)
		{
			// Do nothing
		}

		public RectangleF GetPathBounds(PathF path)
		{
			throw new NotImplementedException();
		}

		public RectangleF GetPathBoundsWhenRotated(PointF center, PathF path, float angle)
		{
			throw new NotImplementedException();
		}

		public bool PathContainsPoint(PathF path, PointF point, float ppu, float zoom, float strokeWidth)
		{
			throw new NotImplementedException();
		}

		public bool PointIsOnPath(PathF path, PointF point, float ppu, float zoom, float strokeWidth)
		{
			throw new NotImplementedException();
		}

		public bool PointIsOnPathSegment(PathF path, int segmentIndex, PointF point, float ppu, float zoom, float strokeWidth)
		{
			throw new NotImplementedException();
		}

		public IImage LoadImageFromStream(Stream stream, ImageFormat format = ImageFormat.Png)
		{
			using (var memoryStream = new MemoryStream())
			{
				using (stream)
				{
					stream.CopyTo(memoryStream);
				}

				return new VirtualImage(memoryStream.ToArray(), format);
			}
		}

		public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
		{
			return null;
		}

		public string SystemFontName => "Arial";
		public string BoldSystemFontName => "Arial-Bold";
	}
}
