﻿using System;
using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public abstract class AbstractFillScenario : AbstractScenario
    {
        private readonly Action<EWCanvas, float, float, float, float> action;

        protected AbstractFillScenario(Action<EWCanvas, float, float, float, float> action)
            : base(720, 1024)
        {
            this.action = action;
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            FillRectanglesOfDifferentSizesAndColors(canvas);
            FillRectanglesWithAlpha(canvas);
            FillShadowedRect(canvas);
        }

        private void FillShadowedRect(EWCanvas canvas)
        {
            canvas.SaveState();
            canvas.FillColor = StandardColors.Black;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            action(canvas, 50.5f, 400.5f, 200, 50);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.FillColor = StandardColors.CornflowerBlue;
            canvas.SetShadow(EWCanvas.DefaultShadowOffset, EWCanvas.DefaultShadowBlur, EWCanvas.DefaultShadowColor, 1);
            action(canvas, 50.5f, 500.5f, 200, 50);
            canvas.RestoreState();
        }

        private void FillRectanglesOfDifferentSizesAndColors(EWCanvas canvas)
        {
            canvas.FillColor = StandardColors.Salmon;
            for (int i = 1; i < 5; i++)
            {
                action(canvas, 50, 50 + i * 30, 150, 20);
            }

            canvas.FillColor = StandardColors.CornflowerBlue;
            for (int i = 1; i < 5; i++)
            {
                action(canvas, 250.5f, 50.5f + i * 30, 150, 20);
            }
        }

        private void FillRectanglesWithAlpha(EWCanvas canvas)
        {
            canvas.FillColor = StandardColors.Black;
            for (int i = 1; i <= 10; i++)
            {
                canvas.Alpha = (float) i / 10f;
                action(canvas, 450f, 200f + i * 30, 150, 20);
            }

            canvas.Alpha = 1;
        }
    }
}