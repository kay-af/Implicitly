using UnityEngine;

namespace Implicitly
{
    public sealed class EaseOutExpoEasing : IEasing
    {
        public float Ease(float t) =>
            Mathf.Approximately(t, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * t);
    }
}
