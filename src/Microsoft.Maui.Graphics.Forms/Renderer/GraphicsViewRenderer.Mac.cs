using System.ComponentModel;
using Microsoft.Maui.Graphics.Forms;
using Microsoft.Maui.Graphics.Forms.Mac;
using Microsoft.Maui.Graphics.Platform;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;

[assembly: ExportRenderer(typeof(GraphicsView), typeof(GraphicsViewRenderer))]
namespace Microsoft.Maui.Graphics.Forms.Mac
{
	[Preserve]
	public class GraphicsViewRenderer : ViewRenderer<GraphicsView, PlatformGraphicsView>
	{
		protected override void OnElementChanged(ElementChangedEventArgs<GraphicsView> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				// Unsubscribe from event handlers and cleanup any resources
				SetNativeControl(null);
			}

			if (e.NewElement != null)
			{
				SetNativeControl(new PlatformGraphicsView());
				UpdateDrawable();
			}
		}

		protected override void OnElementPropertyChanged(
			object sender,
			PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == nameof(GraphicsView.Drawable))
				UpdateDrawable();
		}

		private void UpdateDrawable()
		{
			Control.Drawable = Element.Drawable;
		}
	}
}
