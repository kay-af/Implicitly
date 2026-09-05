namespace Implicitly
{
    public sealed class DoubleInterpolator : IInterpolator<double>
    {
        public double LerpUnclamped(double a, double b, float t) => a + (b - a) * t;
    }
}
