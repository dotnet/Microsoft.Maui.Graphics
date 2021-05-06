using System;
using Context = Cairo.Context;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public partial class NativeCanvas {

		public TextLayout CreateTextLayout() {
			var tl = new TextLayout(Context);
			tl.SetCanvasState(CurrentState);

			return tl;
		}

		public override void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment) {

			using var tl = CreateTextLayout();
			tl.HorizontalAlignment = horizontalAlignment;

			tl.DrawString(value, x, y);
		}

	}

}
