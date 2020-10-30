using System.Collections.Generic;
using System.Graphics.Text;

namespace System.Graphics
{
    public class PictureCanvas : EWCanvas, IDisposable
    {
        private readonly float _x;
        private readonly float _y;
        private readonly float _width;
        private readonly float _height;
        private readonly List<DrawingCommand> _commands;
        private readonly Stack<object> _figureStack = new Stack<object>();

        public PictureCanvas(float x, float y, float width, float height)
        {
            _x = x;
            _y = y;
            _height = height;
            _width = width;

            _commands = new List<DrawingCommand>();
        }

        public EWPicture Picture => new StandardPicture(_x, _y, _width, _height, _commands.ToArray());

        public override object CurrentFigure => _figureStack.Count > 0 ? _figureStack.Peek() : null;

        public override void StartFigure(object figure)
        {
            _figureStack.Push(figure);
        }

        public override void EndFigure()
        {
            _figureStack.Pop();
        }

        public void Dispose()
        {
            _commands.Clear();
        }

        public override float StrokeSize
        {
            set
            {
                _commands.Add(
                    (canvas, zoom) =>
                        canvas.StrokeSize = value
                );
            }
        }

        public override float MiterLimit
        {
            set { _commands.Add((canvas, zoom) => canvas.MiterLimit = value); }
        }

        public override EWColor StrokeColor
        {
            set { _commands.Add((canvas, zoom) => canvas.StrokeColor = value); }
        }

        public override EWLineCap StrokeLineCap
        {
            set { _commands.Add((canvas, zoom) => canvas.StrokeLineCap = value); }
        }

        public override EWLineJoin StrokeLineJoin
        {
            set { _commands.Add((canvas, zoom) => canvas.StrokeLineJoin = value); }
        }

        public override float[] StrokeDashPattern
        {
            set { _commands.Add((canvas, zoom) => canvas.StrokeDashPattern = value); }
        }

        public override EWStrokeLocation StrokeLocation
        {
            set { _commands.Add((canvas, zoom) => canvas.StrokeLocation = value); }
        }

        public override bool LimitStrokeScaling
        {
            set { _commands.Add((canvas, zoom) => canvas.LimitStrokeScaling = value); }
        }

        public override float StrokeLimit
        {
            set { _commands.Add((canvas, zoom) => canvas.StrokeLimit = value); }
        }
        
        public override EWColor FillColor
        {
            set { _commands.Add((canvas, zoom) => canvas.FillColor = value); }
        }

        public override EWColor FontColor
        {
            set { _commands.Add((canvas, zoom) => canvas.FontColor = value); }
        }

        public override string FontName
        {
            set { _commands.Add((canvas, zoom) => canvas.FontName = value); }
        }

        public override float FontSize
        {
            set { _commands.Add((canvas, zoom) => canvas.FontSize = value); }
        }

        public override float Alpha
        {
            set { _commands.Add((canvas, zoom) => canvas.Alpha = value); }
        }

        public override bool Antialias
        {
            set
            {
                // Do nothing, not currently supported in a picture.
            }
        }

        public override EWBlendMode BlendMode
        {
            set { _commands.Add((canvas, zoom) => canvas.BlendMode = value); }
        }

        public override void SubtractFromClip(float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.SubtractFromClip(x, y, width, height));
        }
        
        public override void DrawLine(float x1, float y1, float x2, float y2)
        {
            _commands.Add(
                (canvas, zoom)
                    => canvas.DrawLine(x1, y1, x2, y2)
            );
        }

        public override void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            _commands.Add((canvas, zoom) => canvas.DrawArc(x, y, width, height, startAngle, endAngle, clockwise, closed));
        }

        public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            _commands.Add((canvas, zoom) => canvas.FillArc(x, y, width, height, startAngle, endAngle, clockwise));
        }

        public override void DrawRectangle(float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.DrawRectangle(x, y, width, height));
        }

        public override void FillRectangle(float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.FillRectangle(x, y, width, height));
        }

        public override void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            _commands.Add((canvas, zoom) => canvas.DrawRoundedRectangle(x, y, width, height, cornerRadius));
        }

        public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            _commands.Add((canvas, zoom) => canvas.FillRoundedRectangle(x, y, width, height, cornerRadius));
        }

        public override void DrawOval(float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.DrawOval(x, y, width, height));
        }

        public override void FillOval(float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.FillOval(x, y, width, height));
        }

        public override void DrawString(string value, float x, float y, EwHorizontalAlignment horizontalAlignment)
        {
            _commands.Add((canvas, zoom) => canvas.DrawString(value, x, y, horizontalAlignment));
        }

        public override void DrawString(string value,float x, float y, float width, float height, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0)
        {
            _commands.Add((canvas, zoom) => canvas.DrawString(value, x, y, width, height, horizontalAlignment, verticalAlignment, textFlow, lineSpacingAdjustment));
        }

        public override void DrawString(EWPath path, string value, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0)
        {
            _commands.Add((canvas, zoomx) => canvas.DrawString(path, value, horizontalAlignment, verticalAlignment, textFlow, lineSpacingAdjustment));
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.DrawText(value, x, y, width, height));
        }

        public override void DrawPath(EWPath path)
        {
            _commands.Add((canvas, zoomx) => canvas.DrawPath(path));
        }

        public override void FillPath(EWPath path, EWWindingMode windingMode)
        {
            _commands.Add((canvas, zoomx) => canvas.FillPath(path, windingMode));
        }

        public override void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            _commands.Add((canvas, zoomx) => canvas.ClipPath(path, windingMode));
        }

        public override void ClipRectangle(
            float x,
            float y,
            float width,
            float height)
        {
            _commands.Add((canvas, zoomx) => canvas.ClipRectangle(x, y, width, height));
        }

        public override void Rotate(float degrees, float x, float y)
        {
            _commands.Add((canvas, zoom) => canvas.Rotate(degrees, x, y));
        }

        public override void Rotate(float degrees)
        {
            _commands.Add((canvas, zoom) => canvas.Rotate(degrees));
        }

        public override void Scale(float sx, float sy)
        {
            _commands.Add((canvas, zoom) => canvas.Scale(sx, sy));
        }

        public override void Translate(float tx, float ty)
        {
            _commands.Add((canvas, zoom) => canvas.Translate(tx, ty));
        }

        public override void ConcatenateTransform(EWAffineTransform transform)
        {
            _commands.Add((canvas, zoom) => canvas.ConcatenateTransform(transform));
        }

        public override void SaveState()
        {
            _commands.Add((canvas, zoom) => canvas.SaveState());
        }

        public override bool RestoreState()
        {
            _commands.Add((canvas, zoom) => canvas.RestoreState());
            return true;
        }

        public override void SetShadow(EWSize offset, float blur, EWColor color, float zoom)
        {
            _commands.Add((canvas, zoomx) => canvas.SetShadow(offset, blur, color, zoomx));
        }

        public override void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            _commands.Add((canvas, zoom) => canvas.SetFillPaint(paint, x1, y1, x2, y2));
        }

        public override void SetToSystemFont()
        {
            _commands.Add((canvas, zoom) => canvas.SetToSystemFont());
        }

        public override void SetToBoldSystemFont()
        {
            _commands.Add((canvas, zoom) => canvas.SetToBoldSystemFont());
        }

        public override void DrawImage(EWImage image, float x, float y, float width, float height)
        {
            _commands.Add((canvas, zoom) => canvas.DrawImage(image, x, y, width, height));
        }
    }
}