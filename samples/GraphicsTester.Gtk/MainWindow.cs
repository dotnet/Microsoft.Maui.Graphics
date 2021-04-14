// This is free and unencumbered software released into the public domain.
// Happy coding!!! - GtkSharp Team

using Gtk;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;

namespace Samples
{
    class MainWindow : Window
    {
        private HeaderBar _headerBar;
        private TreeView _treeView;
        private Box _boxContent;
        private TreeStore _store;
        private Dictionary<string, (Type type, Widget widget)> _items;
        private Notebook _notebook;
        private GtkSkiaGraphicsView _skiaGraphicsView;
        private GtkSkiaDirectRenderer _skiaGraphicsRenderer;
        public MainWindow() : base(WindowType.Toplevel)
        {
            // Setup GUI
            WindowPosition = WindowPosition.Center;
            DefaultSize = new Gdk.Size(800, 600);

            _headerBar = new HeaderBar();
            _headerBar.ShowCloseButton = true;
            _headerBar.Title = "GtkSharp Sample Application";

            var btnClickMe = new Button();
            btnClickMe.AlwaysShowImage = true;
            btnClickMe.Image = Image.NewFromIconName("document-new-symbolic", IconSize.Button);
            _headerBar.PackStart(btnClickMe);

            Titlebar = _headerBar;

            var hpanned = new HPaned();
            hpanned.Position = 200;

            _treeView = new TreeView();
            _treeView.HeadersVisible = false;
            hpanned.Pack1(_treeView, false, true);

            _notebook = new Notebook();

            var scroll1 = new ScrolledWindow();
            var vpanned = new VPaned();
            vpanned.Position = 300;
            _boxContent = new Box(Orientation.Vertical, 0);
            _boxContent.Margin = 8;
            vpanned.Pack1(_boxContent, true, true);

            Fonts.Register(new SkiaFontService("",""));
            GraphicsPlatform.RegisterGlobalService(SkiaGraphicsService.Instance);

            _skiaGraphicsView = new GtkSkiaGraphicsView ();
            _skiaGraphicsRenderer = new GtkSkiaDirectRenderer ();
            _skiaGraphicsView.Renderer = _skiaGraphicsRenderer;

            vpanned.Pack2(_skiaGraphicsView, false, true);
            scroll1.Child = vpanned;
            _notebook.AppendPage(scroll1, new Label { Text = "Data", Expand = true });

            var scroll2 = new ScrolledWindow();


            _notebook.AppendPage(scroll2, new Label { Text = "Code", Expand = true });

            hpanned.Pack2(_notebook, true, true);

            Child = hpanned;

            // Fill up data
            FillUpTreeView();

            // Connect events
            _treeView.Selection.Changed += Selection_Changed;
            Destroyed += (sender, e) => Application.Quit();
        }

        private void Selection_Changed(object sender, EventArgs e)
        {
            if (_treeView.Selection.GetSelected(out TreeIter iter))
            {
                var s = _store.GetValue(iter, 0).ToString();

                while (_boxContent.Children.Length > 0)
                    _boxContent.Remove(_boxContent.Children[0]);
                _notebook.CurrentPage = 0;
                _notebook.ShowTabs = false;

                if (_items.TryGetValue(s, out var item))
                {
                    _notebook.ShowTabs = true;

                    if (item.widget == null)
                        _items[s] = item = (item.type, Activator.CreateInstance(item.type) as Widget);


                    _boxContent.PackStart(item.widget, true, true, 0);
                    _boxContent.ShowAll();
                }

            }
        }

        private void FillUpTreeView()
        {
            // Init cells
            var cellName = new CellRendererText();

            // Init columns
            var columeSections = new TreeViewColumn();
            columeSections.Title = "Sections";
            columeSections.PackStart(cellName, true);

            columeSections.AddAttribute(cellName, "text", 0);

            _treeView.AppendColumn(columeSections);

            // Init treeview
            _store = new TreeStore(typeof(string));
            _treeView.Model = _store;


            _treeView.ExpandAll();
        }
    }
}
