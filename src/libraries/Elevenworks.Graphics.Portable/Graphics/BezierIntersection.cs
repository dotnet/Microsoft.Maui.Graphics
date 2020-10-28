using System;

namespace Elevenworks.Graphics
{
    public static class BezierIntersection
    {
        private const float Tolel = 0.01f;

        public static EWPoint[] GetPointsWhereLineIntersectsCubic(EWImmutablePoint aLineStart, EWImmutablePoint aLineEnd, EWImmutablePoint aBezierStart, EWImmutablePoint aControlPoint1,
            EWImmutablePoint aControlPoint2, EWImmutablePoint aBezierEndPoint)
        {
            float tintt = 0;
            const int ndeg1 = 4;

            var x1 = new float[ndeg1];
            var xint = new float[ndeg1];

            var pnt1 = new EWImmutablePoint[ndeg1];
            var pnt2 = new EWImmutablePoint[2];

            pnt1[0] = aBezierStart;
            pnt1[1] = aControlPoint1;
            pnt1[2] = aControlPoint2;
            pnt1[3] = aBezierEndPoint;

            pnt2[0] = aLineStart;
            pnt2[1] = aLineEnd;

            float a = pnt2[0].Y - pnt2[1].Y;
            float b = pnt2[1].X - pnt2[0].X;
            float tmp = (float) Math.Sqrt(a * a + b * b);
            a /= tmp;
            b /= tmp;
            float c = -(a * pnt2[0].X + b * pnt2[0].Y);
            for (int i = 0; i < ndeg1; i++)
            {
                x1[i] = pnt1[i].X * a + pnt1[i].Y * b + c;
            }

            // Get the number of intersections
            int nint = Croot(x1, xint, ndeg1 - 1, ref tintt);
            if (nint == 0)
            {
                return null;
            }

            var vPoints = new EWPoint[nint];

            for (int i = 0; i < nint; i++)
            {
                vPoints[i] = Castljau(xint[i], ndeg1 - 1, pnt1);
            }

            return vPoints;
        }

        public static EWPoint[] GetPointsWhereLineIntersectsQuadratic(EWImmutablePoint aLineStart, EWImmutablePoint aLineEnd, EWImmutablePoint aBezierStart, EWImmutablePoint aControlPoint1,
            EWImmutablePoint aBezierEndPoint)
        {
            float tintt = 0;
            const int ndeg1 = 3;

            var x1 = new float[ndeg1];
            var xint = new float[ndeg1];

            var pnt1 = new EWImmutablePoint[ndeg1];
            var pnt2 = new EWImmutablePoint[2];

            pnt1[0] = aBezierStart;
            pnt1[1] = aControlPoint1;
            pnt1[2] = aBezierEndPoint;

            pnt2[0] = aLineStart;
            pnt2[1] = aLineEnd;

            float a = pnt2[0].Y - pnt2[1].Y;
            float b = pnt2[1].X - pnt2[0].X;
            float tmp = (float) Math.Sqrt(a * a + b * b);
            a /= tmp;
            b /= tmp;
            float c = -(a * pnt2[0].X + b * pnt2[0].Y);
            for (int i = 0; i < ndeg1; i++)
            {
                x1[i] = pnt1[i].X * a + pnt1[i].Y * b + c;
            }

            // Get the number of intersections
            int nint = Croot(x1, xint, ndeg1 - 1, ref tintt);
            if (nint == 0)
            {
                return null;
            }

            var vPoints = new EWPoint[nint];

            for (int i = 0; i < nint; i++)
            {
                vPoints[i] = Castljau(xint[i], ndeg1 - 1, pnt1);
            }

            return vPoints;
        }

        private static EWPoint Castljau(float u, int ndeg, EWImmutablePoint[] pnt)
        {
            var wx = new float[4];
            var wy = new float[4];
            for (int i = 0; i <= ndeg; i++)
            {
                wx[i] = pnt[i].X;
                wy[i] = pnt[i].Y;
            }

            for (int m = 1; m <= ndeg; m++)
            {
                for (int j = 0; j <= ndeg - m; j++)
                {
                    wx[j] += (wx[j + 1] - wx[j]) * u;
                    wy[j] += (wy[j + 1] - wy[j]) * u;
                }
            }

            return new EWPoint(wx[0], wy[0]);
        }

        private static int Croot(float[] x1, float[] xint, int ndeg, ref float tintt)
        {
            var t1 = new float[2];
            int nint = 0;
            t1[0] = 0.0f;
            t1[1] = 1.0f;
            float span = 1.0f / ndeg;
            int iover = Curtrm(x1, t1, ndeg, span, ref tintt);
            if (iover == 1)
            {
                xint[nint] = tintt;
                return 1;
            }

            if (iover == -1)
            {
                if (t1[1] - t1[0] < 0.97999999999999998f)
                {
                    Tspli1(x1, t1[0], 1.0f - t1[1], ndeg);
                }

                nint = CrootSub(x1, t1, nint, xint, ndeg, span, ref tintt);
            }

            return nint;
        }

