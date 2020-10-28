using System;

namespace Elevenworks.Graphics
{
    public static class BezierUtil
    {
        private const int MaxDepth = 64; // maximum recursion depth
        internal static readonly float Epsilon = 1.0f * (float) Math.Pow(2, -MaxDepth - 1); // flatness tolerance

        // pre-computed z(i,j)
        private static readonly float[] ZCubic = {1.0f, 0.6f, 0.3f, 0.1f, 0.4f, 0.6f, 0.6f, 0.4f, 0.1f, 0.3f, 0.6f, 1.0f};
        private static readonly float[] ZQuad = {1.0f, 2f / 3f, 1f / 3f, 1f / 3f, 2f / 3f, 1.0f};

        /**
       * Returns a point on a quadratic Bézier curve.
       * 
       * <p>Adapted from Robert Penner with Robert Penner's optimization of the standard equation.</p>
       * 
       * @param p 	A fraction between [0-1] of the whole curve.
       * @param p1 	First point.
       * @param p2 	Second point.
       * @param p3 	Third point.
       *
       * @return 	The resulting position Point3D.
       */

        public static EWPoint GetPointOnQuadCurveAt(float t, EWImmutablePoint p1, EWImmutablePoint p2, EWImmutablePoint p3)
        {
            float ip2 = 2 * (1 - t);
            return new EWPoint(p1.X + t * (ip2 * (p2.X - p1.X) + t * (p3.X - p1.X)), p1.Y + t * (ip2 * (p2.Y - p1.Y) + t * (p3.Y - p1.Y)));
        }

        /**
       * Returns a point on a qubic Bézier curve.
       * 
       * <p>Adapted from Paul Bourke.</p>
       * 
       * @param p 	A fraction between [0-1] of the whole curve.
       * @param p1 	First point.
       * @param p2 	Second point.
       * @param p3 	Third point.
       * @param p4 	Fourth point.
       *
       * @return 	The resulting position Point3D.
       */

        public static EWPoint GetPointOnCubicCurveAt(float t, EWImmutablePoint p1, EWImmutablePoint p2, EWImmutablePoint p3, EWImmutablePoint p4)
        {
            float d = t * t;
            float a = 1 - t;
            float e = a * a;
            float b = e * a;
            float c = d * t;
            return new EWPoint(b * p1.X + 3 * t * e * p2.X + 3 * d * a * p3.X + c * p4.X, b * p1.Y + 3 * t * e * p2.Y + 3 * d * a * p3.Y + c * p4.Y);
        }

        private static EWPoint PointAt(float t, EWImmutablePoint[] curve)
        {
            if (curve.Length == 3)
            {
                return GetPointOnQuadCurveAt(t, curve[0], curve[1], curve[2]);
            }

            return GetPointOnCubicCurveAt(t, curve[0], curve[1], curve[2], curve[3]);
        }

        public static EWPoint[] InsertHandleIntoQuadCurve(EWImmutablePoint[] curve, float t)
        {
            var a = new EWPoint(curve[0]);
            var b = new EWPoint(curve[1]);
            var c = new EWPoint(curve[2]);

            EWPoint d = Geometry.GetPointAlongLine(a, b, t);
            EWPoint f = Geometry.GetPointAlongLine(b, c, t);
            EWPoint e = Geometry.GetPointAlongLine(d, f, t);
            //EWPoint e = Geometry.GetCenter(d, f);

            return new[] {a, d, e, e, f, c};
        }

        public static EWPoint[] InsertHandleIntoCubicCurve(EWImmutablePoint[] curve, float t)
        {
            float mt = 1 - t;

            var p0 = new EWPoint(curve[0]);
            var p1 = curve[1];
            var p2 = curve[2];
            var p3 = new EWPoint(curve[3]);

            EWPoint p01 = AddPoints(MultiplyPoint(p0, mt), MultiplyPoint(p1, t));
            EWPoint p12 = AddPoints(MultiplyPoint(p1, mt), MultiplyPoint(p2, t));
            EWPoint p23 = AddPoints(MultiplyPoint(p2, mt), MultiplyPoint(p3, t));

            EWPoint p0112 = AddPoints(MultiplyPoint(p01, mt), MultiplyPoint(p12, t));
            EWPoint p1223 = AddPoints(MultiplyPoint(p12, mt), MultiplyPoint(p23, t));

            EWPoint p01121223 = AddPoints(MultiplyPoint(p0112, mt), MultiplyPoint(p1223, t));

            return new EWPoint[] {p0, p01, p0112, p01121223, p01121223, p1223, p23, p3};
        }

