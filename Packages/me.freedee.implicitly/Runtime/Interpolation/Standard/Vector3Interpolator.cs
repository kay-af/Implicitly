using UnityEngine;

namespace Implicitly
{
    public sealed class Vector3Interpolator : IInterpolator<Vector3>
    {
        public Vector3 LerpUnclamped(Vector3 a, Vector3 b, float t) =>
            Vector3.LerpUnclamped(a, b, t);
    }
}
