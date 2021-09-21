using System;

namespace SkiaSharp.Views.Blazor
{
	public class SKPaintGLSurfaceEventArgs : EventArgs
	{
		public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTarget renderTarget)
			: this(surface, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888)
		{
		}

		public SKPaintGLSurfaceEventArgs(SKSurface surface, GRBackendRenderTarget renderTarget, GRSurfaceOrigin origin, SKColorType colorType)
		{
			Surface = surface;
			BackendRenderTarget = renderTarget;
			ColorType = colorType;
			Origin = origin;
		}

		public SKSurface Surface { get; }

		public GRBackendRenderTarget BackendRenderTarget { get; }

		public SKColorType ColorType { get; }

		public GRSurfaceOrigin Origin { get; }
	}
}
