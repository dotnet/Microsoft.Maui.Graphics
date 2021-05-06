using System;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public partial class NativeCanvas {

		public void DrawFillPaint(Cairo.Context? context, Paint? paint, RectangleF rectangle) {
			if (paint == null || context == null)
				return;

			switch (paint) {

				case SolidPaint solidPaint: {
					FillColor = solidPaint.Color;

					break;
				}

				case LinearGradientPaint linearGradientPaint: {
					try {
						if (linearGradientPaint.GetCairoPattern(rectangle, DisplayScale) is { } pattern) {
							context.SetSource(pattern);
							pattern.Dispose();
						} else {
							FillColor = paint.BackgroundColor;
						}
					} catch (Exception exc) {
						Logger.Debug(exc);
						FillColor = linearGradientPaint.BlendStartAndEndColors();
					}

					break;
				}

				case RadialGradientPaint radialGradientPaint: {

					try {
						if (radialGradientPaint.GetCairoPattern(rectangle, DisplayScale) is { } pattern) {
							context.SetSource(pattern);
							pattern.Dispose();
						} else {
							FillColor = paint.BackgroundColor;
						}
					} catch (Exception exc) {
						Logger.Debug(exc);
						FillColor = radialGradientPaint.BlendStartAndEndColors();
					}

					break;
				}

				case PatternPaint patternPaint: {
					try {

#if UseSurfacePattern
						// would be nice to have: draw pattern without creating a pixpuf:

						using var paintSurface = CreateSurface(context, true);

						if (patternPaint.GetCairoPattern(paintSurface, DisplayScale) is { } pattern) {
							pattern.Extend = Cairo.Extend.Repeat;
							context.SetSource(pattern);
							pattern.Dispose();


						} else {
							FillColor = paint.BackgroundColor;
						}
#else
						using var pixbuf = patternPaint.GetPatternBitmap(DisplayScale);

						if (pixbuf?.CreatePattern(DisplayScale) is { } pattern) {
							pattern.Extend = Cairo.Extend.Repeat;
							context.SetSource(pattern);
							pattern.Dispose();
						}

#endif

					} catch (Exception exc) {
						Logger.Debug(exc);
						FillColor = paint.BackgroundColor;
					}

					break;
				}

				case ImagePaint {Image: GtkImage image} imagePaint: {
					var pixbuf = image.NativeImage;

					if (pixbuf?.CreatePattern(DisplayScale) is { } pattern) {
						try {

							context.SetSource(pattern);
							pattern.Dispose();

						} catch (Exception exc) {
							Logger.Debug(exc);
							FillColor = paint.BackgroundColor;
						}
					} else {
						FillColor = paint.BackgroundColor ?? Colors.White;
					}

					break;
				}

				case ImagePaint imagePaint:
					FillColor = paint.BackgroundColor ?? Colors.White;

					break;

				default:
					FillColor = paint.BackgroundColor ?? Colors.White;

					break;
			}
		}

	}

}
