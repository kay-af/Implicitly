namespace Implicitly
{
    public sealed class EaseOutBounceEasing : IEasing
    {
        private const float N1 = 7.5625f;
        private const float D1 = 2.75f;

        public float Ease(float t) => Evaluate(t);

        public static float Evaluate(float t)
        {
            if (t < 1f / D1)
            {
                return N1 * t * t;
            }

            if (t < 2f / D1)
            {
                t -= 1.5f / D1;
                return N1 * t * t + 0.75f;
            }

            if (t < 2.5f / D1)
            {
                t -= 2.25f / D1;
                return N1 * t * t + 0.9375f;
            }

            t -= 2.625f / D1;
            return N1 * t * t + 0.984375f;
        }
    }
}
