using Math.Interfaces;

namespace Math.Service
{
    public class MathService : IMathService
    {
        public double Compute(double x)
        {
            return x * x + 42;
        }
    }
}
