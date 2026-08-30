namespace Implicitly
{
    public sealed class EaseInOutBackEasing : IEasing
    {
        private const float C1 = 1.70158f;
        private const float C2 = C1 * 1.525f;

        public float Ease(float t)
        {
            if (t < 0.5f)
            {
                var f = 2f * t;
                return f * f * ((C2 + 1f) * f - C2) * 0.5f;
            }

            var g = 2f * t - 2f;
            return (g * g * ((C2 + 1f) * g + C2) + 2f) * 0.5f;
        }
    }
}
