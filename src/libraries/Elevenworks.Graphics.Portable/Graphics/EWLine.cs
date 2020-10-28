using System;

namespace Elevenworks.Graphics
{
    public class EWLine
    {
        private readonly EWPoint _point1;
        private readonly EWPoint _point2;

        public EWLine()
        {
            _point1 = new EWPoint(0, 0);
            _point2 = new EWPoint(0, 0);
        }

        public EWLine(EWLine line) : this(new EWPoint(line.Point1), new EWPoint(line.Point2))
        {
        }

        public EWLine(EWImmutablePoint point1, EWImmutablePoint point2)
        {
            _point1 = new EWPoint(point1);
            _point2 = new EWPoint(point2);
        }

        public EWLine(float x1, float y1, float x2, float y2)
        {
            _point1 = new EWPoint(x1, y1);
            _point2 = new EWPoint(x2, y2);
        }

        public EWImmutablePoint Point1
        {
            get => _point1;
            set
            {
                _point1.X = value.X;
                _point1.Y = value.Y;
            }
        }

        public EWImmutablePoint Point2
        {
            get => _point2;
            set
            {
                _point2.X = value.X;
                _point2.Y = value.Y;
            }
        }

        public float X1
        {
            get => _point1.X;
            set => _point1.X = value;
        }

        public float Y1
        {
            get => _point1.Y;
            set => _point1.Y = value;
        }

        public float X2
        {
            get => _point2.X;
            set => _point2.X = value;
        }

        public float Y2
        {
            get => _point2.Y;
            set => _point2.Y = value;
        }

        public float Length => Geometry.GetDistance(_point1, _point2);

        public EWPoint Center
        {
            get
            {
                float x = (X1 + X2) / 2.0f;
                float y = (Y1 + Y2) / 2.0f;
                return new EWPoint(x, y);
            }
        }

        public EWRectangle ToRectangle()
        {
            float minX = Math.Min(_point1.X, _point2.X);
            float minY = Math.Min(_point1.Y, _point2.Y);
            float w = Math.Abs(_point2.X - _point1.X);
            float h = Math.Abs(_point2.Y - _point1.Y);
            return new EWRectangle(minX, minY, w, h);
        }

        public bool Contains(EWImmutablePoint point)
        {
            return Contains(point, 1);
        }

        public bool Contains(EWImmutablePoint point, float slack)
        {
            if (Math.Abs(DistanceFromLine(point)) <= slack)
            {
                float length = Length + slack;
                float d1 = Geometry.GetDistance(_point1, point);
                float d2 = Geometry.GetDistance(_point2, point);
                if (d1 < length && d2 < length)
                {
                    return true;
                }
            }

            return false;
        }

        public float DistanceFromLine(EWImmutablePoint point)
        {
            float x1 = Point1.X;
            float x2 = Point2.X;
            float px = point.X;

            float y2 = Point2.Y;
            float y1 = Point1.Y;
            float py = point.Y;

            if (x1 == x2 && y1 == y2)
            {
                return Geometry.GetDistance(Point1, point);
            }

            if (Math.Abs(x1 - x2) < Geometry.Epsilon && Math.Abs(x1 - px) < Geometry.Epsilon)
            {
                if (y2 > y1)
                {
                    if (py >= y1 && py <= y2)
                    {
                        return 0;
                    }

                    if (py < y1)
                    {
                        return y1 - py;
                    }

                    return py - y2;
                }

                if (py >= y2 && py <= y1)
                {
                    return 0;
                }

                if (py < y2)
                {
                    return y2 - py;
                }

                return py - y1;
            }

            if (Math.Abs(y1 - y2) < Geometry.Epsilon && Math.Abs(y1 - py) < Geometry.Epsilon)
            {
                if (x2 > x1)
                {
                    if (px >= x1 && px <= x2)
                    {
                        return 0;
                    }

                    if (px < x1)
                    {
                        return x1 - px;
                    }

                    return px - x2;
                }

                if (px >= x2 && px <= x1)
                {
                    return 0;
                }

                if (px < x2)
                {
                    return x2 - px;
                }

                return px - x1;
            }

            x2 -= x1;
            y2 -= y1;
            px -= x1;
            py -= y1;

            float dotprod = px * x2 + py * y2;
            float projlenSq = dotprod * dotprod / (x2 * x2 + y2 * y2);
            float lenSq = px * px + py * py - projlenSq;

            if (lenSq < 0)
            {
                lenSq = 0;
            }

            return (float) Math.Sqrt(lenSq);
        }
    }
}