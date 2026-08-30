using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInExpoEasing : IEasing
    {
        public float Ease(float t) =>
            Mathf.Approximately(t, 0f) ? 0f : Mathf.Pow(2f, 10f * t - 10f);
    }
}
