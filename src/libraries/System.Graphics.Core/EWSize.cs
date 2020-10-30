using System.Globalization;

namespace System.Graphics
{
    public class EWSize
    {
        private float _height;
        private float _width;

        public EWSize()
        {
            _width = 0;
            _height = 0;
        }

        public EWSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public EWSize(EWSize size)
        {
            if (size != null)
            {
                Width = size.Width;
                Height = size.Height;
            }
        }

        public float Width
        {
            get => _width;
            set
            {
                if (float.IsNaN(value))
                    _width = 0;
                else if (float.IsInfinity(value))
                    _width = 0;
                else
                    _width = value;
            }
        }

        public float Height
        {
            get => _height;
            set
            {
                if (float.IsNaN(value))
                    _height = 0;
                else if (float.IsInfinity(value))
                    _height = 0;
                else
                    _height = value;
            }
        }

        public void SetSize(float width, float height)
        {
            _width = width;
            _height = height;
        }

        public void SetSize(EWSize size)
        {
            SetSize(size.Width, size.Height);
        }

        public override bool Equals(object obj)
        {
            if (obj is EWSize compareTo)
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                return compareTo.Width == Width && compareTo.Height == Height;
                // ReSharper restore CompareOfFloatsByEqualityOperator
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (int) _width ^ (int) _height;
        }

        public string ToParsableString()
        {
            return _width.ToString(CultureInfo.InvariantCulture) + "," + _height.ToString(CultureInfo.InvariantCulture);
        }

        public override string ToString()
        {
            return $"[EWSize: Width={Width}, Height={Height}]";
        }

        public static EWSize Parse(string aValue)
        {
            try
            {
                string[] vValues = aValue.Split(new[] {','});
                float vWidth = float.Parse(vValues[0], CultureInfo.InvariantCulture);
                float vHeight = float.Parse(vValues[1], CultureInfo.InvariantCulture);
                return new EWSize(vWidth, vHeight);
            }
            catch (Exception exc)
            {
#if DEBUG
                Logger.Debug(exc);
#endif
                return new EWSize(0, 0);
            }
        }
    }
}