        private static EWPoint MultiplyPoint(EWImmutablePoint aPoint, float aFactor)
        {
            return new EWPoint(aPoint.X * aFactor, aPoint.Y * aFactor);
        }

        private static EWPoint AddPoints(EWImmutablePoint aPoint1, EWImmutablePoint aPoint2)
        {
            return new EWPoint(aPoint1.X + aPoint2.X, aPoint1.Y + aPoint2.Y);
        }

        public static float ClosestPointToBezier(EWImmutablePoint[] aCurve, EWImmutablePoint aPoint)
        {
            if (aCurve == null || aPoint == null)
            {
                return 0;
            }

            if (aCurve.Length < 3 || aCurve.Length > 4)
            {
                return 0;
            }

            // record distances from point to endpoints
            var p = aCurve[0];
            float deltaX = p.X - aPoint.X;
            float deltaY = p.Y - aPoint.Y;
            float d0 = (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            p = aCurve[aCurve.Length - 1];
            deltaX = p.X - aPoint.X;
            deltaY = p.Y - aPoint.Y;
            float d1 = (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            int n = aCurve.Length - 1;

            // array of control points
            var v = new EWPoint[aCurve.Length];
            if (aCurve.Length == 3)
            {
                v[0] = new EWPoint(aCurve[0]);
                v[1] = new EWPoint(aCurve[1]);
                v[2] = new EWPoint(aCurve[2]);
            }
            else
            {
                v[0] = new EWPoint(aCurve[0]);
                v[1] = new EWPoint(aCurve[1]);
                v[2] = new EWPoint(aCurve[2]);
                v[3] = new EWPoint(aCurve[3]);
            }

            // instaead of power form, convert the function whose zeros are required to Bezier form
            EWPoint[] w = ToBezierForm(aPoint, v);

            // Find roots of the Bezier curve with control points stored in 'w' (algorithm is recursive, this is root depth of 0)
            float[] roots = FindRoots(w, 2 * n - 1, 0);

            // compare the candidate distances to the endpoints and declare a winner :)
            float dMinimum;
            float tMinimum = 0;
            if (d0 < d1)
            {
                dMinimum = d0;
            }
            else
            {
                tMinimum = 1;
                dMinimum = d1;
            }

            // tbd - compare 2-norm squared
            foreach (float t in roots)
            {
                if (t >= 0 && t <= 1)
                {
                    p = PointAt(t, aCurve);
                    deltaX = p.X - aPoint.X;
                    deltaY = p.Y - aPoint.Y;
                    float d = (float) Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

                    if (d < dMinimum)
                    {
                        tMinimum = t;
                        dMinimum = d;
                    }
                }
            }

            // tbd - alternate optima.
            return tMinimum;
        }

        // compute control points of the polynomial resulting from the inner product of B(t)-P and B'(t), constructing the result as a Bezier
        // curve of order 2n-1, where n is the degree of B(t).
        private static EWPoint[] ToBezierForm(EWImmutablePoint _p, EWPoint[] _v)
        {
            int row; // row index

            var c = new EWPoint[_v.Length]; // V(i) - P
            var d = new EWPoint[_v.Length - 1]; // V(i+1) - V(i)
            var w = new EWPoint[_v.Length == 3 ? 4 : 6];
            // control-points for Bezier curve whose zeros represent candidates for closest point to the input parametric curve

            int n = _v.Length - 1; // degree of B(t)
            int degree = 2 * n - 1; // degree of B(t) . P

            float pX = _p.X;
            float pY = _p.Y;

            for (int i = 0; i <= n; ++i)
            {
                EWPoint v = _v[i];
                c[i] = new EWPoint(v.X - pX, v.Y - pY);
            }

            float s = n;
            for (int i = 0; i <= n - 1; ++i)
            {
                EWPoint v = _v[i];
                EWPoint v1 = _v[i + 1];
                d[i] = new EWPoint(s * (v1.X - v.X), s * (v1.Y - v.Y));
            }

            var cd = new float[12];

            // inner product table
            for (row = 0; row < n; ++row)
            {
                EWPoint di = d[row];
                float dX = di.X;
                float dY = di.Y;

                for (int col = 0; col <= n; ++col)
                {
                    int k = GetLinearIndex(n + 1, row, col);
                    cd[k] = dX * c[col].X + dY * c[col].Y;
                }
            }

            // Bezier is uniform parameterized
            float dInv = 1.0f / degree;
            for (int i = 0; i <= degree; ++i)
            {
                w[i] = new EWPoint(i * dInv, 0);
            }

            // reference to appropriate pre-computed coefficients
            float[] z = n == 3 ? ZCubic : ZQuad;

            // accumulate y-coords of the control points along the skew diagonal of the (n-1) x n matrix of c.d and z values
            int m = n - 1;
            for (int k = 0; k <= n + m; ++k)
            {
                int lb = Math.Max(0, k - m);
                int ub = Math.Min(k, n);
                for (int i = lb; i <= ub; ++i)
                {
                    int j = k - i;
                    EWPoint p = w[i + j];
                    int index = GetLinearIndex(n + 1, j, i);
                    p.Y += cd[index] * z[index];
                    w[i + j] = p;
                }
            }

            return w;
        }

        // convert 2D array indices in a k x n matrix to a linear index (this is an interim step ahead of a future implementation optimized for 1D array indexing)
        private static int GetLinearIndex(int _n, int _row, int _col)
        {
            // no range-checking; you break it ... you buy it!
            return _row * _n + _col;
        }

        // how many times does the Bezier curve cross the horizontal axis - the number of roots is less than or equal to this count
        private static int CrossingCount(EWPoint[] _v, int _degree)
        {
            int nCrossings = 0;
            int sign = _v[0].Y < 0 ? -1 : 1;
            int oldSign = sign;
            for (int i = 1; i <= _degree; ++i)
            {
                if (_v[i] != null)
                {
                    sign = _v[i].Y < 0 ? -1 : 1;
                    if (sign != oldSign)
                    {
                        nCrossings++;
                    }

                    oldSign = sign;
                }
            }

            return nCrossings;
        }

        // is the control polygon for a Bezier curve suitably linear for subdivision to terminate?
        private static bool IsControlPolygonLinear(EWPoint[] _v, int _degree)
        {
            // Given array of control points, _v, find the distance from each interior control point to line connecting v[0] and v[degree]

            // implicit equation for line connecting first and last control points
            float a = _v[0].Y - _v[_degree].Y;
            float b = _v[_degree].X - _v[0].X;
            float c = _v[0].X * _v[_degree].Y - _v[_degree].X * _v[0].Y;

            float abSquared = a * a + b * b;
            var distance = new float[_degree];

            for (int i = 1; i < _degree; ++i)
            {
                // Compute distance from each of the points to that line
                distance[i] = a * _v[i].X + b * _v[i].Y + c;
                if (distance[i] > 0.0)
                {
                    distance[i] = (distance[i] * distance[i]) / abSquared;
                }

                if (distance[i] < 0.0)
                {
                    distance[i] = -((distance[i] * distance[i]) / abSquared);
                }
            }

            // Find the largest distance
            float maxDistanceAbove = 0.0f;
            float maxDistanceBelow = 0.0f;
            for (int i = 1; i < _degree; ++i)
            {
                if (distance[i] < 0.0)
                {
                    maxDistanceBelow = Math.Min(maxDistanceBelow, distance[i]);
                }

                if (distance[i] > 0.0)
                {
                    maxDistanceAbove = Math.Max(maxDistanceAbove, distance[i]);
                }
            }

            // Implicit equation for zero line
            const float a1 = 0.0f;
            const float b1 = 1.0f;
            const float c1 = 0.0f;

            // Implicit equation for "above" line
            float a2 = a;
            float b2 = b;
            float c2 = c + maxDistanceAbove;

            float det = a1 * b2 - a2 * b1;
            float dInv = 1.0f / det;

            float intercept1 = (b1 * c2 - b2 * c1) * dInv;

            //  Implicit equation for "below" line
            b2 = b;
            c2 = c + maxDistanceBelow;

            float intercept2 = (b1 * c2 - b2 * c1) * dInv;

            // Compute intercepts of bounding box
            float leftIntercept = Math.Min(intercept1, intercept2);
            float rightIntercept = Math.Max(intercept1, intercept2);

            float error = 0.5f * (rightIntercept - leftIntercept);

            return error < Epsilon;
        }

        // compute intersection of line segnet from first to last control point with horizontal axis
        private static float ComputeXIntercept(EWPoint[] _v, int _degree)
        {
            float XNM = _v[_degree].X - _v[0].X;
            float YNM = _v[_degree].Y - _v[0].Y;
            float XMK = _v[0].X;
            float YMK = _v[0].Y;

            float detInv = -1.0f / YNM;

            return (XNM * YMK - YNM * XMK) * detInv;
        }

        // return roots in [0,1] of a polynomial in Bernstein-Bezier form
        private static float[] FindRoots(EWPoint[] _w, int _degree, int _depth)
        {
            int m = 2 * _degree - 1;

            switch (CrossingCount(_w, _degree))
            {
                case 0:
                    return new float[] { };

                case 1:
                    // Unique solution - stop recursion when the tree is deep enough (return 1 solution at midpoint)
                    if (_depth >= MaxDepth)
                    {
                        return new[] {0.5f * (_w[0].X + _w[m].X)};
                    }

                    if (IsControlPolygonLinear(_w, _degree))
                    {
                        return new[] {ComputeXIntercept(_w, _degree)};
                    }

                    break;
            }

            // Otherwise, solve recursively after subdividing control polygon
            var left = new EWPoint[_degree + 1];
            var right = new EWPoint[_degree + 1];

            // child solutions
            Subdivide(_w, 0.5f, left, right);
            float[] leftT = FindRoots(left, _degree, _depth + 1);
            float[] rightT = FindRoots(right, _degree, _depth + 1);

            var t = new float[leftT.Length + rightT.Length]; // t-values of roots

            // Gather solutions together
            for (int i = 0; i < leftT.Length; ++i)
            {
                t[i] = leftT[i];
            }

            for (int i = 0; i < rightT.Length; ++i)
            {
                t[i + leftT.Length] = rightT[i];
            }

            return t;
        }

        /**
      * subdivide( _c:Array, _t:Number, _left:Array, _right:Array ) - deCasteljau subdivision of an arbitrary-order Bezier curve
      *
      * @param _c:Array array of control points for the Bezier curve
      * @param _t:Number t-parameter at which the curve is subdivided (must be in (0,1) = no check at this point
      * @param _left:Array reference to an array in which the control points, <code>Array</code> of <code>Point</code> references, of the left control cage after subdivision are stored
      * @param _right:Array reference to an array in which the control points, <code>Array</code> of <code>Point</code> references, of the right control cage after subdivision are stored
      * @return nothing 
      *
      * @since 1.0
      *
      */

        public static void Subdivide(EWPoint[] aCurve, float aPoint, EWPoint[] aLeftCurve, EWPoint[] aRightCurve)
        {
            int degree = aCurve.Length - 1;
            int n = degree + 1;
            var p = new EWPoint[MaxDepth];
            for (int i = 0; i < aCurve.Length; i++)
            {
                p[i] = aCurve[i];
            }

            float t1 = 1.0f - aPoint;

            for (int i = 1; i <= degree; ++i)
            {
                for (int j = 0; j <= degree - i; ++j)
                {
                    var vertex = new EWPoint();
                    int ij = GetLinearIndex(n, i, j);
                    int im1j = GetLinearIndex(n, i - 1, j);
                    int im1jp1 = GetLinearIndex(n, i - 1, j + 1);

                    vertex.X = t1 * p[im1j].X + aPoint * p[im1jp1].X;
                    vertex.Y = t1 * p[im1j].Y + aPoint * p[im1jp1].Y;

                    p[ij] = vertex;
                }
            }

            for (int j = 0; j <= degree; ++j)
            {
                int index = GetLinearIndex(n, j, 0);
                aLeftCurve[j] = p[index];
            }

            for (int j = 0; j <= degree; ++j)
            {
                int index = GetLinearIndex(n, degree - j, j);
                aRightCurve[j] = p[index];
            }
        }
    }
}