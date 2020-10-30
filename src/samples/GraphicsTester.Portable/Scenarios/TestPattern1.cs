using System.Graphics;

namespace GraphicsTester.Scenarios
{
    public class TestPattern1 : AbstractScenario
    {
        public TestPattern1() : base(720, 1024)
        {
        }

        public override void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            canvas.SaveState();

            //
            // DrawXXXX Methods
            //
            canvas.StrokeColor = StandardColors.Grey;
            canvas.DrawLine(0, 0, 100, 100);
            canvas.DrawRectangle(100, 0, 100, 100);
            canvas.DrawOval(200, 0, 100, 100);
            canvas.DrawRoundedRectangle(300, 0, 100, 100, 25);

            var path = new EWPath();
            path.MoveTo(400, 0);
            path.LineTo(400, 100);
            path.QuadTo(500, 100, 500, 0);
            path.CurveTo(450, 0, 500, 50, 450, 50);
            canvas.DrawPath(path);

            canvas.DrawRectangle(500, 0, 100, 50);
            canvas.DrawOval(600, 0, 100, 50);
            canvas.DrawRoundedRectangle(700, 0, 100, 50, 25);
            canvas.DrawRoundedRectangle(800, 0, 100, 25, 25);

            // 
            // FillXXXX Methods
            //

            canvas.FillColor = StandardColors.Red;
            canvas.FillRectangle(210, 210, 80, 80);

            canvas.FillColor = StandardColors.Green;
            canvas.FillOval(310, 210, 80, 80);

            canvas.FillColor = StandardColors.Blue;
            canvas.FillRoundedRectangle(410, 210, 80, 80, 10);

            canvas.FillColor = StandardColors.CornflowerBlue;
            path = new EWPath();
            path.MoveTo(510, 210);
            path.LineTo(550, 290);
            path.LineTo(590, 210);
            path.Close();
            canvas.FillPath(path);

            canvas.FillColor = StandardColors.White;

            //
            // EWStrokeLocation.CENTER
            //
            canvas.StrokeLocation = EWStrokeLocation.CENTER;
            canvas.StrokeSize = 10;
            canvas.DrawRectangle(50, 400, 100, 50);
            canvas.DrawOval(200, 400, 100, 50);
            canvas.DrawRoundedRectangle(350, 400, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 400);
            path.LineTo(500, 450);
            path.LineTo(600, 450);
            path.Close();
            canvas.DrawPath(path);

            //
            // EWStrokeLocation.INSIDE
            //
            canvas.StrokeLocation = EWStrokeLocation.INSIDE;
            canvas.DrawRectangle(50, 500, 100, 50);
            canvas.DrawOval(200, 500, 100, 50);
            canvas.DrawRoundedRectangle(350, 500, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 500);
            path.LineTo(500, 550);
            path.LineTo(600, 550);
            path.Close();
            canvas.DrawPath(path);

            //
            // EWStrokeLocation.OUTSIDE
            //
            canvas.StrokeLocation = EWStrokeLocation.OUTSIDE;
            canvas.DrawRectangle(50, 600, 100, 50);
            canvas.DrawOval(200, 600, 100, 50);
            canvas.DrawRoundedRectangle(350, 600, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 600);
            path.LineTo(500, 650);
            path.LineTo(600, 650);
            path.Close();
            canvas.DrawPath(path);

            //
            // Stroke Color and Line Caps
            //

            canvas.StrokeColor = StandardColors.CornflowerBlue;
            canvas.DrawLine(100, 120, 300, 120);

            canvas.StrokeColor = StandardColors.Red;
            canvas.StrokeLineCap = EWLineCap.BUTT;
            canvas.DrawLine(100, 140, 300, 140);

            canvas.StrokeColor = StandardColors.Green;
            canvas.StrokeLineCap = EWLineCap.ROUND;
            canvas.DrawLine(100, 160, 300, 160);

            canvas.StrokeColor = StandardColors.Blue;
            canvas.StrokeLineCap = EWLineCap.SQUARE;
            canvas.DrawLine(100, 180, 300, 180);

            canvas.StrokeLineCap = EWLineCap.BUTT;

            //
            // Line Joins
            //

            canvas.StrokeColor = StandardColors.Black;
            canvas.StrokeLocation = EWStrokeLocation.CENTER;

