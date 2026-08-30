using UnityEngine;

namespace Implicitly
{
    public sealed class FloatInterpolator : IInterpolator<float>
    {
        public float Lerp(float a, float b, float t) => Mathf.LerpUnclamped(a, b, t);
    }
}
