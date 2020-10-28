
using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Elevenworks.Graphics;

namespace GraphicsTester.Android
{
    [Activity (
        Label = "GraphicsTester.Android",
        MainLauncher = true,
        Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MainView mainView;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            GraphicsPlatform.Register (MDGraphicsService.Instance);

            mainView = new MainView (this);
            // Defining the LinearLayout layout parameters to fill the parent.
            LinearLayout.LayoutParams llp = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.MatchParent);

            SetContentView (mainView, llp);
        }
    }
}


