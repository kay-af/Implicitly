namespace Implicitly
{
    public sealed class EaseInBackEasing : IEasing
    {
        private const float C1 = 1.70158f;
        private const float C3 = C1 + 1f;

        public float Ease(float t) => C3 * t * t * t - C1 * t * t;
    }
}
