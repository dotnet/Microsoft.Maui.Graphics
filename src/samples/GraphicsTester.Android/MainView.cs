using System;
using System.Collections;
using Android.Runtime;
using Android.Content;
using Android.Widget;
using Elevenworks.Graphics;
using GraphicsTester.Scenarios;

namespace GraphicsTester.Android
{
    public class MainView : LinearLayout
    {
        private ListView listView;
        private MDGraphicsView graphicsView;

        public MainView (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
        {
        }

        public MainView (Context context) : base (context)
        {
            Orientation = Orientation.Horizontal;

            listView = new ListView (context);
            listView.LayoutParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WrapContent,
                LinearLayout.LayoutParams.MatchParent,
                2.5f);
            base.AddView (listView);

            graphicsView = new MDGraphicsView (context);
            graphicsView.BackgroundColor = StandardColors.White;
            graphicsView.LayoutParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WrapContent,
                LinearLayout.LayoutParams.MatchParent,
                1);
            base.AddView (graphicsView);

            var adapter = new ArrayAdapter (context, Resource.Layout.ListViewItem, ScenarioList.Scenarios as IList);
            listView.Adapter = adapter;

            listView.ItemClick += (sender, e) => 
            {
                var scenario = ScenarioList.Scenarios[e.Position];
                graphicsView.Drawable = scenario;
            };
        }   
    }
}

