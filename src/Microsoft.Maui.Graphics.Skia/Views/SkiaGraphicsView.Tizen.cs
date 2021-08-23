using SkiaSharp.Views.Tizen;
using ElmSharp;

namespace Microsoft.Maui.Graphics.Skia.Views
{
	public class SkiaGraphicsView : SKCanvasView
	{
		private IDrawable _drawable;
		private SkiaCanvas _canvas;
		private ScalingCanvas _scalingCanvas;

		public SkiaGraphicsView(EvasObject parent, IDrawable drawable = null) : base(parent)
		{
			_canvas = new SkiaCanvas();
			_scalingCanvas = new ScalingCanvas(_canvas);
			Drawable = drawable;
			PaintSurface += OnPaintSurface;
		}

		public float DeviceScalingFactor { get; set; }

		public IDrawable Drawable
		{
			get => _drawable;
			set
			{
				_drawable = value;
				Invalidate();
			}
		}

		protected virtual void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
		{
			if (_drawable == null) return;

			var skiaCanvas = e.Surface.Canvas;
			skiaCanvas.Clear();

			_canvas.Canvas = skiaCanvas;
			_scalingCanvas.ResetState();

			float width = e.Info.Width;
			float height = e.Info.Height;
			if (DeviceScalingFactor > 0)
			{
				width = width / DeviceScalingFactor;
				height = height / DeviceScalingFactor;
			}

			_scalingCanvas.SaveState();
			if (DeviceScalingFactor > 0)
				_scalingCanvas.Scale(DeviceScalingFactor, DeviceScalingFactor);
			_drawable.Draw(_scalingCanvas, new RectangleF(0, 0, width, height));
			_scalingCanvas.RestoreState();
		}
	}
}
