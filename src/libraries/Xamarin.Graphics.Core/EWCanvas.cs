using System;
using Xamarin.Text;

namespace Xamarin.Graphics
{
    public abstract class EWCanvas
    {
        public static readonly EWColor DefaultShadowColor = new EWColor(0f, 0f, 0f, .5f);
        public static readonly EWSize DefaultShadowOffset = new EWSize(5, 5);
        public const float DefaultShadowBlur = 5;
        public const float DefaultMiterLimit = 10;

        public virtual float DisplayScale { get; set; } = 1;
        public virtual float RetinaScale { get; set; } = 1;

        public abstract float StrokeSize { set; }
        public abstract float MiterLimit { set; }
        public abstract EWColor StrokeColor { set; }
        public abstract EWLineCap StrokeLineCap { set; }
        public abstract EWLineJoin StrokeLineJoin { set; }
        public abstract EWStrokeLocation StrokeLocation { set; }
        public abstract float[] StrokeDashPattern { set; }

        public abstract bool LimitStrokeScaling { set; }
        public abstract float StrokeLimit { set; }

        public abstract EWColor FillColor { set; }
        public abstract EWColor FontColor { set; }
        public abstract string FontName { set; }
        public abstract float FontSize { set; }
        public abstract float Alpha { set; }
        public abstract bool Antialias { set; }
        public abstract EWBlendMode BlendMode { set; }
        public abstract bool PixelShifted { get; set; }

        public void PixelShift(float tx, float ty)
        {
            Translate(tx, ty);
            PixelShifted = tx > 0;
        }

        public abstract object CurrentFigure { get; }
        public abstract void StartFigure(object figure);
        public abstract void EndFigure();

        public abstract void DrawPath(EWPath path);

        public abstract void FillPath(EWPath path, EWWindingMode windingMode);

        public void SubtractFromClip(EWRectangle rect)
        {
            SubtractFromClip(rect.MinX, rect.MinY, Math.Abs(rect.Width), Math.Abs(rect.Height));
        }

        public abstract void SubtractFromClip(float x, float y, float width, float height);
        
        public abstract void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero);

        public abstract void ClipRectangle(float x, float y, float width, float height);

        public abstract void DrawLine(float x1, float y1, float x2, float y2);

        public abstract void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed);

        public abstract void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise);

        public abstract void DrawRectangle(float x, float y, float width, float height);

        public abstract void FillRectangle(float x, float y, float width, float height);

        public abstract void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius);

        public abstract void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius);

        public abstract void DrawOval(float x, float y, float width, float height);

        public abstract void FillOval(float x, float y, float width, float height);

        public abstract void DrawString(string value, float x, float y, EwHorizontalAlignment horizontalAlignment);

        public abstract void DrawString(string value, float x, float y, float width, float height, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0);

        public virtual void DrawString(EWPath path, string value, EwHorizontalAlignment horizontalAlignment, EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS, float lineSpacingAdjustment = 0)
        {
            // Subclass must implement this
        }

        public abstract void DrawText(IAttributedText value, float x, float y, float width, float height);

        public abstract void Rotate(float degrees, float x, float y);

        public abstract void Rotate(float degrees);

        public abstract void Scale(float sx, float sy);

        public abstract void Translate(float tx, float ty);

        public abstract void ConcatenateTransform(EWAffineTransform transform);

        public abstract void SaveState();

        public abstract bool RestoreState();

        public virtual void ResetState()
        {
            // Do nothing by default.
        }

        public abstract void SetShadow(EWSize offset, float blur, EWColor color, float zoom);

        public void SetFillPaint(EWPaint paint, EWImmutablePoint point1, EWImmutablePoint point2)
        {
            SetFillPaint(paint, point1.X, point1.Y, point2.X, point2.Y);
        }
        
        public void SetFillPaint(EWPaint paint, EWRectangle rectangle)
        {
            SetFillPaint(paint, rectangle.X1, rectangle.Y1, rectangle.X2, rectangle.Y2);
        }

        public abstract void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2);

        public abstract void SetToSystemFont();

        public abstract void SetToBoldSystemFont();

        public abstract void DrawImage(EWImage image, float x, float y, float width, float height);
    }
}