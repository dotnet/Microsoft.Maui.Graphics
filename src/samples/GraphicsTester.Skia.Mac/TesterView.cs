using AppKit;
using CoreGraphics;
using Elevenworks.Graphics;
using Xamarin.Graphics;

namespace GraphicsTester.Mac
{
    public class TesterView : NSView
    {
        private readonly NSTableView tableView;
        private readonly MMGraphicsView graphicsView;
        private readonly TesterTableViewSource tableSource;

        public TesterView (CGRect rect) : base(rect)
        {
            GraphicsPlatform.Register(MMGraphicsService.Instance);

            tableSource = new TesterTableViewSource ();
            tableSource.ScenarioSelected += (drawable) =>
            {
                graphicsView.Drawable = drawable;
                graphicsView.InvalidateDrawable();
            };

            tableView = new NSTableView (new CGRect (0, 0, 300, rect.Height));
            tableView.AddColumn (new NSTableColumn ()
            {
                Width = 300,
            });
            tableView.Source = tableSource;
            tableView.BackgroundColor = NSColor.White;

            AddSubview (tableView);

            graphicsView = new MMGraphicsView ();
            AddSubview (graphicsView);

            Layout ();

            tableView.SelectRow (0, false);
        }

        public override bool IsFlipped => true;

        public override void Layout ()
        {
            var bounds = Bounds;
            tableView.Frame = new CGRect (0, 24, 300, bounds.Height-24);
            graphicsView.Frame = new CGRect (300, 24, bounds.Width - 300, bounds.Height-24);
        }

        public override CGRect Frame
        {
            get => base.Frame;
            set
            {
                base.Frame = value;
                Layout ();
            }
        }
    }
}

