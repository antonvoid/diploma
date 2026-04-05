using Math.Interfaces;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.OdeSolvers;

namespace Math.Service
{
    public class MathService : IMathService
    {
        /// <summary>
    /// 1) Простейшее ОДУ: y' = a*y, y(t0)=y0
    /// Решаем численно RK4 (скалярный вариант).
    /// </summary>
    public (double[] t, double[] y) SolveExponentialGrowthDecay_RK4(
        double a,
        double y0,
        double t0,
        double t1,
        int n)
        {
            if (n < 2) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 2.");
            if (t1 <= t0) throw new ArgumentOutOfRangeException(nameof(t1), "t1 должно быть > t0.");

            // Math.NET: FourthOrder(double y0, double start, double end, int N, Func<double,double,double> f) [[3]]
            Func<double, double, double> f = (t, y) => a * y;

            var yApprox = RungeKutta.FourthOrder(y0, t0, t1, n, f);

            var t = Linspace(t0, t1, n);
            return (t, yApprox);
        }

        /// <summary>
        /// 2) Гармонический осциллятор: x'' + ω^2 x = 0 [[7]]
        /// Приводим к системе 1-го порядка:
        ///   x' = v
        ///   v' = -ω^2 x
        /// Решаем RK4 (векторный вариант).
        /// </summary>
        public (double[] t, double[] x, double[] v) SolveSimpleHarmonicOscillator_RK4(
            double omega,
            double x0,
            double v0,
            double t0,
            double t1,
            int n)
        {
            if (omega < 0) throw new ArgumentOutOfRangeException(nameof(omega), "omega должно быть >= 0.");
            if (n < 2) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 2.");
            if (t1 <= t0) throw new ArgumentOutOfRangeException(nameof(t1), "t1 должно быть > t0.");

            var y0 = Vector<double>.Build.Dense(new[] { x0, v0 });

            Func<double, Vector<double>, Vector<double>> f = (t, y) =>
            {
                double x = y[0];
                double v = y[1];
                return Vector<double>.Build.Dense(new[]
                {
                v,
                -(omega * omega) * x
            });
            };

            // Math.NET: FourthOrder(Vector y0, double start, double end, int N, Func<double,Vector,Vector> f) [[3]]
            var yApprox = RungeKutta.FourthOrder(y0, t0, t1, n, f);

            var tGrid = Linspace(t0, t1, n);
            var x = new double[n];
            var vArr = new double[n];

            for (int i = 0; i < n; i++)
            {
                x[i] = yApprox[i][0];
                vArr[i] = yApprox[i][1];
            }

            return (tGrid, x, vArr);
        }

        /// <summary>
        /// 3) Нелинейный маятник: θ'' + (g/L) sin(θ) = 0 [[8]]
        /// Приводим к системе 1-го порядка:
        ///   θ' = ω
        ///   ω' = -(g/L) sin(θ)
        /// Решаем RK4 (векторный вариант).
        /// </summary>
        public (double[] t, double[] theta, double[] omega) SolveNonlinearPendulum_RK4(
            double g,
            double length,
            double theta0,
            double omega0,
            double t0,
            double t1,
            int n)
        {
            if (g <= 0) throw new ArgumentOutOfRangeException(nameof(g), "g должно быть > 0.");
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length), "length должно быть > 0.");
            if (n < 2) throw new ArgumentOutOfRangeException(nameof(n), "n должно быть >= 2.");
            if (t1 <= t0) throw new ArgumentOutOfRangeException(nameof(t1), "t1 должно быть > t0.");

            var y0 = Vector<double>.Build.Dense(new[] { theta0, omega0 });
            double k = g / length;

            Func<double, Vector<double>, Vector<double>> f = (t, y) =>
            {
                double theta = y[0];
                double omega = y[1];
                return Vector<double>.Build.Dense(new[]
                {
                omega,
                -k * System.Math.Sin(theta)
            });
            };

            var yApprox = RungeKutta.FourthOrder(y0, t0, t1, n, f);

            var tGrid = Linspace(t0, t1, n);
            var theta = new double[n];
            var omegaArr = new double[n];

            for (int i = 0; i < n; i++)
            {
                theta[i] = yApprox[i][0];
                omegaArr[i] = yApprox[i][1];
            }

            return (tGrid, theta, omegaArr);
        }

        private static double[] Linspace(double start, double end, int n)
        {
            var arr = new double[n];
            double step = (end - start) / (n - 1);
            for (int i = 0; i < n; i++)
                arr[i] = start + step * i;
            return arr;
        }
    }
}
