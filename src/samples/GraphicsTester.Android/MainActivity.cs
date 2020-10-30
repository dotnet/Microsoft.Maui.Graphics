﻿using System.Graphics;
using System.Graphics.Android;
using Android.App;
using Android.Widget;
using Android.OS;

namespace GraphicsTester.Android
{
    [Activity (
        Label = "GraphicsTester.Android",
        MainLauncher = true,
        Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private MainView _mainView;

        protected override void OnCreate (Bundle bundle)
        {
            base.OnCreate (bundle);

            GraphicsPlatform.Register (MDGraphicsService.Instance);

            _mainView = new MainView (this);
            // Defining the LinearLayout layout parameters to fill the parent.
            LinearLayout.LayoutParams llp = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.MatchParent);

            SetContentView (_mainView, llp);
        }
    }
}


