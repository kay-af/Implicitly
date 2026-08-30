namespace Implicitly
{
    public sealed class EaseInEasing : IEasing
    {
        public float Ease(float t) => t * t;
    }
}
