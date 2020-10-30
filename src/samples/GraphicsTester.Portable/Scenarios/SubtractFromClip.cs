﻿using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class SubtractFromClip : AbstractScenario
    {
        public SubtractFromClip() : base(720, 1024)
        {
        }

        public override void Draw(ICanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            canvas.SubtractFromClip(100, 100, 100, 100);
            canvas.FillColor = StandardColors.CornflowerBlue;
            canvas.FillRectangle(0, 0, 300, 300);

            canvas.RestoreState();

            canvas.FillColor = StandardColors.Salmon;
            canvas.FillRectangle(120, 120, 60, 60);
        }
    }
}