namespace Implicitly
{
    public sealed class EaseOutQuartEasing : IEasing
    {
        public float Ease(float t)
        {
            var f = 1f - t;
            return 1f - f * f * f * f;
        }
    }
}
