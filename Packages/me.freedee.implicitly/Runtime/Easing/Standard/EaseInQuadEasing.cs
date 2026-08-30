namespace Implicitly
{
    public sealed class EaseInQuadEasing : IEasing
    {
        public float Ease(float t) => t * t;
    }
}
