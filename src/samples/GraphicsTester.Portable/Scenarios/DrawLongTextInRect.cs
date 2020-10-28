﻿using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawLongTextInRect : AbstractScenario
    {
        public DrawLongTextInRect()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.StrokeSize = 1;
            canvas.StrokeColor = StandardColors.Blue;
            canvas.FontName = "Arial";
            canvas.FontSize = 12f;

            const string textLong =
                "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    float dx = x * 200;
                    float dy = 0 + y * 150;

                    canvas.DrawRectangle(dx, dy, 190, 140);

                    var horizontalAlignment = (EWHorizontalAlignment) x;
                    var verticalAlignment = (EWVerticalAlignment) y;

                    canvas.DrawString(textLong, dx, dy, 190, 140, horizontalAlignment, verticalAlignment);
                }
            }
        }
    }
}