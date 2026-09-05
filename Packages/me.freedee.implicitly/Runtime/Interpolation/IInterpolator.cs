namespace Implicitly
{
    public interface IInterpolator { }

    public interface IInterpolator<T> : IInterpolator
        where T : struct
    {
        public T Lerp(T a, T b, float t);
    }
}
