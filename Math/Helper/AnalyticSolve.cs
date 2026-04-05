using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Helper
{
    public static class AnalyticSolve
    {
        public static Func<double, double> AnalyticExponential(double a, double y0, double t0)
        {
            return t => y0 * System.Math.Exp(a * (t - t0));
        }

        public static (Func<double, double> x, Func<double, double> v) AnalyticSho(double omega, double x0, double v0, double t0)
        {
            if (omega == 0.0)
            {
                return (
                    x: t => x0 + v0 * (t - t0),
                    v: t => v0
                );
            }

            return (
                x: t =>
                {
                    double tau = t - t0;
                    return x0 * System.Math.Cos(omega * tau) + (v0 / omega) * System.Math.Sin(omega * tau);
                },
                v: t =>
                {
                    double tau = t - t0;
                    return -x0 * omega * System.Math.Sin(omega * tau) + v0 * System.Math.Cos(omega * tau);
                }
            );
        }

        public static (Func<double, double> theta, Func<double, double> omega) AnalyticLinearPendulum(
        double g,
        double length,
        double theta0,
        double omega0,
        double t0)
        {
            if (g <= 0) throw new ArgumentOutOfRangeException(nameof(g), "g должно быть > 0.");
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "length должно быть > 0.");

            double w = System.Math.Sqrt(g / length); // ω = sqrt(g/L)

            return (
                theta: t =>
                {
                    double tau = t - t0;
                    return theta0 * System.Math.Cos(w * tau) + (omega0 / w) * System.Math.Sin(w * tau);
                },
                omega: t =>
                {
                    double tau = t - t0;
                    return -theta0 * w * System.Math.Sin(w * tau) + omega0 * System.Math.Cos(w * tau);
                }
            );
        }
    }
}
