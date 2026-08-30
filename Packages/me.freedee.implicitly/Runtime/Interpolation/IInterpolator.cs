namespace Implicitly
{
    public interface IInterpolator { }

    public interface IInterpolator<T> : IInterpolator
    {
        public T Lerp(T a, T b, float t);
    }
}
