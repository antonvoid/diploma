using Math.Interfaces;
using System.Diagnostics;
using Math.Helper;
namespace API.Metrics
{
    public class MeasuredMathService : IMathService
    {
        private readonly IMathService _inner;

        public MeasuredMathService(IMathService inner) => _inner = inner;

        public (double[] t, double[] y) SolveExponentialGrowthDecay_RK4(double a, double y0, double t0, double t1, int n)
        {
            var operation = nameof(IMathService.SolveExponentialGrowthDecay_RK4);

            return ObserveDurationThenRms(
                operation,
                call: () => _inner.SolveExponentialGrowthDecay_RK4(a, y0, t0, t1, n),
                rmsFactory: result =>
                {
                    var (t, yNum) = result;

                    var yAn = OdeQuality.Evaluate(t, AnalyticSolve.AnalyticExponential(a, y0, t0));
                    var rms = OdeQuality.Rms(yNum, yAn);

                    return new[] { (component: "y", rms) };
                });
        }

        public (double[] t, double[] theta, double[] omega) SolveNonlinearPendulum_RK4(double g, double length, double theta0, double omega0, double t0, double t1, int n)
        {
            var operation = nameof(IMathService.SolveNonlinearPendulum_RK4);

            return ObserveDurationThenRms(
                operation,
                call: () => _inner.SolveNonlinearPendulum_RK4(g, length, theta0, omega0, t0, t1, n),
                rmsFactory: result =>
                {
                    var (t, thetaNum, omegaNum) = result;

                    // RMS считаем к аналитике ЛИНЕАРИЗОВАННОГО маятника
                    var (thetaA, omegaA) = AnalyticSolve.AnalyticLinearPendulum(g, length, theta0, omega0, t0);

                    var thetaAn = OdeQuality.Evaluate(t, thetaA);
                    var omegaAn = OdeQuality.Evaluate(t, omegaA);

                    return new (string component, double rms)[]
                    {
                    ("theta", OdeQuality.Rms(thetaNum, thetaAn)),
                    ("omega", OdeQuality.Rms(omegaNum, omegaAn)),
                    };
                });
        }

        public (double[] t, double[] x, double[] v) SolveSimpleHarmonicOscillator_RK4(double omega, double x0, double v0, double t0, double t1, int n)
        {
            var operation = nameof(IMathService.SolveSimpleHarmonicOscillator_RK4);

            return ObserveDurationThenRms(
                operation,
                call: () => _inner.SolveSimpleHarmonicOscillator_RK4(omega, x0, v0, t0, t1, n),
                rmsFactory: result =>
                {
                    var (t, xNum, vNum) = result;

                    var (xA, vA) = AnalyticSolve.AnalyticSho(omega, x0, v0, t0);
                    var xAn = OdeQuality.Evaluate(t, xA);
                    var vAn = OdeQuality.Evaluate(t, vA);

                    return new[]
                    {
                        (component: "x", rms: OdeQuality.Rms(xNum, xAn)),
                        (component: "v", rms: OdeQuality.Rms(vNum, vAn)),
                    };
                });
        }

        private T ObserveDurationThenRms<T>(
        string operation,
        Func<T> call,
        Func<T, IEnumerable<(string component, double rms)>> rmsFactory)
        {
            var sw = Stopwatch.StartNew();
            var status = "ok";

            T result = default!;
            var hasResult = false;

            try
            {
                result = call();
                hasResult = true;
                return result;
            }
            catch
            {
                status = "error";
                throw;
            }
            finally
            {
                sw.Stop();

                MathMetrics.ComputeDurationSecondsLast
                    .WithLabels(operation, status)
                    .Set(sw.Elapsed.TotalSeconds);
                MathMetrics.ComputeDurationSeconds
                    .WithLabels(operation, status)
                    .Observe(sw.Elapsed.TotalSeconds);

                if (hasResult)
                {
                    ObserveRms(operation, rmsFactory, result, status);
                }
            }
        }
        private void ObserveRms<T>(string operation, 
            Func<T, IEnumerable<(string component, double rms)>> rmsFactory,
            T result,
            string status)
        {
            try
            {
                foreach (var (component, rms) in rmsFactory(result))
                {
                    MathMetrics.OdeRmsErrorLast
                        .WithLabels(operation, component, status)
                        .Set(rms);
                    MathMetrics.OdeRmsError
                        .WithLabels(operation, component, status)
                        .Observe(rms);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
