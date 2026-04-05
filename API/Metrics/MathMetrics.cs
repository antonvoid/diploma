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

        public static readonly Prometheus.Histogram OdeRmsError =
        Prometheus.Metrics.CreateHistogram(
            "math_ode_rms_error",
            "RMS error between numerical and analytic solution (same time grid).",
            new Prometheus.HistogramConfiguration
            {
                LabelNames = new[] { "operation", "component", "status" },
                Buckets = Prometheus.Histogram.ExponentialBuckets(start: 1e-12, factor: 2, count: 30)
            });

        public static readonly Prometheus.Gauge OdeRmsErrorLast =
        Prometheus.Metrics.CreateGauge(
        "math_ode_rms_error_last",
        "Last observed RMS error (point-in-time).",
        new Prometheus.GaugeConfiguration
        {
            LabelNames = new[] { "operation", "component", "status" }
        });

        public static readonly Prometheus.Gauge ComputeDurationSecondsLast =
        Prometheus.Metrics.CreateGauge(
        "math_compute_duration_seconds_last",
        "Time spent in math computations (seconds).",
        new Prometheus.GaugeConfiguration
        {
            LabelNames = new[] { "operation", "status" }
        });

    }
}
