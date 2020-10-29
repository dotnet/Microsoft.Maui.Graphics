namespace Xamarin.Graphics
{
    public delegate void DrawingCommand(EWCanvas canvas, float zoom);

    public class StandardPicture : EWPicture
    {
        private readonly DrawingCommand[] _commands;

        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }
        public string Hash { get; set; }

        public StandardPicture(float x, float y, float width, float height, DrawingCommand[] commands, string hash = null)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            _commands = commands;

            Hash = hash;
        }

        public void Draw(EWCanvas canvas, float zoom, float ppu)
        {
            if (_commands != null)
                foreach (var command in _commands)
                    command.Invoke(canvas, zoom);
        }
    }
}