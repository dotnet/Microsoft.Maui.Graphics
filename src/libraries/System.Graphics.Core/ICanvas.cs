using System.Graphics.Text;

namespace System.Graphics
{
    public interface ICanvas
    {
        public float DisplayScale { get; set; }
        public float RetinaScale { get; set; }

        public  float StrokeSize { set; }
        public  float MiterLimit { set; }
        public  EWColor StrokeColor { set; }
        public  EWLineCap StrokeLineCap { set; }
        public  EWLineJoin StrokeLineJoin { set; }
        public  EWStrokeLocation StrokeLocation { set; }
        public  float[] StrokeDashPattern { set; }

        public  bool LimitStrokeScaling { set; }
        public  float StrokeLimit { set; }

        public  EWColor FillColor { set; }
        public  EWColor FontColor { set; }
        public  string FontName { set; }
        public  float FontSize { set; }
        public  float Alpha { set; }
        public  bool Antialias { set; }
        public  BlendMode BlendMode { set; }

        public  void DrawPath(EWPath path);

        public  void FillPath(EWPath path, EWWindingMode windingMode);
        public  void SubtractFromClip(float x, float y, float width, float height);
        
        public  void ClipPath(EWPath path, EWWindingMode windingMode = EWWindingMode.NonZero);

        public  void ClipRectangle(float x, float y, float width, float height);

        public  void DrawLine(float x1, float y1, float x2, float y2);

        public  void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed);

        public  void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise);

        public  void DrawRectangle(float x, float y, float width, float height);

        public  void FillRectangle(float x, float y, float width, float height);

        public  void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius);

        public  void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius);

        public  void DrawOval(float x, float y, float width, float height);

        public  void FillOval(float x, float y, float width, float height);

        public  void DrawString(string value, float x, float y, EwHorizontalAlignment horizontalAlignment);

        public void DrawString(
            string value,
            float x,
            float y,
            float width,
            float height,
            EwHorizontalAlignment horizontalAlignment,
            EwVerticalAlignment verticalAlignment,
            EWTextFlow textFlow = EWTextFlow.CLIP_BOUNDS,
            float lineSpacingAdjustment = 0);

        public  void DrawText(
            IAttributedText value, 
            float x, 
            float y, 
            float width,
            float height);

        public  void Rotate(float degrees, float x, float y);

        public  void Rotate(float degrees);

        public  void Scale(float sx, float sy);

        public  void Translate(float tx, float ty);

        public  void ConcatenateTransform(AffineTransform transform);

        public  void SaveState();

        public  bool RestoreState();

        public void ResetState();

        public  void SetShadow(EWSize offset, float blur, EWColor color);

        public  void SetFillPaint(EWPaint paint, float x1, float y1, float x2, float y2);

        public  void SetToSystemFont();

        public  void SetToBoldSystemFont();

        public  void DrawImage(EWImage image, float x, float y, float width, float height);
    }
}