namespace Implicitly
{
    public sealed class EaseInOutQuartEasing : IEasing
    {
        public float Ease(float t)
        {
            if (t < 0.5f)
            {
                return 8f * t * t * t * t;
            }

            var f = -2f * t + 2f;
            return 1f - f * f * f * f * 0.5f;
        }
    }
}
