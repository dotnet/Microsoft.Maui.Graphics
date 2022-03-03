using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Foundation;
using UIKit;

namespace Microsoft.Maui.Graphics.Platform
{
	public class PlatformImage : IImage
	{
		private UIImage _image;

		public PlatformImage(UIImage image)
		{
			_image = image;
		}

		public float Width => (float) (_image?.Size.Width ?? 0);

		public float Height => (float) (_image?.Size.Height ?? 0);

		public IImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false)
		{
			var scaledImage = _image.ScaleImage(maxWidthOrHeight, maxWidthOrHeight, disposeOriginal);
			return new PlatformImage(scaledImage);
		}

		public IImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false)
		{
			var scaledImage = _image.ScaleImage(maxWidth, maxHeight, disposeOriginal);
			return new PlatformImage(scaledImage);
		}

		public IImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false)
		{
			using (var context = new PlatformBitmapExportContext((int) width, (int) height, 1))
			{
				var fx = width / Width;
				var fy = height / Height;

				var w = Width;
				var h = Height;

				var x = 0f;
				var y = 0f;

				if (resizeMode == ResizeMode.Fit)
				{
					if (fx < fy)
					{
						w *= fx;
						h *= fx;
					}
					else
					{
						w *= fy;
						h *= fy;
					}

					x = (width - w) / 2;
					y = (height - h) / 2;
				}
				else if (resizeMode == ResizeMode.Bleed)
				{
					if (fx > fy)
					{
						w *= fx;
						h *= fx;
					}
					else
					{
						w *= fy;
						h *= fy;
					}

					x = (width - w) / 2;
					y = (height - h) / 2;
				}
				else
				{
					w = width;
					h = height;
				}

				context.Canvas.DrawImage(this, x, y, w, h);
				return context.Image;
			}
		}

		public UIImage PlatformRepresentation => _image;

		public void Save(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1)
		{
			var data = CreateData(format, quality);
			data.AsStream().CopyTo(stream);
		}

		public async Task SaveAsync(Stream stream, ImageFormat format = ImageFormat.Png, float quality = 1)
		{
			var data = CreateData(format, quality);
			await data.AsStream().CopyToAsync(stream);
		}

		private NSData CreateData(ImageFormat format = ImageFormat.Png, float quality = 1)
		{
			NSData data;
			switch (format)
			{
				case ImageFormat.Jpeg:
					data = _image.AsJPEG(quality);
					break;
				default:
					data = _image.AsPNG();
					break;
			}

			if (data == null)
			{
				throw new Exception($"Unable to write the image in the {format} format.");
			}

			return data;
		}

		public void Dispose()
		{
			var disp = Interlocked.Exchange(ref _image, null);
			disp?.Dispose();
		}

		public void Draw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.DrawImage(this, dirtyRect.Left, dirtyRect.Top, (float)Math.Round(dirtyRect.Width), (float)Math.Round(dirtyRect.Height));
		}

		public IImage ToPlatformImage()
			=> this;

		public static IImage FromStream(Stream stream, ImageFormat format = ImageFormat.Png)
		{
			var data = NSData.FromStream(stream);
			var image = UIImage.LoadFromData(data);
			return new PlatformImage(image);
		}
	}
}
