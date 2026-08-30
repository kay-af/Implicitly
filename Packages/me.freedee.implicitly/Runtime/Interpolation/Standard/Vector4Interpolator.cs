using UnityEngine;

namespace Implicitly
{
    public sealed class Vector4Interpolator : IInterpolator<Vector4>
    {
        public Vector4 Lerp(Vector4 a, Vector4 b, float t) => Vector4.LerpUnclamped(a, b, t);
    }
}
