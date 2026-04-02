using Prometheus;

namespace API.Metrics
{
    public static class MathMetrics
    {
        public static readonly Prometheus.Histogram ComputeDurationSeconds =
        Prometheus.Metrics.CreateHistogram(
        "math_compute_duration_seconds",
        "Time spent in math computations (seconds).",
        new Prometheus.HistogramConfiguration
        {
            LabelNames = new[] { "operation", "status" }
        });
    }
}
