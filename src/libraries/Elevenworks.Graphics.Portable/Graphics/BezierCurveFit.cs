using System;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public class BezierCurveFit
    {
        /** Prevent instance creation. */

        private BezierCurveFit()
        {
        }

        /**
     * Fit a Bezier curve to a set of digitized points.
     *
     * @param d  Array of digitized points.
     * @param error User-defined error squared.
     * @return Returns a EWPath containing the bezier curves.
     */

        public static EWPath FitCurve(List<EWPoint> d, float error)
        {
            /*  Unit tangent vectors at endpoints */
            var bezierPath = new EWPath();
            bezierPath.MoveTo(d[0].X, d[0].Y);

            EWPoint tHat1 = ComputeLeftTangent(d, 0);
            EWPoint tHat2 = ComputeRightTangent(d, d.Count - 1);

            FitCubic(d, 0, d.Count - 1, tHat1, tHat2, error, bezierPath);
            return bezierPath;
        }

        /**
     * Fit a Bezier curve to a (sub)set of digitized points.
     *
     * @param d  Array of digitized points.
     * @param first Indice of first point in d.
     * @param last Indice of last point in d.
     * @param tHat1 Unit tangent vectors at start point.
     * @param tHat2 Unit tanget vector at end point.
     * @param error User-defined error squared.
     * @param bezierPath Path to which the bezier curve segments are added.
     */

        private static void FitCubic(List<EWPoint> d, int first, int last, EWPoint tHat1, EWPoint tHat2, float error, EWPath bezierPath)
        {
            EWPoint[] bezCurve; /*Control points of fitted Bezier curve*/
            float maxError; /*  Maximum fitting error	 */
            var splitPoint = new int[1];
            /*  Point to split point set at.
         This is an array of size one, because we need it as an input/output parameter.
         */
            int nPts; /*  Number of points in subset  */
            float iterationError; /*Error below which you try iterating  */
            const int maxIterations = 4;
            int i;

            iterationError = error * error;
            nPts = last - first + 1;

            /*  Use heuristic if region only has two points in it */
            if (nPts == 2)
            {
                float dist = V2DistanceBetween2Points(d[last], d[first]) / 3.0f;

                bezCurve = new EWPoint[4];
                for (i = 0; i < bezCurve.Length; i++)
                {
                    bezCurve[i] = new EWPoint();
                }

                bezCurve[0] = d[first];
                bezCurve[3] = d[last];
                V2Add(bezCurve[0], V2Scale(tHat1, dist), bezCurve[1]);
                V2Add(bezCurve[3], V2Scale(tHat2, dist), bezCurve[2]);
                bezierPath.CurveTo(bezCurve[1].X, bezCurve[1].Y, bezCurve[2].X, bezCurve[2].Y, bezCurve[3].X, bezCurve[3].Y);
                return;
            }

            /*  Parameterize points, and attempt to fit curve */
            var u = ChordLengthParameterize(d, first, last);
            bezCurve = GenerateBezier(d, first, last, u, tHat1, tHat2);

            /*  Find max deviation of points to fitted curve */
            maxError = ComputeMaxError(d, first, last, bezCurve, u, splitPoint);
            if (maxError < error)
            {
                bezierPath.CurveTo(bezCurve[1].X, bezCurve[1].Y, bezCurve[2].X, bezCurve[2].Y, bezCurve[3].X, bezCurve[3].Y);
                return;
            }

            /*  If error not too large, try some reparameterization  */
            /*  and iteration */
            if (maxError < iterationError)
            {
                for (i = 0; i < maxIterations; i++)
                {
                    float[] uPrime = Reparameterize(d, first, last, u, bezCurve); /*  Improved parameter values */
                    bezCurve = GenerateBezier(d, first, last, uPrime, tHat1, tHat2);
                    maxError = ComputeMaxError(d, first, last, bezCurve, uPrime, splitPoint);
                    if (maxError < error)
                    {
                        bezierPath.CurveTo(bezCurve[1].X, bezCurve[1].Y, bezCurve[2].X, bezCurve[2].Y, bezCurve[3].X, bezCurve[3].Y);
                        return;
                    }

                    u = uPrime;
                }
            }

            /* Fitting failed -- split at max error point and fit recursively */
            var tHatCenter = ComputeCenterTangent(d, splitPoint[0]);
            FitCubic(d, first, splitPoint[0], tHat1, tHatCenter, error, bezierPath);
            V2Negate(tHatCenter);
            FitCubic(d, splitPoint[0], last, tHatCenter, tHat2, error, bezierPath);
        }

        /**
     * Use least-squares method to find Bezier control points for region.
     *
     * @param d  Array of digitized points.
     * @param first Indice of first point in d.
     * @param last Indice of last point in d.
     * @param uPrime Parameter values for region .
     * @param tHat1 Unit tangent vectors at start point.
     * @param tHat2 Unit tanget vector at end point.
     */

        private static EWPoint[] GenerateBezier(List<EWPoint> d, int first, int last, float[] uPrime, EWPoint tHat1, EWPoint tHat2)
        {
            int i;
            var a = new EWPoint[d.Count + 1, 2]; /* Precomputed rhs for eqn	*/
            int nPts; /* Number of pts in sub-curve */
            var c = new float[2, 2]; /* Matrix C		*/
            var x = new float[2]; /* Matrix X			*/
            float detC0C1, /* Determinants of matrices	*/ detC0X, detXc1;
            float alphaL, /* Alpha values, left and right	*/ alphaR;

            var bezCurve = new EWPoint[4];
            for (i = 0; i < bezCurve.Length; i++)
            {
                bezCurve[i] = new EWPoint();
            }

            nPts = last - first + 1;

            /* Compute the A's	*/
            for (i = 0; i < nPts; i++)
            {
                var v1 = new EWPoint(tHat1);
                var v2 = new EWPoint(tHat2);
                V2Scale(v1, B1(uPrime[i]));
                V2Scale(v2, B2(uPrime[i]));
                a[i, 0] = v1;
                a[i, 1] = v2;
            }

            /* Create the C and X matrices	*/
            c[0, 0] = 0.0f;
            c[0, 1] = 0.0f;
            c[1, 0] = 0.0f;
            c[1, 1] = 0.0f;
            x[0] = 0.0f;
            x[1] = 0.0f;

            for (i = 0; i < nPts; i++)
            {
                c[0, 0] += V2Dot(a[i, 0], a[i, 0]);
                c[0, 1] += V2Dot(a[i, 0], a[i, 1]);
                /*					C[1,0] += V2Dot(&A[i,0], &A[i,1]);*/
                c[1, 0] = c[0, 1];
                c[1, 1] += V2Dot(a[i, 1], a[i, 1]);

                var tmp = V2SubII(d[first + i],
                    V2AddII(V2ScaleIii(d[first], B0(uPrime[i])),
                        V2AddII(V2ScaleIii(d[first], B1(uPrime[i])), V2AddII(V2ScaleIii(d[last], B2(uPrime[i])), V2ScaleIii(d[last], B3(uPrime[i])))))); /* Utility variable		*/

                x[0] += V2Dot(a[i, 0], tmp);
                x[1] += V2Dot(a[i, 1], tmp);
            }

            /* Compute the determinants of C and X	*/
            detC0C1 = c[0, 0] * c[1, 1] - c[1, 0] * c[0, 1];
            detC0X = c[0, 0] * x[1] - c[0, 1] * x[0];
            detXc1 = x[0] * c[1, 1] - x[1] * c[0, 1];

            /* Finally, derive alpha values	*/
            if (Math.Abs(detC0C1 - 0.0) < Geometry.Epsilon)
            {
                detC0C1 = (c[0, 0] * c[1, 1]) * 10e-12f;
            }

            alphaL = detXc1 / detC0C1;
            alphaR = detC0X / detC0C1;

            /*  If alpha negative, use the Wu/Barsky heuristic (see text) */
            /* (if alpha is 0, you get coincident control points that lead to
         * divide by zero in any subsequent NewtonRaphsonRootFind() call. */
            if (alphaL < 1.0e-6 || alphaR < 1.0e-6)
            {
                float dist = V2DistanceBetween2Points(d[last], d[first]) / 3.0f;

                bezCurve[0] = d[first];
                bezCurve[3] = d[last];
                V2Add(bezCurve[0], V2Scale(tHat1, dist), bezCurve[1]);
                V2Add(bezCurve[3], V2Scale(tHat2, dist), bezCurve[2]);
                return (bezCurve);
            }

            /*  First and last control points of the Bezier curve are */
            /*  positioned exactly at the first and last data points */
            /*  Control points 1 and 2 are positioned an alpha distance out */
            /*  on the tangent vectors, left and right, respectively */
            bezCurve[0] = d[first];
            bezCurve[3] = d[last];
            V2Add(bezCurve[0], V2Scale(tHat1, alphaL), bezCurve[1]);
            V2Add(bezCurve[3], V2Scale(tHat2, alphaR), bezCurve[2]);
            return (bezCurve);
        }

        /**
     * Given set of points and their parameterization, try to find
     * a better parameterization.
     *
     * @param d  Array of digitized points.
     * @param first Indice of first point of region in d.
     * @param last Indice of last point of region in d.
     * @param u Current parameter values.
     * @param bezCurve Current fitted curve.
     */

        private static float[] Reparameterize(List<EWPoint> d, int first, int last, float[] u, EWPoint[] bezCurve)
        {
            int nPts = last - first + 1;
            int i;

            var uPrime = new float[nPts];
            for (i = first; i <= last; i++)
            {
                uPrime[i - first] = NewtonRaphsonRootFind(bezCurve, d[i], u[i - first]);
            }

            return (uPrime);
        }

        /**
     * Use Newton-Raphson iteration to find better root.
     *
     * @param Q  Current fitted bezier curve.
     * @param P  Digitized point.
     * @param u  Parameter value vor P.
     */

        private static float NewtonRaphsonRootFind(EWPoint[] q, EWPoint p, float u)
        {
            float numerator, denominator;
            EWPoint[] q1 = new EWPoint[3], q2 = new EWPoint[2]; /*  Q' and Q''			*/
            float uPrime; /*  Improved u	*/
            int i;

            /* Compute Q(u)	*/
            var qU = BezierII(3, q, u);

            /* Generate control vertices for Q'	*/
            for (i = 0; i <= 2; i++)
            {
                q1[i] = new EWPoint((q[i + 1].X - q[i].X) * 3.0f, (q[i + 1].Y - q[i].Y) * 3.0f);
            }

            /* Generate control vertices for Q'' */
            for (i = 0; i <= 1; i++)
            {
                q2[i] = new EWPoint((q1[i + 1].X - q1[i].X) * 2.0f, (q1[i + 1].Y - q1[i].Y) * 2.0f);
            }

            /* Compute Q'(u) and Q''(u)	*/
            var q1U = BezierII(2, q1, u);
            var q2U = BezierII(1, q2, u);

            /* Compute f(u)/f'(u) */
            numerator = (qU.X - p.X) * (q1U.X) + (qU.Y - p.Y) * (q1U.Y);
            denominator = (q1U.X) * (q1U.X) + (q1U.Y) * (q1U.Y) + (qU.X - p.X) * (q2U.X) + (qU.Y - p.Y) * (q2U.Y);

            /* u = u - f(u)/f'(u) */
            uPrime = u - (numerator / denominator);
            return (uPrime);
        }

        /**
     * Evaluate a Bezier curve at a particular parameter value.
     *
     * @param degree  The degree of the bezier curve.
     * @param V  Array of control points.
     * @param t  Parametric value to find point for.
     */

        private static EWPoint BezierII(int degree, EWPoint[] v, float t)
        {
            int i;

            /* Copy array	*/
            var vtemp = new EWPoint[degree + 1];
            for (i = 0; i <= degree; i++)
            {
                vtemp[i] = new EWPoint(v[i]);
            }

            /* Triangle computation	*/
            for (i = 1; i <= degree; i++)
            {
                for (int j = 0; j <= degree - i; j++)
                {
                    vtemp[j].X = (1.0f - t) * vtemp[j].X + t * vtemp[j + 1].X;
                    vtemp[j].Y = (1.0f - t) * vtemp[j].Y + t * vtemp[j + 1].Y;
                }
            }

            var q = vtemp[0];
            return q;
        }

        /**
     *  B0, B1, B2, B3 :
     *	Bezier multipliers
     */

        private static float B0(float u)
        {
            float tmp = 1.0f - u;
            return (tmp * tmp * tmp);
        }

        private static float B1(float u)
        {
            float tmp = 1.0f - u;
            return (3 * u * (tmp * tmp));
        }

        private static float B2(float u)
        {
            float tmp = 1.0f - u;
            return (3 * u * u * tmp);
        }

        private static float B3(float u)
        {
            return (u * u * u);
        }

        /**
     * Approximate unit tangents at "left" endpoint of digitized curve.
     *
     * @param d Digitized points.
     * @param end Index to "left" end of region.
     */

        private static EWPoint ComputeLeftTangent(List<EWPoint> d, int end)
        {
            var tHat1 = V2SubII(d[end + 1], d[end]);
            tHat1 = V2Normalize(tHat1);
            return tHat1;
        }

        /**
     * Approximate unit tangents at "right" endpoint of digitized curve.
     *
     * @param d Digitized points.
     * @param end Index to "right" end of region.
     */

        private static EWPoint ComputeRightTangent(List<EWPoint> d, int end)
        {
            var tHat2 = V2SubII(d[end - 1], d[end]);
            tHat2 = V2Normalize(tHat2);
            return tHat2;
        }

        /**
     * Approximate unit tangents at "center" of digitized curve.
     *
     * @param d Digitized points.
     * @param center Index to "center" end of region.
     */

        private static EWPoint ComputeCenterTangent(List<EWPoint> d, int center)
        {
            EWPoint tHatCenter = new EWPoint();

            var v1 = V2SubII(d[center - 1], d[center]);
            var v2 = V2SubII(d[center], d[center + 1]);
            tHatCenter.X = (v1.X + v2.X) / 2.0f;
            tHatCenter.Y = (v1.Y + v2.Y) / 2.0f;
            tHatCenter = V2Normalize(tHatCenter);
            return tHatCenter;
        }

        /**
     * Assign parameter values to digitized points
     * using relative distances between points.
     *
     * @param d Digitized points.
     * @param first Indice of first point of region in d.
     * @param last Indice of last point of region in d.
     */

        private static float[] ChordLengthParameterize(List<EWPoint> d, int first, int last)
        {
            int i;

            var u = new float[last - first + 1];

            u[0] = 0.0f;
            for (i = first + 1; i <= last; i++)
            {
                u[i - first] = u[i - first - 1] + V2DistanceBetween2Points(d[i], d[i - 1]);
            }

            for (i = first + 1; i <= last; i++)
            {
                u[i - first] = u[i - first] / u[last - first];
            }

            return (u);
        }

        /**
     * Find the maximum squared distance of digitized points
     * to fitted curve.
     *
     * @param d Digitized points.
     * @param first Indice of first point of region in d.
     * @param last Indice of last point of region in d.
     * @param bezCurve Fitted Bezier curve
     * @param u Parameterization of points*
     * @param splitPoint Point of maximum error (input/output parameter, must be
     * an array of 1)
     */

        private static float ComputeMaxError(List<EWPoint> d, int first, int last, EWPoint[] bezCurve, float[] u, int[] splitPoint)
        {
            int i;
            float maxDist; /*  Maximum error */

            splitPoint[0] = (last - first + 1) / 2;
            maxDist = 0.0f;
            for (i = first + 1; i < last; i++)
            {
                var p = BezierII(3, bezCurve, u[i - first]); /*  Point on curve */
                var v = V2SubII(p, d[i]); /*  Vector from point to curve */
                float dist = V2SquaredLength(v); /*  Current error */
                if (dist >= maxDist)
                {
                    maxDist = dist;
                    splitPoint[0] = i;
                }
            }

            return (maxDist);
        }

        private static EWImmutablePoint V2AddII(EWImmutablePoint a, EWImmutablePoint b)
        {
            var c = new EWPoint {X = a.X + b.X, Y = a.Y + b.Y};
            return c;
        }

        private static EWImmutablePoint V2ScaleIii(EWImmutablePoint v, float s)
        {
            var result = new EWPoint {X = v.X * s, Y = v.Y * s};
            return result;
        }

        private static EWPoint V2SubII(EWImmutablePoint a, EWImmutablePoint b)
        {
            var c = new EWPoint {X = a.X - b.X, Y = a.Y - b.Y};
            return (c);
        }

        /* -------------------------------------------------------------------------
     * GraphicsGems.c
     * 2d and 3d Vector C Library
     * by Andrew Glassner
     * from "Graphics Gems", Academic Press, 1990
     * -------------------------------------------------------------------------
     */
        /**
     * Return the distance between two points
     */

        private static float V2DistanceBetween2Points(EWImmutablePoint a, EWImmutablePoint b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float) Math.Sqrt((dx * dx) + (dy * dy));
        }

        /**
     * Scales the input vector to the new length and returns it.
     */

        private static EWImmutablePoint V2Scale(EWPoint v, float newlen)
        {
            float len = V2Length(v);
            if (Math.Abs(len - 0.0) > Geometry.Epsilon)
            {
                v.X *= newlen / len;
                v.Y *= newlen / len;
            }

            return v;
        }

        /**
     * Returns length of input vector.
     */

        private static float V2Length(EWImmutablePoint a)
        {
            return (float) Math.Sqrt(V2SquaredLength(a));
        }

        /**
     * Returns squared length of input vector.
     */

        private static float V2SquaredLength(EWImmutablePoint a)
        {
            return (a.X * a.X) + (a.Y * a.Y);
        }

        /**
     * Return vector sum c = a+b.
     */

        private static void V2Add(EWImmutablePoint a, EWImmutablePoint b, EWPoint c)
        {
            c.X = a.X + b.X;
            c.Y = a.Y + b.Y;
        }

        /**
     * Negates the input vector and returns it.
     */

        private static void V2Negate(EWPoint v)
        {
            v.X = -v.X;
            v.Y = -v.Y;
        }

        /**
     * Return the dot product of vectors a and b.
     */

        private static float V2Dot(EWImmutablePoint a, EWImmutablePoint b)
        {
            return (a.X * b.X) + (a.Y * b.Y);
        }

        /**
     * Normalizes the input vector and returns it.
     */

        private static EWPoint V2Normalize(EWPoint v)
        {
            float len = V2Length(v);
            if (Math.Abs(len - 0.0) > Geometry.Epsilon)
            {
                v.X /= len;
                v.Y /= len;
            }

            return v;
        }
    }
}