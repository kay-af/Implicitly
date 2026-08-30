namespace Implicitly
{
    public sealed class EaseInOutBounceEasing : IEasing
    {
        public float Ease(float t)
        {
            if (t < 0.5f)
            {
                return (1f - EaseOutBounceEasing.Evaluate(1f - 2f * t)) * 0.5f;
            }

            return (1f + EaseOutBounceEasing.Evaluate(2f * t - 1f)) * 0.5f;
        }
    }
}
