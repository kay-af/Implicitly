namespace Implicitly
{
    public sealed class EaseOutQuadEasing : IEasing
    {
        public float Ease(float t) => 1f - (1f - t) * (1f - t);
    }
}
