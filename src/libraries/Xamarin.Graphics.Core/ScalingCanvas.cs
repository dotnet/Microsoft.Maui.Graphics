using System;
using System.Collections.Generic;
using Elevenworks.Text;

namespace Xamarin.Graphics
{
    public class ScalingCanvas : EWCanvas, BlurrableCanvas
    {
        private readonly EWCanvas _canvas;
        private readonly BlurrableCanvas _blurrableCanvas;
        private readonly Stack<float> _scaleXStack = new Stack<float>();
        private readonly Stack<float> _scaleYStack = new Stack<float>();
        private float _scaleX = 1f;
        private float _scaleY = 1f;

        public ScalingCanvas(EWCanvas wrapped)
        {
            _canvas = wrapped;
            _blurrableCanvas = _canvas as BlurrableCanvas;
        }

        public override float DisplayScale => _canvas.DisplayScale;

        public override bool PixelShifted
        {
            get => _canvas.PixelShifted;
            set => _canvas.PixelShifted = value;
        }

        public object Wrapped => _canvas;

        public EWCanvas ParentCanvas => _canvas;

        public override float StrokeSize
        {
            set => _canvas.StrokeSize = value;
        }

        public override float MiterLimit
        {
            set => _canvas.MiterLimit = value;
        }

        public override EWColor StrokeColor
        {
            set => _canvas.StrokeColor = value;
        }

        public override EWLineCap StrokeLineCap
        {
            set => _canvas.StrokeLineCap = value;
        }
        
        public override float Alpha
        {
            set => _canvas.Alpha = value;
        }

        public override EWLineJoin StrokeLineJoin
        {
            set => _canvas.StrokeLineJoin = value;
        }

        public override float[] StrokeDashPattern
        {
            set => _canvas.StrokeDashPattern = value;
        }

        public override EWStrokeLocation StrokeLocation
        {
            set => _canvas.StrokeLocation = value;
        }

        public override bool LimitStrokeScaling
        {
            set => _canvas.LimitStrokeScaling = value;
        }

        public override float StrokeLimit
        {
            set => _canvas.StrokeLimit = value;
        }

        public override EWColor FillColor
        {
            set => _canvas.FillColor = value;
        }

        public override EWColor FontColor
        {
            set => _canvas.FontColor = value;
        }

        public override string FontName
        {
            set => _canvas.FontName = value;
        }

        public override float FontSize
        {
            set => _canvas.FontSize = value;
        }

        public override EWBlendMode BlendMode
        {
            set => _canvas.BlendMode = value;
        }

        public override bool Antialias
        {
            set => _canvas.Antialias = value;
        }

        public override void SubtractFromClip(float x1, float y1, float x2, float y2)
        {
            _canvas.SubtractFromClip(x1 * _scaleX, y1 * _scaleY, x2 * _scaleX, y2 * _scaleY);
        }
        
        public override void DrawLine(float x1, float y1, float x2, float y2)
        {
            _canvas.DrawLine(x1 * _scaleX, y1 * _scaleY, x2 * _scaleX, y2 * _scaleY);
        }

        public override void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            _canvas.DrawArc(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, startAngle, endAngle, clockwise, closed);
        }

        public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            _canvas.FillArc(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, startAngle, endAngle, clockwise);
        }

        public override void DrawOval(float x, float y, float width, float height)
        {
            _canvas.DrawOval(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public override void DrawImage(EWImage image, float x, float y, float width, float height)
        {
            _canvas.DrawImage(image, x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }
        
        public override void DrawRectangle(float x, float y, float width, float height)
        {
            _canvas.DrawRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public override void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            _canvas.DrawRoundedRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, cornerRadius * _scaleX);
        }

        public override void DrawString(string value, float x, float y, EwHorizontalAlignment horizontalAlignment)
        {
            _canvas.DrawString(value, x * _scaleX, y * _scaleY, horizontalAlignment);
        }

        public override void DrawString(string value, float x, float y, float width, float height, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0)
        {
            _canvas.DrawString(value, x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, horizontalAlignment, verticalAlignment, textFlow);
        }

        public override void DrawString(EWPath path, string value, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0)
        {
            _canvas.DrawString(path, value, horizontalAlignment, verticalAlignment, textFlow);
        }

        public override void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            _canvas.DrawText(value, x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public override void FillOval(float x, float y, float width, float height)
        {
            _canvas.FillOval(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public override void FillRectangle(float x, float y, float width, float height)
        {
            _canvas.FillRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            _canvas.FillRoundedRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, cornerRadius * _scaleX);
        }

        public override void DrawPath(EWPath path)
        {
            _canvas.DrawPath(path);
        }

        public override void FillPath(EWPath path, EWWindingMode windingMode)
        {
            _canvas.FillPath(path, windingMode);
        }

        public override void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            _canvas.ClipPath(path, windingMode);
        }

        public override void ClipRectangle(float x, float y, float width, float height)
        {
            _canvas.ClipRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public override void Rotate(float degrees, float x, float y)
        {
            _canvas.Rotate(degrees, x * _scaleX, y * _scaleY);
        }

        public override void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            _canvas.SetFillPaint(paint, x1 * _scaleX, y1 * _scaleY, x2 * _scaleX, y2 * _scaleY);
        }

        public override void Rotate(float degrees)
        {
            _canvas.Rotate(degrees);
        }

        public override void Scale(float sx, float sy)
        {
            _scaleX *= Math.Abs(sx);
            _scaleY *= Math.Abs(sy);
            _canvas.Scale(sx, sy);
        }

        public override void Translate(float tx, float ty)
        {
            _canvas.Translate(tx, ty);
        }

        public override void ConcatenateTransform(EWAffineTransform transform)
        {
            _scaleX *= transform.GetScaleX();
            _scaleY *= transform.GetScaleY();
            _canvas.ConcatenateTransform(transform);
        }

        public override void SaveState()
        {
            _canvas.SaveState();
            _scaleXStack.Push(_scaleX);
            _scaleYStack.Push(_scaleY);
        }

        public override void ResetState()
        {
            _canvas.ResetState();
            _scaleXStack.Clear();
            _scaleYStack.Clear();
            _scaleX = 1;
            _scaleY = 1;
        }

        public override bool RestoreState()
        {
            var restored = _canvas.RestoreState();
            if (_scaleXStack.Count > 0)
            {
                _scaleX = _scaleXStack.Pop();
                _scaleY = _scaleYStack.Pop();
            }
            else
            {
                _scaleX = 1;
                _scaleY = 1;
            }

            return restored;
        }

        public float GetScale()
        {
            return _scaleX;
        }

        public override void SetShadow(EWSize offset, float blur, EWColor color, float zoom)
        {
            _canvas.SetShadow(offset, blur, color, zoom);
        }

        public override void SetToSystemFont()
        {
            _canvas.SetToSystemFont();
        }

        public override void SetToBoldSystemFont()
        {
            _canvas.SetToBoldSystemFont();
        }

        public override object CurrentFigure => _canvas.CurrentFigure;

        public override void StartFigure(object figure)
        {
            _canvas.StartFigure(figure);
        }

        public override void EndFigure()
        {
            _canvas.EndFigure();
        }

        public void SetBlur(float blurRadius)
        {
            _blurrableCanvas?.SetBlur(blurRadius);
        }
    }
}