namespace System.Graphics.CoreGraphics
{
    public class CGCanvasState : CanvasState
    {
        private bool _shadowed;

        public CGCanvasState() : base()
        {
        }

        public CGCanvasState(CGCanvasState prototype) : base(prototype)
        {
            _shadowed = prototype._shadowed;
        }

        public bool Shadowed
        {
            get => _shadowed;
            set => _shadowed = value;
        }
    }
}