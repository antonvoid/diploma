using Math.Interfaces;
using System.Diagnostics;

namespace API.Metrics
{
    public class MeasuredMathService : IMathService
    {
        private readonly IMathService _inner;

        public MeasuredMathService(IMathService inner) => _inner = inner;

        public double Compute(double x)
        {
            var sw = Stopwatch.StartNew();
            var status = "ok";
            Console.WriteLine(status);

            try
            {
                return _inner.Compute(x);
            }
            catch
            {
                status = "error";
                throw;
            }
            finally
            {
                sw.Stop();
                MathMetrics.ComputeDurationSeconds
                                .WithLabels(nameof(IMathService.Compute), status)
                                .Observe(sw.Elapsed.TotalSeconds);
            }
        }

        public (double[] t, double[] y) SolveExponentialGrowthDecay_RK4(double a, double y0, double t0, double t1, int n)
        {
            var sw = Stopwatch.StartNew();
            var status = "ok";
            Console.WriteLine(status);

            try
            {
                return _inner.SolveExponentialGrowthDecay_RK4(a, y0, t0, t1, n);
            }
            catch
            {
                status = "error";
                throw;
            }
            finally
            {
                sw.Stop();
                MathMetrics.ComputeDurationSeconds
                                .WithLabels(nameof(IMathService.SolveExponentialGrowthDecay_RK4), status)
                                .Observe(sw.Elapsed.TotalSeconds);
            }
        }

        public (double[] t, double[] theta, double[] omega) SolveNonlinearPendulum_RK4(double g, double length, double theta0, double omega0, double t0, double t1, int n)
        {
            var sw = Stopwatch.StartNew();
            var status = "ok";
            Console.WriteLine(status);

            try
            {
                return _inner.SolveNonlinearPendulum_RK4(g, length, theta0, omega0, t0, t1, n);
            }
            catch
            {
                status = "error";
                throw;
            }
            finally
            {
                sw.Stop();
                MathMetrics.ComputeDurationSeconds
                                .WithLabels(nameof(IMathService.SolveNonlinearPendulum_RK4), status)
                                .Observe(sw.Elapsed.TotalSeconds);
            }
        }

        public (double[] t, double[] x, double[] v) SolveSimpleHarmonicOscillator_RK4(double omega, double x0, double v0, double t0, double t1, int n)
        {
            var sw = Stopwatch.StartNew();
            var status = "ok";
            Console.WriteLine(status);

            try
            {
                return _inner.SolveSimpleHarmonicOscillator_RK4(omega, x0, v0, t0, t1, n);
            }
            catch
            {
                status = "error";
                throw;
            }
            finally
            {
                sw.Stop();
                MathMetrics.ComputeDurationSeconds
                                .WithLabels(nameof(IMathService.SolveSimpleHarmonicOscillator_RK4), status)
                                .Observe(sw.Elapsed.TotalSeconds);
            }
        }
    }
}
