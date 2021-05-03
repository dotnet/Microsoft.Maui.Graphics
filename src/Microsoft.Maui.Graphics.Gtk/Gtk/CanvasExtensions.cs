namespace Microsoft.Maui.Graphics.Native.Gtk {

	public static class CanvasExtensions {

		public static Cairo.LineJoin ToLineJoin(this LineJoin lineJoin) =>
			lineJoin switch {
				LineJoin.Bevel => Cairo.LineJoin.Bevel,
				LineJoin.Round => Cairo.LineJoin.Round,
				_ => Cairo.LineJoin.Miter
			};

		public static Cairo.LineCap ToLineCap(this LineCap lineCap) =>
			lineCap switch {
				LineCap.Butt => Cairo.LineCap.Butt,
				LineCap.Round => Cairo.LineCap.Round,
				_ => Cairo.LineCap.Square
			};

		public static Cairo.Antialias ToAntialias(bool antialias) => antialias ? Cairo.Antialias.Default : Cairo.Antialias.None;

	}

}
