using Elevenworks.Graphics;

namespace GraphicsTester.Scenarios
{
    public abstract class AbstractScenario : EWPicture, EWDrawable
    {
        private float x;
        private float y;
        private float width;
        private float height;
        private string hash;

        public float X
        {
            get => x;
            set => x = value;
        }

        public float Y
        {
            get => y;
            set => y = value;
        }

        public float Width
        {
            get => width;
            set => width = value;
        }

        public float Height
        {
            get => height;
            set => height = value;
        }

        public AbstractScenario(float x, float y, float width, float height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public AbstractScenario(float width, float height)
        {
            this.width = width;
            this.height = height;
        }

        public virtual void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            // Do nothing by default
        }

        public void Draw(EWCanvas canvas, EWRectangle dirtyRect)
        {
            Draw(canvas, 1f, 1f);
        }

        public string Hash
        {
            get => hash;
            set => hash = value;
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}