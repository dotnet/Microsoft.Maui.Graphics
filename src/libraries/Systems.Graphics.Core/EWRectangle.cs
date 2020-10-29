using System;

namespace Xamarin.Graphics
{
    public class EWRectangle
    {
        internal const int ToTheLeftOf = 1;
        internal const int Above = 2;
        internal const int ToTheRightOf = 4;
        internal const int Beneath = 8;

        private readonly EWPoint _point;
        private readonly EWSize _size;

        public EWRectangle()
        {
            _point = new EWPoint(0, 0);
            _size = new EWSize(0, 0);
        }

        public EWRectangle(EWImmutablePoint location, EWSize size)
        {
            _point = new EWPoint(location);
            _size = size;
        }

        public EWRectangle(EWImmutablePoint location, float width, float height)
        {
            _point = new EWPoint(location);
            _size = new EWSize(width, height);
        }

        public EWRectangle(float x, float y, float width, float height) : this(new EWPoint(x, y), new EWSize(width, height))
        {
        }

        public EWRectangle(EWRectangle rectangle)
        {
            _point = new EWPoint(rectangle.Point1);
            _size = new EWSize(rectangle.Size);
        }

        public EWRectangle(EWSize size)
        {
            _point = new EWPoint();
            _size = new EWSize(size);
        }

        public EWRectangle(EWImmutablePoint point1, EWImmutablePoint point2)
        {
            _point = new EWPoint(Math.Min(point1.X, point2.X), Math.Min(point1.Y, point2.Y));
            _size = new EWSize(Math.Max(point1.X, point2.X) - _point.X, Math.Max(point1.Y, point2.Y) - _point.Y);
        }

        // ------------------------------------------------------------------------------------------------------------------------
        // Properties
        // ------------------------------------------------------------------------------------------------------------------------
        public EWPoint Point1
        {
            get => _point;
            set
            {
                _point.X = value.X;
                _point.Y = value.Y;
            }
        }

        public EWPoint Point2
        {
            get => new EWPoint(_point.X + _size.Width, _point.Y + _size.Height);
            set
            {
                _size.Width = value.X - _point.X;
                _size.Height = value.Y - _point.Y;
            }
        }

        public EWSize Size
        {
            get => _size;
            set => _size.SetSize(value);
        }

        public float X1
        {
            get => _point.X;
            set => _point.X = value;
        }

        public float Y1
        {
            get => _point.Y;
            set => _point.Y = value;
        }

        public float X2
        {
            get => _point.X + _size.Width;
            set => _size.Width = value - _point.X;
        }

        public float Y2
        {
            get => _point.Y + _size.Height;
            set => _size.Height = value - _point.Y;
        }

        public float Width
        {
            get => _size.Width;
            set => _size.Width = value;
        }

        public float Height
        {
            get => _size.Height;
            set => _size.Height = value;
        }

        public float MinX => Math.Min(X1, X2);

        public float MaxX => Math.Max(X1, X2);

        public float MinY => Math.Min(Y1, Y2);

        public float MaxY => Math.Max(Y1, Y2);

        public float MidX => X1 + Width / 2;

        public float MidY => Y1 + Height / 2;

        public EWRectangle Bounds => this;
        
        public void SetPoint2KeepingWidthHeightIntact(EWPoint aPoint)
        {
            _point.X = aPoint.X - _size.Width;
            _point.Y = aPoint.Y - _size.Height;
        }

        public float GetXAtRatio(float fx)
        {
            var x = Point1.X + (fx * Width);
            return x;
        }
        
        public float GetYAtRatio(float fx)
        {
            var y = Point1.Y + (fx * Height);
            return y;
        }
        
        public EWPoint GetPointAtRatio(float fx, float fy)
        {
            var vPoint = new EWPoint(Point1);
            vPoint.X += fx * Width;
            vPoint.Y += fy * Height;
            return vPoint;
        }

        public EWSize GetRatioAtPoint(EWImmutablePoint aPoint)
        {
            var vRatio = new EWSize
            {
                Width = Geometry.GetFactor(X1, X2, aPoint.X),
                Height = Geometry.GetFactor(Y1, Y2, aPoint.Y)
            };
            return vRatio;
        }

        public EWRectangle Inset(float inset)
        {
            var vRectangle = new EWRectangle(this);
            vRectangle.X1 += inset;
            vRectangle.Y1 += inset;
            vRectangle.Width -= (inset * 2);
            vRectangle.Height -= (inset * 2);
            return vRectangle;
        }

        public EWRectangle Grow(float inset)
        {
            var vRectangle = new EWRectangle(this);
            vRectangle.X1 -= inset;
            vRectangle.Y1 -= inset;
            vRectangle.Width += (inset * 2);
            vRectangle.Height += (inset * 2);
            return vRectangle;
        }

