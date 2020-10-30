using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class ArcScenario1 : AbstractScenario
    {
        public readonly bool includeOvals;

        public ArcScenario1(bool includeOvals = false) : base(720, 1024)
        {
            this.includeOvals = includeOvals;
        }

        public override void Draw(ICanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            if (includeOvals)
            {
                canvas.StrokeColor = StandardColors.LightGrey;
                canvas.DrawOval(50, 50, 150, 20);
                canvas.StrokeColor = StandardColors.Black;
            }

            canvas.DrawArc(
                50, // x
                50, // y
                150, // width
                20, // height
                45, // startAngle
                180, // endAngle 
                false, // clockwise
                false); // closed

            /*canvas.DrawArc(
                200,     // x
                50,     // y
                150,    // width
                20,     // height
                45,     // startAngle
                180,    // endAngle 
                true,   // clockwise
                false); // closed
   
            canvas.DrawArc(
                50,     // x
                200,    // y
                150,    // width
                100,    // height
                45,     // startAngle
                180,    // endAngle 
                false,  // clockwise
                false); // closed
   
            canvas.DrawArc(
                200,     // x
                200,     // y
                150,    // width
                100,     // height
                45,     // startAngle
                180,    // endAngle 
                true,   // clockwise
                false); // closed*/

            canvas.RestoreState();
        }
    }
}