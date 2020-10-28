using System;
using System.Collections;
using Android.Runtime;
using Android.Content;
using Android.Views;
using Android.Widget;
using Elevenworks.Graphics;
using GraphicsTester.Scenarios;
using Xamarin.Graphics;

namespace GraphicsTester.Android
{
    public class MainView : LinearLayout
    {
        private readonly ListView _listView;
        private readonly MDGraphicsView _graphicsView;

        public MainView (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
        {
        }

        public MainView (Context context) : base (context)
        {
            Orientation = Orientation.Horizontal;

            _listView = new ListView (context);
            _listView.LayoutParameters = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.MatchParent,
                2.5f);
            base.AddView (_listView);

            _graphicsView = new MDGraphicsView (context);
            _graphicsView.BackgroundColor = StandardColors.White;
            _graphicsView.LayoutParameters = new LinearLayout.LayoutParams(
                ViewGroup.LayoutParams.WrapContent,
                ViewGroup.LayoutParams.MatchParent,
                1);
            base.AddView (_graphicsView);

            var adapter = new ArrayAdapter (context, Resource.Layout.ListViewItem, ScenarioList.Scenarios);
            _listView.Adapter = adapter;

            _listView.ItemClick += (sender, e) => 
            {
                var scenario = ScenarioList.Scenarios[e.Position];
                _graphicsView.Drawable = scenario;
            };
        }   
    }
}

