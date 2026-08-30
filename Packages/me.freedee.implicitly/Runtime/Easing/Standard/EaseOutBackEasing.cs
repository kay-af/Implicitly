namespace Implicitly
{
    public sealed class EaseOutBackEasing : IEasing
    {
        private const float C1 = 1.70158f;
        private const float C3 = C1 + 1f;

        public float Ease(float t)
        {
            var f = t - 1f;
            return 1f + C3 * f * f * f + C1 * f * f;
        }
    }
}
