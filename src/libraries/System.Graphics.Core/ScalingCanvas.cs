using System.Collections.Generic;
using System.Graphics.Text;

namespace System.Graphics
{
    public class ScalingCanvas : ICanvas, IBlurrableCanvas
    {
        private readonly ICanvas _canvas;
        private readonly IBlurrableCanvas _blurrableCanvas;
        private readonly Stack<float> _scaleXStack = new Stack<float>();
        private readonly Stack<float> _scaleYStack = new Stack<float>();
        private float _scaleX = 1f;
        private float _scaleY = 1f;

        public ScalingCanvas(ICanvas wrapped)
        {
            _canvas = wrapped;
            _blurrableCanvas = _canvas as IBlurrableCanvas;
        }

        public float RetinaScale
        {
            get => _canvas.RetinaScale;
            set => _canvas.RetinaScale = value;
        }
        
        public float DisplayScale
        {
            get => _canvas.DisplayScale;
            set => _canvas.DisplayScale = value;
        }

        public object Wrapped => _canvas;

        public ICanvas ParentCanvas => _canvas;

        public float StrokeSize
        {
            set => _canvas.StrokeSize = value;
        }

        public float MiterLimit
        {
            set => _canvas.MiterLimit = value;
        }

        public EWColor StrokeColor
        {
            set => _canvas.StrokeColor = value;
        }

        public EWLineCap StrokeLineCap
        {
            set => _canvas.StrokeLineCap = value;
        }
        
        public float Alpha
        {
            set => _canvas.Alpha = value;
        }

        public EWLineJoin StrokeLineJoin
        {
            set => _canvas.StrokeLineJoin = value;
        }

        public float[] StrokeDashPattern
        {
            set => _canvas.StrokeDashPattern = value;
        }

        public EWStrokeLocation StrokeLocation
        {
            set => _canvas.StrokeLocation = value;
        }

        public bool LimitStrokeScaling
        {
            set => _canvas.LimitStrokeScaling = value;
        }

        public float StrokeLimit
        {
            set => _canvas.StrokeLimit = value;
        }

        public EWColor FillColor
        {
            set => _canvas.FillColor = value;
        }

        public EWColor FontColor
        {
            set => _canvas.FontColor = value;
        }

        public string FontName
        {
            set => _canvas.FontName = value;
        }

        public float FontSize
        {
            set => _canvas.FontSize = value;
        }

        public BlendMode BlendMode
        {
            set => _canvas.BlendMode = value;
        }

        public bool Antialias
        {
            set => _canvas.Antialias = value;
        }

        public void SubtractFromClip(float x1, float y1, float x2, float y2)
        {
            _canvas.SubtractFromClip(x1 * _scaleX, y1 * _scaleY, x2 * _scaleX, y2 * _scaleY);
        }
        
        public void DrawLine(float x1, float y1, float x2, float y2)
        {
            _canvas.DrawLine(x1 * _scaleX, y1 * _scaleY, x2 * _scaleX, y2 * _scaleY);
        }

        public void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            _canvas.DrawArc(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, startAngle, endAngle, clockwise, closed);
        }

        public void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
        {
            _canvas.FillArc(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, startAngle, endAngle, clockwise);
        }

        public void DrawOval(float x, float y, float width, float height)
        {
            _canvas.DrawOval(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public void DrawImage(EWImage image, float x, float y, float width, float height)
        {
            _canvas.DrawImage(image, x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }
        
        public void DrawRectangle(float x, float y, float width, float height)
        {
            _canvas.DrawRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            _canvas.DrawRoundedRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, cornerRadius * _scaleX);
        }

        public void DrawString(string value, float x, float y, EwHorizontalAlignment horizontalAlignment)
        {
            _canvas.DrawString(value, x * _scaleX, y * _scaleY, horizontalAlignment);
        }

        public void DrawString(string value, float x, float y, float width, float height, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0)
        {
            _canvas.DrawString(value, x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, horizontalAlignment, verticalAlignment, textFlow);
        }
        
        public void DrawText(IAttributedText value, float x, float y, float width, float height)
        {
            _canvas.DrawText(value, x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public void FillOval(float x, float y, float width, float height)
        {
            _canvas.FillOval(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public void FillRectangle(float x, float y, float width, float height)
        {
            _canvas.FillRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            _canvas.FillRoundedRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY, cornerRadius * _scaleX);
        }

        public void DrawPath(EWPath path)
        {
            _canvas.DrawPath(path);
        }

        public void FillPath(EWPath path, EWWindingMode windingMode)
        {
            _canvas.FillPath(path, windingMode);
        }

        public void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero)
        {
            _canvas.ClipPath(path, windingMode);
        }

        public void ClipRectangle(float x, float y, float width, float height)
        {
            _canvas.ClipRectangle(x * _scaleX, y * _scaleY, width * _scaleX, height * _scaleY);
        }

        public void Rotate(float degrees, float x, float y)
        {
            _canvas.Rotate(degrees, x * _scaleX, y * _scaleY);
        }

        public void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2)
        {
            _canvas.SetFillPaint(paint, x1 * _scaleX, y1 * _scaleY, x2 * _scaleX, y2 * _scaleY);
        }

        public void Rotate(float degrees)
        {
            _canvas.Rotate(degrees);
        }

        public void Scale(float sx, float sy)
        {
            _scaleX *= Math.Abs(sx);
            _scaleY *= Math.Abs(sy);
            _canvas.Scale(sx, sy);
        }

        public void Translate(float tx, float ty)
        {
            _canvas.Translate(tx, ty);
        }

        public void ConcatenateTransform(AffineTransform transform)
        {
            _scaleX *= transform.ScaleX;
            _scaleY *= transform.ScaleY;
            _canvas.ConcatenateTransform(transform);
        }

        public void SaveState()
        {
            _canvas.SaveState();
            _scaleXStack.Push(_scaleX);
            _scaleYStack.Push(_scaleY);
        }

        public void ResetState()
        {
            _canvas.ResetState();
            _scaleXStack.Clear();
            _scaleYStack.Clear();
            _scaleX = 1;
            _scaleY = 1;
        }

        public bool RestoreState()
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

        public void SetShadow(EWSize offset, float blur, EWColor color)
        {
            _canvas.SetShadow(offset, blur, color);
        }

        public void SetToSystemFont()
        {
            _canvas.SetToSystemFont();
        }

        public void SetToBoldSystemFont()
        {
            _canvas.SetToBoldSystemFont();
        }

        public void SetBlur(float blurRadius)
        {
            _blurrableCanvas?.SetBlur(blurRadius);
        }
    }
}