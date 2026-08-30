namespace Implicitly
{
    public sealed class EaseOutEasing : IEasing
    {
        public float Ease(float t) => 1f - (1f - t) * (1f - t);
    }
}
