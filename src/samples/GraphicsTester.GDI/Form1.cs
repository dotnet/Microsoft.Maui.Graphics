using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elevenworks.Graphics;
using GraphicsTester.Scenarios;

namespace GraphicsTester.GDI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            gdiGraphicsView1.Renderer = new GDIDirectGraphicsRenderer() {BackgroundColor = StandardColors.White};
            foreach (var scenario in ScenarioList.Scenarios)
            {
                listBox1.Items.Add(scenario);
            }

            listBox1.SelectedIndexChanged += delegate(object sender, EventArgs args)
            {
                var item = ScenarioList.Scenarios[listBox1.SelectedIndex];
                gdiGraphicsView1.Drawable = item;
            };
        }
    }
}
