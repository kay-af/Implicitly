namespace Implicitly
{
    public sealed class EaseInQuintEasing : IEasing
    {
        public float Ease(float t) => t * t * t * t * t;
    }
}
