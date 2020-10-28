using System;

namespace Xamarin.Graphics
{
    public class EWPaintStop : IComparable<EWPaintStop>
    {
        private EWColor _color;
        private float _offset;

        public EWPaintStop(float offset, EWColor color)
        {
            _color = color;
            _offset = offset;
        }

        public EWPaintStop(EWPaintStop source)
        {
            _color = source._color;
            _offset = source._offset;
        }

        public EWColor Color
        {
            get => _color;
            set => _color = value;
        }

        public float Offset
        {
            get => _offset;
            set => _offset = value;
        }

        public int CompareTo(EWPaintStop obj)
        {
            if (_offset < obj._offset)
                return -1;
            if (_offset > obj._offset)
                return 1;

            return 0;
        }
    }
}