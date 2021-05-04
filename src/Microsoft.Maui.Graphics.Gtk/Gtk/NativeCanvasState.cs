using System;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeCanvasState : CanvasState {

		public NativeCanvasState() {
			Alpha = 1;
			StrokeColor = Colors.Black.ToCairoColor();
			MiterLimit = 10;
			LineJoin = Cairo.LineJoin.Miter;
			LineCap = Cairo.LineCap.Butt;
		}

		public NativeCanvasState(NativeCanvasState prototype) {

			StrokeDashPattern = prototype.StrokeDashPattern;
			StrokeSize = prototype.StrokeSize;
			Scale = prototype.Scale;
			Transform = prototype.Transform;

			Antialias = prototype.Antialias;
			MiterLimit = prototype.MiterLimit;
			StrokeColor = prototype.StrokeColor;
			LineCap = prototype.LineCap;
			LineJoin = prototype.LineJoin;
			FillColor = prototype.FillColor;
			FontName = prototype.FontName;
			FontSize = prototype.FontSize;
			BlendMode = prototype.BlendMode;
			Alpha = prototype.Alpha;

			Shadow = prototype.Shadow;
		}

		public Cairo.Antialias Antialias { get; set; }

		public double MiterLimit { get; set; }

		public Cairo.Color StrokeColor { get; set; }

		public Cairo.LineCap LineCap { get; set; }

		public Cairo.LineJoin LineJoin { get; set; }

		public Cairo.Color FillColor { get; set; }

		public Cairo.Color FontColor { get; set; }

		public string FontName { get; set; }

		public float FontSize { get; set; }

		public BlendMode BlendMode { get; set; }

		public float Alpha { get; set; }

		private readonly double[] zerodash = new double[0];

		public double[] NativeDash => StrokeDashPattern != null ? Array.ConvertAll(StrokeDashPattern, f => (double) f) : zerodash;

		public (SizeF offset, float blur, Color color) Shadow { get; set; }

	}

}


