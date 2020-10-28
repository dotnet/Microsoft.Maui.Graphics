using Xamarin.Graphics;

namespace GraphicsTester.Scenarios
{
    public class DrawShortTextInRect : AbstractScenario
    {
        public DrawShortTextInRect()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.StrokeSize = 1;
            canvas.StrokeColor = StandardColors.Blue;
            canvas.FontName = "Arial";
            canvas.FontSize = 12f;

            const string textShort = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";

            canvas.SaveState();
            canvas.SetShadow(new EWSize(2, 2), 2, StandardColors.DarkGrey, 1);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    float dx = x * 200;
                    float dy = y * 150;

                    canvas.DrawRectangle(dx, dy, 190, 140);

                    var horizontalAlignment = (EwHorizontalAlignment) x;
                    var verticalAlignment = (EwVerticalAlignment) y;

                    canvas.DrawString(textShort, dx, dy, 190, 140, horizontalAlignment, verticalAlignment);
                }
            }

            canvas.RestoreState();
        }
    }
}