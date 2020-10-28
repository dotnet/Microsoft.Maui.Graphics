using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Elevenworks.Graphics;
using GraphicsTester.Scenarios;

namespace GraphicsTester.Xaml
{
    public partial class MainWindow : Window
    {
        private readonly XamlCanvas canvas = new XamlCanvas();
        private EWDrawable drawable;

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            canvas.Canvas = Canvas;

            foreach (var scenario in ScenarioList.Scenarios)
            {
                List.Items.Add(scenario);
            }
            List.SelectionChanged += (source, args) => Drawable = List.SelectedItem as EWDrawable;

            List.SelectedIndex = 0;

            this.SizeChanged += (source,args) => Draw();
        }

        public EWDrawable Drawable
        {
            get { return drawable; }
            set
            {
                drawable = value;
                Draw();
            }
        }

        private void Draw()
        {
            if (drawable != null)
            {
                using (canvas.CreateSession())
                {
                    drawable.Draw(canvas, new EWRectangle(0, 0, (float) Canvas.Width, (float) Canvas.Height));
                }
            }            
        }
    }
}
