namespace Implicitly
{
    public sealed class EaseInOutQuadEasing : IEasing
    {
        public float Ease(float t)
        {
            if (t < 0.5f)
            {
                return 2f * t * t;
            }

            var f = -2f * t + 2f;
            return 1f - f * f * 0.5f;
        }
    }
}
