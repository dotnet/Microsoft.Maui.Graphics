using System;

namespace Xamarin.Graphics
{
    public class CanvasState : IDisposable
    {
        public float[] StrokeDashPattern { get; set; }
        public float StrokeSize { get; set; } = 1;
        public EWStrokeLocation StrokeLocation { get; set; } = EWStrokeLocation.CENTER;
        public float Scale { get; set; } = 1;
        public EWAffineTransform Transform { get; set; }

        public CanvasState()
        {
            Transform = new EWAffineTransform();
        }

        public CanvasState(CanvasState prototype)
        {
            StrokeDashPattern = prototype.StrokeDashPattern;
            StrokeSize = prototype.StrokeSize;
            StrokeLocation = prototype.StrokeLocation;
            Transform = new EWAffineTransform(prototype.Transform);
            Scale = prototype.Scale;
        }

        public virtual void Dispose()
        {
            // Do nothing right now
        }
    }
}