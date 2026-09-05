using UnityEngine;

namespace Implicitly
{
    public sealed class Vector2Interpolator : IInterpolator<Vector2>
    {
        public Vector2 LerpUnclamped(Vector2 a, Vector2 b, float t) =>
            Vector2.LerpUnclamped(a, b, t);
    }
}
