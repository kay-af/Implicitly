using UnityEngine;

namespace Implicitly
{
    public sealed class EaseOutCircEasing : IEasing
    {
        public float Ease(float t)
        {
            var f = t - 1f;
            return Mathf.Sqrt(1f - f * f);
        }
    }
}