            path = new EWPath();
            path.MoveTo(350, 120);
            path.LineTo(370, 180);
            path.LineTo(390, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.MITER;
            path = new EWPath();
            path.MoveTo(400, 120);
            path.LineTo(420, 180);
            path.LineTo(440, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.ROUND;
            path = new EWPath();
            path.MoveTo(450, 120);
            path.LineTo(470, 180);
            path.LineTo(490, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.BEVEL;
            path = new EWPath();
            path.MoveTo(500, 120);
            path.LineTo(520, 180);
            path.LineTo(540, 120);
            canvas.DrawPath(path);

            canvas.StrokeLineJoin = EWLineJoin.MITER;
            canvas.MiterLimit = 2;
            path = new EWPath();
            path.MoveTo(550, 120);
            path.LineTo(570, 180);
            path.LineTo(590, 120);
            canvas.DrawPath(path);

            canvas.MiterLimit = EWCanvas.DefaultMiterLimit;

            //
            // Stroke Dash Pattern
            //
            canvas.StrokeSize = 1;
            canvas.StrokeDashPattern = DOTTED;
            canvas.DrawLine(650, 120, 800, 120);

            canvas.StrokeSize = 3;
            canvas.StrokeDashPattern = DOTTED;
            canvas.DrawLine(650, 140, 800, 140);

            canvas.StrokeDashPattern = DASHED_DOT;
            canvas.DrawLine(650, 160, 800, 160);

            canvas.StrokeDashPattern = SOLID;
            canvas.DrawLine(650, 180, 800, 180);

            canvas.StrokeLineCap = EWLineCap.BUTT;

            //
            // Linear Gradient Fill
            //

            canvas.StrokeLocation = EWStrokeLocation.CENTER;

            var vPaint = new EWPaint
            {
                PaintType = EWPaintType.LINEAR_GRADIENT,
                StartColor = StandardColors.White,
                EndColor = StandardColors.Black
            };

            canvas.SetFillPaint(vPaint, 50, 700, 150, 750);
            canvas.FillRectangle(50, 700, 100, 50);

            canvas.SetFillPaint(vPaint, 200, 700, 300, 700);
            canvas.FillOval(200, 700, 100, 50);

            vPaint.AddOffset(.5f, StandardColors.IndianRed);
            canvas.SetFillPaint(vPaint, 350, 700, 450, 700);
            canvas.FillRoundedRectangle(350, 700, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 700);
            path.LineTo(500, 750);
            path.LineTo(600, 750);
            path.Close();

            canvas.SetFillPaint(vPaint, 500, 700, 600, 700);
            canvas.FillPath(path);

            //
            // Radial Gradient Fill
            //

            canvas.StrokeLocation = EWStrokeLocation.CENTER;

            vPaint = new EWPaint
            {
                PaintType = EWPaintType.RADIAL_GRADIENT,
                StartColor = StandardColors.White,
                EndColor = StandardColors.Black
            };

            canvas.SetFillPaint(vPaint, 100, 825, 150, 850);
            canvas.FillRectangle(50, 800, 100, 50);

            canvas.SetFillPaint(vPaint, 250, 825, 300, 800);
            canvas.FillOval(200, 800, 100, 50);

            vPaint.AddOffset(.5f, StandardColors.IndianRed);
            canvas.SetFillPaint(vPaint, 400, 825, 450, 800);
            canvas.FillRoundedRectangle(350, 800, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 800);
            path.LineTo(500, 850);
            path.LineTo(600, 850);
            path.Close();

            canvas.SetFillPaint(vPaint, 550, 825, 600, 800);
            canvas.FillPath(path);

            //
            // Solid Fill With Shadow
            //

            canvas.SaveState();
            canvas.FillColor = StandardColors.CornflowerBlue;
            canvas.SetShadow(new EWSize(5, 5), 0, StandardColors.Grey, 1);
            canvas.FillRectangle(50, 900, 100, 50);

            canvas.SetShadow(new EWSize(5, 5), 2, StandardColors.Red, 1);
            canvas.FillOval(200, 900, 100, 50);

            canvas.SetShadow(new EWSize(5, 5), 5, StandardColors.Green, 1);
            canvas.FillRoundedRectangle(350, 900, 100, 50, 25);

            canvas.SetShadow(new EWSize(10, 10), 5, StandardColors.Blue, 1);

            path = new EWPath();
            path.MoveTo(550, 900);
            path.LineTo(500, 950);
            path.LineTo(600, 950);
            path.Close();

            canvas.FillPath(path);

            //
            // Draw With Shadow
            //

            canvas.StrokeColor = StandardColors.Black;
            canvas.SetShadow(new EWSize(5, 5), 0, StandardColors.Grey, 1);
            canvas.DrawRectangle(50, 1000, 100, 50);

            canvas.SetShadow(new EWSize(5, 5), 2, StandardColors.Red, 1);
            canvas.DrawOval(200, 1000, 100, 50);

            canvas.SetShadow(new EWSize(5, 5), 5, StandardColors.Green, 1);
            canvas.DrawRoundedRectangle(350, 1000, 100, 50, 25);

            canvas.SetShadow(new EWSize(10, 10), 5, StandardColors.Blue, 1);
            path = new EWPath();
            path.MoveTo(550, 1000);
            path.LineTo(500, 1050);
            path.LineTo(600, 1050);
            path.Close();

            canvas.DrawPath(path);

            canvas.RestoreState();

            //
            // Solid Fill Without Shadow
            //

            canvas.FillColor = StandardColors.DarkOliveGreen;
            canvas.FillRectangle(50, 1100, 100, 50);
            canvas.FillOval(200, 1100, 100, 50);
            canvas.FillRoundedRectangle(350, 1100, 100, 50, 25);

            path = new EWPath();
            path.MoveTo(550, 1100);
            path.LineTo(500, 1150);
            path.LineTo(600, 1150);
            path.Close();

            canvas.FillPath(path);

            //
            // FILL WITH SHADOW USING ALPHA
            //

            canvas.SaveState();

            canvas.Alpha = .25f;
            canvas.FillColor = StandardColors.CornflowerBlue;
            canvas.SetShadow(new EWSize(5, 5), 0, StandardColors.Grey, 1);
            canvas.FillRectangle(50, 1200, 100, 50);

            canvas.Alpha = .5f;
            canvas.SetShadow(new EWSize(5, 5), 2, StandardColors.Red, 1);
            canvas.FillOval(200, 1200, 100, 50);

            canvas.Alpha = .75f;
            canvas.SetShadow(new EWSize(5, 5), 5, StandardColors.Green, 1);
            canvas.FillRoundedRectangle(350, 1200, 100, 50, 25);

            canvas.Alpha = 1;
            canvas.SetShadow(new EWSize(10, 10), 5, StandardColors.Blue, 1);

            path = new EWPath();
            path.MoveTo(550, 1200);
            path.LineTo(500, 1250);
            path.LineTo(600, 1250);
            path.Close();

            canvas.FillPath(path);
            canvas.RestoreState();

            //
            // Test Scaling
            //

            canvas.StrokeSize = 1;
            canvas.StrokeColor = StandardColors.Black;
            canvas.DrawLine(10, 0, 0, 10);

            canvas.SaveState();

            canvas.Scale(2, 2);
            canvas.DrawLine(10, 0, 0, 10);

            canvas.Scale(2, 2);
            canvas.DrawLine(10, 0, 0, 10);

            canvas.RestoreState();

            //
            // Test simple rotation relative to 0,0
            //

            canvas.SaveState();
            canvas.SetShadow(new EWSize(2, 0), 2, StandardColors.Black, 1);
            canvas.StrokeColor = StandardColors.CornflowerBlue;
            canvas.Rotate(15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.Rotate(15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.Rotate(15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.StrokeColor = StandardColors.DarkSeaGreen;
            canvas.Rotate(-60);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.Rotate(-15);
            canvas.DrawOval(60, 60, 10, 10);
            canvas.RestoreState();

            canvas.DrawRectangle(60, 60, 10, 10);

            //
            // Test rotation relative to a point
            //

            canvas.DrawRectangle(25, 125, 50, 50);

            canvas.SaveState();
            canvas.Rotate(5, 50, 150);
            canvas.DrawRectangle(25, 125, 50, 50);
            canvas.RestoreState();

            canvas.SaveState();
            canvas.Rotate(-5, 50, 150);
            canvas.DrawRectangle(25, 125, 50, 50);
            canvas.RestoreState();

            //
            // Test text
            //

            canvas.StrokeSize = 1;
            canvas.StrokeColor = StandardColors.Blue;

            const string vTextLong =
                "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            const string vTextShort = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    float dx = 1000 + x * 200;
                    float dy = 0 + y * 150;

                    canvas.DrawRectangle(dx, dy, 190, 140);

                    var vHorizontalAlignment = (EwHorizontalAlignment) x;
                    var vVerticalAlignment = (EwVerticalAlignment) y;

                    canvas.FontName = "Arial";
                    canvas.FontSize = 12f;
                    canvas.DrawString(vTextLong, dx, dy, 190, 140, vHorizontalAlignment, vVerticalAlignment);
                }
            }

            canvas.SaveState();
            canvas.SetShadow(new EWSize(2, 2), 2, StandardColors.DarkGrey, 1);

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    float dx = 1000 + x * 200;
                    float dy = 450 + y * 150;

                    canvas.DrawRectangle(dx, dy, 190, 140);

                    var vHorizontalAlignment = (EwHorizontalAlignment) x;
                    var vVerticalAlignment = (EwVerticalAlignment) y;

                    canvas.FontName = "Arial";
                    canvas.FontSize = 12f;
                    canvas.DrawString(vTextShort, dx, dy, 190, 140, vHorizontalAlignment, vVerticalAlignment);
                }
            }

            canvas.RestoreState();

            for (int y = 0; y < 3; y++)
            {
                float dx = 1000 + y * 200;
                const float dy = 1050;

                canvas.DrawRectangle(dx, dy, 190, 140);

                const EwHorizontalAlignment vHorizontalAlignment = EwHorizontalAlignment.Left;
                var vVerticalAlignment = (EwVerticalAlignment) y;

                canvas.FontName = "Arial";
                canvas.FontSize = 12f;
                canvas.DrawString(
                    vTextLong,
                    dx,
                    dy,
                    190,
                    140,
                    vHorizontalAlignment,
                    vVerticalAlignment,
                    EWTextFlow.OVERFLOW_BOUNDS);
            }

            //
            // Test simple drawing string
            //
            canvas.DrawLine(1000, 1300, 1200, 1300);
            canvas.DrawLine(1000, 1325, 1200, 1325);
            canvas.DrawLine(1000, 1350, 1200, 1350);
            canvas.DrawLine(1000, 1375, 1200, 1375);
            canvas.DrawLine(1100, 1300, 1100, 1400);
            canvas.DrawString("This is a test.", 1100, 1300, EwHorizontalAlignment.Left);
            canvas.DrawString("This is a test.", 1100, 1325, EwHorizontalAlignment.Center);
            canvas.DrawString("This is a test.", 1100, 1350, EwHorizontalAlignment.Right);
            canvas.DrawString("This is a test.", 1100, 1375, EwHorizontalAlignment.Justified);

            //
            // Test inverse clipping area
            //

            canvas.SaveState();
            canvas.DrawRectangle(200, 1300, 200, 50);
            canvas.SubtractFromClip(200, 1300, 200, 50);
            canvas.DrawLine(100, 1325, 500, 1325);
            canvas.DrawLine(300, 1275, 300, 1375);
            canvas.RestoreState();

            var graphicsPlatform = GraphicsPlatform.CurrentService;
            if (graphicsPlatform != null)
            {
                //
                // Test String Measuring
                //

                canvas.StrokeColor = StandardColors.Blue;
                for (int i = 0; i < 4; i++)
                {
                    canvas.FontSize = 12 + i * 6;
                    canvas.DrawString("Test String Length", 650, 400 + (100 * i), EwHorizontalAlignment.Left);

                    var size = graphicsPlatform.GetStringSize("Test String Length", "Arial", 12 + i * 6);
                    canvas.DrawRectangle(650, 400 + (100 * i), size.Width, size.Height);
                }

                //
                // Test Path Measuring
                //

                var vBuilder = new EWPathBuilder();
                path =
                    vBuilder.BuildPath(
                        "M0 52.5 C60 -17.5 60 -17.5 100 52.5 C140 122.5 140 122.5 100 152.5 Q60 182.5 0 152.5 Z");

                canvas.SaveState();
                canvas.Translate(650, 900);
                canvas.StrokeColor = StandardColors.Black;
                canvas.DrawPath(path);

                canvas.RestoreState();
            }

            canvas.RestoreState();
        }
    }
}