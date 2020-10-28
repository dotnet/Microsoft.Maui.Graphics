using System;
using System.Windows.Forms;
using Elevenworks.Graphics;
using Xamarin.Graphics;

namespace GraphicsTester.GDI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GraphicsPlatform.RegisterGlobalService(new GDIGraphicsService());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
