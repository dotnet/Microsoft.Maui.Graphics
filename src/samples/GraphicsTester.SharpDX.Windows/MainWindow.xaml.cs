﻿using System.Windows;
using Elevenworks.Graphics;
using Elevenworks.Graphics.SharpDX;
using GraphicsTester.Scenarios;

namespace GraphicsTester.WPF.SharpDX
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        public void Initialize()
        {
            GraphicsPlatform.RegisterGlobalService(DXGraphicsService.Instance);
            GraphicsView.BackgroundColor = StandardColors.White;

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