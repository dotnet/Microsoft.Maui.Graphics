using Elevenworks.Graphics;
using Foundation;
using UIKit;

namespace GraphicsTester.iOS
{
   public class DetailViewController : UIViewController
	{
		private UIPopoverController _popoverController;
		private UIWebView _webview;
		private MTGraphicsView _graphicsView;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();	
			_webview = new UIWebView ();
			_webview.ScalesPageToFit = false;
			View.AddFullSizeSubview (_webview);
			
			_graphicsView = new MTGraphicsView();
			View.AddFullSizeSubview(_graphicsView);
			_graphicsView.Hidden = true;
		}
			
		public void ShowFile(string path)
		{
			_graphicsView.Hidden = true;
			_webview.Hidden = false;
			
			var url = new NSUrl (path, false);
			var request = new NSUrlRequest (url);
			_webview.LoadRequest(request);
			_webview.ScalesPageToFit = true;
			_webview.ScrollView.SetZoomScale (1, true);
		}
		
		public void ShowDrawable(EWDrawable drawable)
		{
			_graphicsView.Hidden = false;
			_webview.Hidden = true;

			_graphicsView.Drawable = drawable;
		}

		[Export ("splitViewController:willHideViewController:withBarButtonItem:forPopoverController:")]
		public void WillHideViewController (
			UISplitViewController splitController,
			UIViewController viewController,
			UIBarButtonItem barButtonItem,
			UIPopoverController popoverController)
		{
			barButtonItem.Title = NSBundle.MainBundle.GetLocalizedString (
				"Master",
				"Master");
			NavigationItem.SetLeftBarButtonItem (barButtonItem, true);
			this._popoverController = popoverController;
		}

		[Export ("splitViewController:willShowViewController:invalidatingBarButtonItem:")]
		public void WillShowViewController (
			UISplitViewController svc,
			UIViewController vc,
			UIBarButtonItem button)
		{
			// Called when the view is shown again in the split view, invalidating the button and popover controller.
			NavigationItem.SetLeftBarButtonItem (null, true);
			_popoverController = null;
		}
	}
}

