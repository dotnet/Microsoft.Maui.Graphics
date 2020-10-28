using Elevenworks;
using Elevenworks.Services;
using Elevenworks.Graphics;
using Foundation;
using UIKit;

namespace GraphicsTester.iOS
{
	[Register ("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		private UIWindow _window;
		private AppController _appController;

		public override bool FinishedLaunching (
			UIApplication application,
			NSDictionary launchOptions)
		{
            ServiceContainer.Register<IPopoverService>(() => new MTPopoverService());
            ServiceContainer.Register<IGraphicsService>(MTGraphicsService.Instance);

			_window = new UIWindow (UIScreen.MainScreen.Bounds);

			_appController = new AppController ();
			_window.RootViewController = _appController;
			_window.MakeKeyAndVisible ();

			return true;
		}
	}
}

