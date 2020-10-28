using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawVerticallyCenteredText2 : AbstractScenario
    {
        public DrawVerticallyCenteredText2()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.StrokeSize = 1;
            canvas.StrokeColor = StandardColors.Blue;
            canvas.FontName = "Arial";
            canvas.FontSize = 24f;

            canvas.SaveState();

            var rectHeight = 32;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 1", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 2", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 3", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 4", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(300, 0);

            rectHeight = 32;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 1", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 2", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 3", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 4", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(600, 0);

            rectHeight = 32;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 1", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 2", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 3", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Really Short 4", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(0, 300);
            canvas.SetToSystemFont();

            rectHeight = 32;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 1", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 2", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 3", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 4", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.CENTER);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(300, 300);
            canvas.SetToSystemFont();

            rectHeight = 32;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 1", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 2", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 3", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 4", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.TOP);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            canvas.RestoreState();

            canvas.SaveState();
            canvas.Translate(600, 300);
            canvas.SetToSystemFont();

            rectHeight = 32;
            canvas.Translate(0, 10);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 1", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 2", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 3", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            rectHeight -= 2;
            canvas.Translate(0, 45);
            canvas.DrawRectangle(10, 0, 200, rectHeight);
            canvas.DrawString("Sys Font R 4", 0, 10, 0, 200, rectHeight, EWHorizontalAlignment.CENTER, EWVerticalAlignment.BOTTOM);
            canvas.DrawLine(10, rectHeight / 2, 200, rectHeight / 2);

            canvas.RestoreState();
        }
    }
}