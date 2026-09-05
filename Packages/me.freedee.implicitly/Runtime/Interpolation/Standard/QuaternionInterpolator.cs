using UnityEngine;

namespace Implicitly
{
    public sealed class QuaternionInterpolator : IInterpolator<Quaternion>
    {
        public Quaternion LerpUnclamped(Quaternion a, Quaternion b, float t) =>
            Quaternion.SlerpUnclamped(a, b, t);
    }
}
