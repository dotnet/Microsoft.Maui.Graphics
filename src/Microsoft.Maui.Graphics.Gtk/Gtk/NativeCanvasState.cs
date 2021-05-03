namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeCanvasState : CanvasState {

		public NativeCanvasState() { }

		public NativeCanvasState(NativeCanvasState prototype) { }

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

	}

}
