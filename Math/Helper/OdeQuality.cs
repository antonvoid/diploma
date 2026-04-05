using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Helper
{
    public static class OdeQuality
    {
        public static double Rms(double[] numeric, double[] analytic)
        {
            if (numeric.Length != analytic.Length)
                throw new ArgumentException("numeric and analytic must have same length.");

            double sumSq = 0.0;
            for (int i = 0; i < numeric.Length; i++)
            {
                double e = numeric[i] - analytic[i];
                sumSq += e * e;
            }

            return System.Math.Sqrt(sumSq / numeric.Length);
        }

        public static double[] Evaluate(double[] t, Func<double, double> f)
        {
            var y = new double[t.Length];
            for (int i = 0; i < t.Length; i++)
                y[i] = f(t[i]);
            return y;
        }
    }
}
