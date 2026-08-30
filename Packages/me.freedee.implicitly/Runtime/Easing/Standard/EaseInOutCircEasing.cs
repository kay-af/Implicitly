using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInOutCircEasing : IEasing
    {
        public float Ease(float t)
        {
            if (t < 0.5f)
            {
                var f = 2f * t;
                return (1f - Mathf.Sqrt(1f - f * f)) * 0.5f;
            }

            var g = -2f * t + 2f;
            return (Mathf.Sqrt(1f - g * g) + 1f) * 0.5f;
        }
    }
}
