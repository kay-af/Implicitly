namespace Implicitly
{
    public sealed class EaseInBounceEasing : IEasing
    {
        public float Ease(float t) => 1f - EaseOutBounceEasing.Evaluate(1f - t);
    }
}
