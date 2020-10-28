using UIKit;

namespace GraphicsTester.iOS
{
	public static class UiViewExtensions
	{
		public static void AddFullSizeChildViewController(this UIViewController parent, UIViewController childController)
		{
			parent.AddChildViewController (childController);
			parent.View.AddFullSizeSubview (childController.View);
		}

		public static void AddFullSizeSubview(this UIView target, UIView subview)
		{
			target.AutosizesSubviews = true;

			subview.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
			subview.Frame = target.Bounds;
			target.AddSubview (subview);
		}
	}
}

