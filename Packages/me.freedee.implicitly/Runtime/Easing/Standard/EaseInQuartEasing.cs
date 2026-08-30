namespace Implicitly
{
    public sealed class EaseInQuartEasing : IEasing
    {
        public float Ease(float t) => t * t * t * t;
    }
}
