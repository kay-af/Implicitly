using UnityEngine;

namespace Implicitly
{
    public sealed class RectInterpolator : IInterpolator<Rect>
    {
        public Rect Lerp(Rect a, Rect b, float t) =>
            new(
                Mathf.LerpUnclamped(a.x, b.x, t),
                Mathf.LerpUnclamped(a.y, b.y, t),
                Mathf.LerpUnclamped(a.width, b.width, t),
                Mathf.LerpUnclamped(a.height, b.height, t)
            );
    }
}
