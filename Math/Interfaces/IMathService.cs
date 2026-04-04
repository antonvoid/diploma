using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math.Interfaces
{
    public interface IMathService
    {
        double Compute(double x);

        (double[] t, double[] y) SolveExponentialGrowthDecay_RK4(
        double a,
        double y0,
        double t0,
        double t1,
        int n);

        public (double[] t, double[] x, double[] v) SolveSimpleHarmonicOscillator_RK4(
            double omega,
            double x0,
            double v0,
            double t0,
            double t1,
            int n);

        public (double[] t, double[] theta, double[] omega) SolveNonlinearPendulum_RK4(
            double g,
            double length,
            double theta0,
            double omega0,
            double t0,
            double t1,
            int n);
    }
}
