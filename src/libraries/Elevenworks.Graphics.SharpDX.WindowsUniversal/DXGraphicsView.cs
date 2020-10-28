﻿using SharpDX.Direct2D1;

#if WINDOWS_UWP
using Windows.Graphics.Display;
#endif

namespace Elevenworks.Graphics.SharpDX
{
    public class DXGraphicsView : DirectXPanelBase
    {
        private readonly DXCanvas canvas;
        private EWDrawable drawable;
        private EWColor backgroundColor;

        public DXGraphicsView()
        {
            canvas = new DXCanvas();

#if WINDOWS_UWP
            var displayInformation = DisplayInformation.GetForCurrentView();
            canvas.Dpi = displayInformation.LogicalDpi;
#endif

            BackgroundColor = StandardColors.Green;
        }

        public EWDrawable Drawable
        {
            get => drawable;
            set
            {
                drawable = value;
                Invalidate();
            }
        }

        public DXCanvas Canvas => canvas;

        public EWColor BackgroundColor
        {
            get => backgroundColor;
            set => backgroundColor = value;
        }

        protected override void Draw(DeviceContext context)
        {
            DXGraphicsService.CurrentFactory.Value = context.Factory;
            DXGraphicsService.CurrentTarget.Value = context;

            RenderTransform = null;
            canvas.RenderTarget = context;
            var scale = canvas.DisplayScale;
            var bounds = new EWRectangle(0, 0, (float) ActualWidth * scale, (float) ActualHeight * scale);

            if (backgroundColor != null)
            {
                canvas.FillColor = backgroundColor;
                canvas.FillRectangle(bounds);
                canvas.FillColor = StandardColors.White;
            }

            drawable.Draw(canvas, bounds);

            DXGraphicsService.CurrentTarget.Value = null;
            DXGraphicsService.CurrentFactory.Value = null;
        }
    }
}