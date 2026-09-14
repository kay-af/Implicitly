using UnityEngine;

namespace Implicitly
{
    public sealed class QuaternionLerpInterpolator : IInterpolator<Quaternion>
    {
        public Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t) =>
            Quaternion.LerpUnclamped(a, b, t);
    }
}
