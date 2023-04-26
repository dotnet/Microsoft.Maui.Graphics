using AppKit;

namespace Microsoft.Maui.Graphics.Platform
{
	public static class CoreGraphicsExtensions
	{
		public static ImagePaint AsPaint(this NSImage target)
		{
			if (target == null)
				return null;

			var image = new PlatformImage(target);
			var paint = new ImagePaint {Image = image};

			return paint;
		}

		public static NSColor AsNSColor(this Color color)
		{
			return NSColor.FromDeviceRgba(color.Red, color.Green, color.Blue, color.Alpha);
		}
	}
}
