// This is free and unencumbered software released into the public domain.
// Happy coding!!! - GtkSharp Team

using System;
using Gtk;
using Microsoft.Maui.Graphics.Platform.Gtk;

namespace Samples {

	class Program {

		public static Application App;
		public static Window Win;

		[STAThread]
		public static void Main(string[] args) {

			Application.Init();

			App = new Application("Microsoft.Maui.Graphics.Samples", GLib.ApplicationFlags.None);

			App.Startup += (s, e) => StartupTests();

			App.Startup += (s, e) => {

				Win = new MainWindow();
				App.AddWindow(Win);

				var menu = new GLib.Menu();

				menu.AppendItem(new GLib.MenuItem("About", "app.about"));
				menu.AppendItem(new GLib.MenuItem("Quit", "app.quit"));
				App.AppMenu = menu;

				var aboutAction = new GLib.SimpleAction("about", null);
				aboutAction.Activated += AboutActivated;
				App.AddAction(aboutAction);

				var quitAction = new GLib.SimpleAction("quit", null);
				quitAction.Activated += QuitActivated;
				App.AddAction(quitAction);

				Win.ShowAll();
			};

			App.Activated += (s, e) => {
				App.Windows[0].Present();
			};

			((GLib.Application) App).Run();
		}

		private static void StartupTests() {
			StartupTest.InitTests();
		}

		private static void AboutActivated(object sender, EventArgs e) {
			var dialog = new AboutDialog {
				TransientFor = Win,
				ProgramName = $"{nameof(GtkGraphicsView)} Sample Application",
				Version = "1.0.0.0",
				Comments = $"A gtk sample application for the {typeof(Microsoft.Maui.Graphics.Point).Namespace} project.",
				LogoIconName = "system-run-symbolic",
				License = "This sample application is licensed under public domain.",
				Website = "https://www.github.com/dotnet/Microsoft.Maui.Graphics",
				WebsiteLabel = "Microsoft.Maui.Graphics Website"
			};

			dialog.Run();
			dialog.Hide();
		}

		private static void QuitActivated(object sender, EventArgs e) {
			((GLib.Application) App).Quit();
		}

	}

}
