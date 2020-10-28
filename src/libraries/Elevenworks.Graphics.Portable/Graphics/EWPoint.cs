using System;
using System.Globalization;
using System.IO;

namespace Elevenworks.Graphics
{
    public class EWPoint : EWImmutablePoint
    {
        private float _x;
        private float _y;

        public EWPoint()
        {
            _x = 0;
            _y = 0;
        }

        public EWPoint(float x, float y)
        {
            X = x;
            Y = y;
        }

        public EWPoint(EWImmutablePoint point)
        {
            X = point?.X ?? 0;
            Y = point?.Y ?? 0;
        }

        public float X
        {
            get => _x;
            set
            {
                if (float.IsNaN(value))
                    _x = 0;
                else if (float.IsInfinity(value))
                    _x = 0;
                else
                    _x = value;
            }
        }

        public float Y
        {
            get => _y;
            set
            {
                if (float.IsNaN(value))
                    _y = 0;
                else if (float.IsInfinity(value))
                    _y = 0;
                else
                    _y = value;
            }
        }

        public override string ToString()
        {
            return $"[EWPoint: X={_x}, Y={_y}]";
        }

        public override bool Equals(object obj)
        {
            if (obj is EWImmutablePoint compareTo)
            {
                // ReSharper disable CompareOfFloatsByEqualityOperator
                return compareTo.X == _x && compareTo.Y == _y;
                // ReSharper restore CompareOfFloatsByEqualityOperator
            }

            return false;
        }
        
        public bool Equals(object obj, float epsilon)
        {
            if (obj is EWImmutablePoint compareTo)
            {
                return Math.Abs(compareTo.X - _x) < epsilon && Math.Abs(compareTo.Y - _y) < epsilon;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return (int) _x ^ (int) _y;
        }

        public float[] ToArray()
        {
            return new[] {_x, _y};
        }

        public void SetLocation(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetLocation(EWPoint point)
        {
            X = point.X;
            Y = point.Y;
        }

        public void Move(float dx, float dy)
        {
            X += dx;
            Y += dy;
        }

        public float Distance(EWPoint to)
        {
            return Geometry.GetDistance(this, to);
        }

        public string ToParsableString()
        {
            var writer = new StringWriter();
            writer.Write(X.ToString(CultureInfo.InvariantCulture));
            writer.Write(",");
            writer.Write(Y.ToString(CultureInfo.InvariantCulture));
            return writer.ToString();
        }

        public static EWPoint Parse(string aValue)
        {
            var point = new EWPoint();
            try
            {
                var values = aValue.Split(',');

                if (values.Length > 0)
                    point.X = float.Parse(values[0], CultureInfo.InvariantCulture);

                if (values.Length > 1)
                    point.Y = float.Parse(values[1], CultureInfo.InvariantCulture);
            }
            catch (Exception exc)
            {
#if DEBUG
                Logger.Debug(exc);
#endif
            }

            return point;
        }

        public EWPoint PointAtAngle(float radius, float degrees)
        {
            var radians = Geometry.DegreesToRadians(degrees);
            var px = radius * (float) Math.Cos(radians) + _x;
            var py = -1 * radius * (float) Math.Sin(radians) + _y;
            return new EWPoint(px, py);
        }

        public void RoundToNearest(float d)
        {
            _x = (float)Math.Round(_x / d) * d;
            _y = (float)Math.Round(_y / d) * d;
        }
    }
}