        private static int CrootSub(float[] x1, float[] t1, int nint, float[] xint, int ndeg, float span, ref float tintt)
        {
            var x1L = new float[8];
            var x1R = new float[8];
            var t1L = new float[2];
            var t1R = new float[2];
            Tdiv(x1, x1L, x1R, 0.5f, ndeg);
            t1L[1] = t1[0] + (t1[1] - t1[0]) * 0.5f;
            t1L[0] = t1[0];
            t1R[0] = t1L[1];
            int iover = Curtrm(x1L, t1L, ndeg, span, ref tintt);
            if (iover == 1)
            {
                xint[nint] = tintt;
                nint++;
            }
            else if (iover == -1)
            {
                nint = CrootSub(x1L, t1L, nint, xint, ndeg, span, ref tintt);
            }

            t1R[1] = t1[1];
            iover = Curtrm(x1R, t1R, ndeg, span, ref tintt);
            if (iover == 1)
            {
                xint[nint] = tintt;
                nint++;
            }
            else if (iover == -1)
            {
                nint = CrootSub(x1R, t1R, nint, xint, ndeg, span, ref tintt);
            }

            return nint;
        }

        private static int Curtrm(float[] x1, float[] t1, int ndeg, float span, ref float tintt)
        {
            const float zero = 9.9999999999999995E-007f;
            var cf = new float[10];
            for (int i = 0; i <= ndeg; i++)
            {
                cf[i] = x1[i];
            }

            int ic = 0;
            do
            {
                if (cf[0] > zero && cf[ndeg] > zero)
                {
                    for (int i = 1; i < ndeg; i++)
                    {
                        if (cf[i] < -zero)
                        {
                            return -1;
                        }
                    }

                    return 0;
                }

                if (cf[0] < -zero && cf[ndeg] < -zero)
                {
                    for (int i = 1; i < ndeg; i++)
                    {
                        if (cf[i] > zero)
                        {
                            return -1;
                        }
                    }

                    return 0;
                }

                EWVector tt = Root(ndeg, span, cf);
                float t0 = t1[0];
                t1[0] = t0 + (t1[1] - t0) * tt.U;
                t1[1] = t0 + (t1[1] - t0) * tt.V;
                if (t1[1] - t1[0] < Tolel && Math.Abs(t1[1] - t1[0] - -1) > Geometry.Epsilon)
                {
                    tintt = (t1[0] + t1[1]) * 0.5f;
                    return 1;
                }

                if (tt.U < 0.02f && tt.V > 0.97999999999999998f)
                {
                    return -1;
                }

                Tspli1(cf, tt.U, 1.0f - tt.V, ndeg);
            } while (++ic < 10);

            return 0;
        }

        private static void Tspli1(float[] x, float tl, float ur, int ndeg)
        {
            for (int i = 1; i <= ndeg; i++)
            {
                for (int j = 0; j <= ndeg - i; j++)
                {
                    x[j] += tl * (x[j + 1] - x[j]);
                }
            }

            if (ur > 0.99999899999999997D)
            {
                return;
            }

            float tmp = ur / (1.0f - tl);
            for (int i = 1; i <= ndeg; i++)
            {
                for (int j = ndeg; j >= i; j--)
                {
                    x[j] += (x[j - 1] - x[j]) * tmp;
                }
            }
        }

        private static void Tdiv(float[] x, float[] xleft, float[] xright, float t, int ndeg)
        {
            var bz = new float[4, 4];
            for (int i = 0; i <= ndeg; i++)
            {
                bz[0, i] = x[i];
            }

            for (int k = 0; k < ndeg; k++)
            {
                for (int i = 1; i <= ndeg; i++)
                {
                    bz[k + 1, i] = bz[k, i - 1] + t * (bz[k, i] - bz[k, i - 1]);
                }
            }

            for (int i = 0; i <= ndeg; i++)
            {
                xleft[i] = bz[i, i];
                xright[i] = bz[ndeg - i, ndeg];
            }
        }

        private static EWVector Root(int nmdeg, float span, float[] cf)
        {
            float tmin = nmdeg;
            float tmax = 0.0f;
            for (int i = 0; i < nmdeg; i++)
            {
                if (cf[i] > 0.0D)
                {
                    for (int j = i + 1; j <= nmdeg; j++)
                    {
                        if (cf[j] < 0.0D)
                        {
                            float t = (cf[i] / (cf[i] - cf[j])) * (j - i) + i;
                            if (t > tmax)
                            {
                                tmax = t;
                            }

                            if (t < tmin)
                            {
                                tmin = t;
                            }
                        }
                    }
                }
                else if (cf[i] < 0.0D)
                {
                    for (int j = i + 1; j <= nmdeg; j++)
                    {
                        if (cf[j] > 0.0D)
                        {
                            float t = (cf[i] / (cf[i] - cf[j])) * (j - i) + i;
                            if (t > tmax)
                            {
                                tmax = t;
                            }

                            if (t < tmin)
                            {
                                tmin = t;
                            }
                        }
                    }
                }
            }

            float tl = tmin * span;
            float ur = tmax * span;
            return new EWVector(tl, ur);
        }
    }
}