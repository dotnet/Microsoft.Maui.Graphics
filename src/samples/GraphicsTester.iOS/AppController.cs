using UIKit;
using Elevenworks.Menus;
using GraphicsTester.Scenarios;

namespace GraphicsTester.iOS
{
	public class AppController : UIViewController
	{
		private UISplitViewController _splitViewController;
		private MenuViewController _menuViewController;
		private DetailViewController _detailViewController;
		private UINavigationController _masterNavigationController;
		private UINavigationController _detailNavigationController;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			var xg = new XfiniumPdfGenerator ();

			var graphicsViewSection = new Section();
			var xfiniumSection = new Section();

			foreach (var scenario in ScenarioList.Scenarios)
			{
				var name = scenario.GetType().Name;
				graphicsViewSection.Add(new ActionElement<GraphicsViewAction>(name, new GraphicsViewAction(scenario)));
				xfiniumSection.Add(new ActionElement<GeneratorAction>(name, new GeneratorAction(xg, scenario)));
			}
			
			var menu = new Section ("Scenarios") 
			{
				graphicsViewSection,
				xfiniumSection
			};

			_menuViewController = new MenuViewController (menu);
			_menuViewController.View.BackgroundColor = UIColor.GroupTableViewBackgroundColor;
			_masterNavigationController = new UINavigationController (_menuViewController);
			_masterNavigationController.NavigationBar.Translucent = false;

			_detailViewController = new DetailViewController ();
			_detailNavigationController = new UINavigationController (_detailViewController);
			_detailNavigationController.NavigationBar.Translucent = false;

			_splitViewController = new UISplitViewController ();
			_splitViewController.ViewControllers = new UIViewController[] 
			{
				_masterNavigationController,
				_detailNavigationController
			};

			_splitViewController.WeakDelegate = _detailViewController;
			this.AddFullSizeChildViewController(_splitViewController);

			_menuViewController.MenuContext = _detailViewController;
		}
	}
}

