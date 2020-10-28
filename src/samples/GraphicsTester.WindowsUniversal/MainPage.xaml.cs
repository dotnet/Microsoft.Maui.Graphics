using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Elevenworks.Graphics;
using Elevenworks.Graphics.Win2D;
using GraphicsTester.Scenarios;
using Xamarin.Graphics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GraphicsTester.WindowsUniversal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            GraphicsPlatform.RegisterGlobalService(W2DGraphicsService.Instance);
            Fonts.Register(W2DFontService.Instance);

            GraphicsView.Background = new SolidColorBrush(Colors.White);

            foreach (var scenario in ScenarioList.Scenarios)
            {
                List.Items.Add(scenario);
            }
            List.SelectionChanged += (source, args) => Drawable = List.SelectedItem as EWDrawable;

            List.SelectedIndex = 0;

            this.SizeChanged += (source, args) => Draw();
        }

        public EWDrawable Drawable
        {
            set
            {
                GraphicsView.Drawable = value;
                Draw();
            }
        }

        private void Draw()
        {
            GraphicsView.Invalidate();
        }
    }
}