        public EWRectangle GrowCopy(float horizontal, float vertical)
        {
            var copy = new EWRectangle(this);
            copy.X1 -= horizontal;
            copy.Y1 -= vertical;
            copy.Width += (horizontal * 2);
            copy.Height += (vertical * 2);
            return copy;
        }

        public void SetRectangle(float aX, float aY, float aWidth, float aHeight)
        {
            _point.X = aX;
            _point.Y = aY;
            _size.Width = aWidth;
            _size.Height = aHeight;
        }

        public void SetRectangle(EWRectangle aRectangle)
        {
            SetRectangle(aRectangle.X1, aRectangle.Y1, aRectangle.Width, aRectangle.Height);
        }

        public EWRectangle GetIntersection(EWRectangle aRectangle)
        {
            var x1 = Math.Max(MinX, aRectangle.MinX);
            var y1 = Math.Max(MinY, aRectangle.MinY);
            var x2 = Math.Min(MaxX, aRectangle.MaxX);
            var y2 = Math.Min(MaxY, aRectangle.MaxY);

            return new EWRectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public EWRectangle GetUnion(EWRectangle aRectangle)
        {
            var x1 = Math.Min(MinX, aRectangle.MinX);
            var y1 = Math.Min(MinY, aRectangle.MinY);
            var x2 = Math.Max(MaxX, aRectangle.MaxX);
            var y2 = Math.Max(MaxY, aRectangle.MaxY);

            return new EWRectangle(x1, y1, x2 - x1, y2 - y1);
        }
        
        public bool Contains(EWImmutablePoint aPoint)
        {
            return Contains(aPoint.X, aPoint.Y);
        }

        public bool Contains(float x, float y)
        {
            // If the rectangle has a width of 0 or a height of 0
            if ((Math.Abs(Width - 0) < Geometry.Epsilon || Math.Abs(Height - 0) < Geometry.Epsilon))
            {
                return (Math.Abs(x - Point1.X) < Geometry.Epsilon) && (Math.Abs(y - Point1.Y) < Geometry.Epsilon);
            }

            // Next, check if it's above or left of the rectangle.
            if (x < MinX || y < MinY)
            {
                return false;
            }

            // Next, check if it's to the right or below
            if (x > MaxX || y > MaxY)
            {
                return false;
            }

            return true;
        }

        public static bool ContainsPoint(float x1, float y1, float x2, float y2, float px, float py)
        {
            var w = Math.Abs(x2 - x1);
            var h = Math.Abs(y2 - y1);

            // If the rectangle has a width of 0 or a height of 0
            if ((w < Geometry.Epsilon || h < Geometry.Epsilon))
            {
                return (Math.Abs(px - x1) < Geometry.Epsilon) && (Math.Abs(py - y1) < Geometry.Epsilon);
            }

            var minX = Math.Min(x1, x2);
            var minY = Math.Min(y1, y2);

            // Next, check if it's above or left of the rectangle.
            if (px < minX || py < minY)
            {
                return false;
            }

            var maxX = Math.Max(x1, x2);
            var maxy = Math.Max(y1, y2);

            // Next, check if it's to the right or below
            if (px > maxX || py > maxy)
            {
                return false;
            }

            return true;
        }

        public bool Contains(EWRectangle aRectangle)
        {
            return MinX <= aRectangle.MinX && MinY <= aRectangle.MinY && MaxX >= aRectangle.MaxX && MaxY >= aRectangle.MaxY;
        }
        
        public EWPoint GetCenter()
        {
            return new EWPoint(X1 + Width / 2, Y1 + Height / 2);
        }

        public override bool Equals(object obj)
        {
            if (obj is EWRectangle compareTo)
            {
                return compareTo.Point1.Equals(Point1) && compareTo.Size.Equals(Size);
            }

            return false;
        }

        public override string ToString()
        {
            return $"[EWRectangle: X1={X1}, Y1={Y1}, Width={Width}, Height={Height}]";
        }

        public override int GetHashCode()
        {
            return Point1.GetHashCode() ^ Size.GetHashCode();
        }

        public EWPoint PointAtAngle(float degrees)
        {
            var radians = Geometry.DegreesToRadians(degrees);

            var si = (float) Math.Sin(radians);
            var co = (float) Math.Cos(radians);
            const float e = 0.0001f;

            float x = 0, y = 0;
            if (Math.Abs(si) > e)
            {
                x = ((1.0f + co / (float) Math.Abs(si)) / 2.0f * Width);
                x = Range(0, Width, x);
            }
            else if (co >= 0.0)
            {
                x = Width;
            }

            if (Math.Abs(co) > e)
            {
                y = ((1.0f + si / (float) Math.Abs(co)) / 2.0f * Height);
                y = Range(0, Height, y);
            }
            else if (si >= 0.0)
            {
                y = Height;
            }

            return new EWPoint(X1 + x, Y1 + y);
        }

        /**
        * Constains a value to the given range.
        * @return the constrained value
        */

        private float Range(float min, float max, float value)
        {
            if (value < min)
            {
                value = min;
            }

            if (value > max)
            {
                value = max;
            }

            return value;
        }

        // ------------------------------------------------------------------------------------------------------------------------
        // Private, Internal and Protected Methods
        // ------------------------------------------------------------------------------------------------------------------------

        internal void Add(float aNewX, float aNewY)
        {
            if (Math.Abs(Width - 0) < Geometry.Epsilon || Math.Abs(Height - 0) < Geometry.Epsilon)
            {
                X1 = aNewX;
                Y1 = aNewY;
                Width = Height = 0;
                return;
            }

            var x1 = X1;
            var y1 = Y1;
            var x2 = Width;
            var y2 = Height;
            x2 += x1;
            y2 += y1;
            if (x1 > aNewX)
            {
                x1 = aNewX;
            }

            if (y1 > aNewY)
            {
                y1 = aNewY;
            }

            if (x2 < aNewX)
            {
                x2 = aNewX;
            }

            if (y2 < aNewY)
            {
                y2 = aNewY;
            }

            x2 -= x1;
            y2 -= y1;
            if (x2 > float.MaxValue)
            {
                x2 = float.MaxValue;
            }

            if (y2 > float.MaxValue)
            {
                y2 = float.MaxValue;
            }

            X1 = x1;
            Y1 = y1;
            Width = x2;
            Height = y2;
        }

        internal void Grow(float horizontalValue, float verticalValue)
        {
            var x0 = X1;
            var y0 = Y1;
            var x1 = Width;
            var y1 = Height;
            x1 += x0;
            y1 += y0;

            x0 -= horizontalValue;
            y0 -= verticalValue;
            x1 += horizontalValue;
            y1 += verticalValue;

            if (x1 < x0)
            {
                // Non-existant in X direction
                // Final width must remain negative so subtract x0 before
                // it is clipped so that we avoid the risk that the clipping
                // of x0 will reverse the ordering of x0 and x1.
                x1 -= x0;
                if (x1 < float.MinValue)
                {
                    x1 = float.MinValue;
                }

                if (x0 < float.MinValue)
                {
                    x0 = float.MinValue;
                }
                else if (x0 > float.MaxValue)
                {
                    x0 = float.MaxValue;
                }
            }
            else
            {
                // (x1 >= x0)
                // Clip x0 before we subtract it from x1 in case the clipping
                // affects the representable area of the rectangle.
                if (x0 < float.MinValue)
                {
                    x0 = float.MinValue;
                }
                else if (x0 > float.MaxValue)
                {
                    x0 = float.MaxValue;
                }

                x1 -= x0;
                // The only way x1 can be negative now is if we clipped
                // x0 against MIN and x1 is less than MIN - in which case
                // we want to leave the width negative since the result
                // did not intersect the representable area.
                if (x1 < float.MinValue)
                {
                    x1 = float.MinValue;
                }
                else if (x1 > float.MaxValue)
                {
                    x1 = float.MaxValue;
                }
            }

            if (y1 < y0)
            {
                // Non-existant in Y direction
                y1 -= y0;
                if (y1 < float.MinValue)
                {
                    y1 = float.MinValue;
                }

                if (y0 < float.MinValue)
                {
                    y0 = float.MinValue;
                }
                else if (y0 > float.MaxValue)
                {
                    y0 = float.MaxValue;
                }
            }
            else
            {
                // (y1 >= y0)
                if (y0 < float.MinValue)
                {
                    y0 = float.MinValue;
                }
                else if (y0 > float.MaxValue)
                {
                    y0 = float.MaxValue;
                }

                y1 -= y0;
                if (y1 < float.MinValue)
                {
                    y1 = float.MinValue;
                }
                else if (y1 > float.MaxValue)
                {
                    y1 = float.MaxValue;
                }
            }

            X1 = x0;
            Y1 = y0;
            Width = x1;
            Height = y1;
        }

        internal int GetRelativePosition(float x, float y)
        {
            var position = 0;
            if (Width <= 0)
            {
                position |= ToTheLeftOf | ToTheRightOf;
            }
            else if (x < X1)
            {
                position |= ToTheLeftOf;
            }
            else if (x > X1 + Width)
            {
                position |= ToTheRightOf;
            }

            if (Height <= 0)
            {
                position |= Above | Beneath;
            }
            else if (y < Y1)
            {
                position |= Above;
            }
            else if (y > Y1 + Height)
            {
                position |= Beneath;
            }

            return position;
        }

        public EWPath AsPath()
        {
            var path = new EWPath(new EWPoint(MinX, MinY));
            path.LineTo(new EWPoint(MaxX, MinY));
            path.LineTo(new EWPoint(MaxX, MaxY));
            path.LineTo(new EWPoint(MinX, MaxY));
            path.Close();
            return path;
        }

        public void MoveBy(float dx, float dy)
        {
            _point.X += dx;
            _point.Y += dy;
        }
    }
}