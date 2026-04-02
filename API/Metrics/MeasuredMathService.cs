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
    }
}
