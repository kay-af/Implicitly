namespace Implicitly
{
    public sealed class EaseInCubicEasing : IEasing
    {
        public float Ease(float t) => t * t * t;
    }
}
