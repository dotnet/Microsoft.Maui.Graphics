using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public class PatternFills : AbstractScenario
    {
        public PatternFills()
            : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            EWPattern pattern;
            using (var picture = new PictureCanvas(0, 0, 12, 12))
            {
                picture.StrokeColor = StandardColors.LimeGreen;

                picture.StrokeSize = 1f;
                picture.StrokeDashPattern = null;
                picture.DrawLine(0, 12, 12, 0);

                pattern = AddPictureAsPattern(picture.Picture, 12, 12);
            }

            canvas.SetFillPaint(pattern.AsPaint(), 0, 0, 0, 0);
            canvas.FillRectangle(50, 50, 500, 500);

            canvas.RestoreState();
        }

        private EWPattern AddPictureAsPattern(EWPicture picture, float stepX, float stepY)
        {
            return new EWPicturePattern(picture, stepX, stepY);
        }
    }
}