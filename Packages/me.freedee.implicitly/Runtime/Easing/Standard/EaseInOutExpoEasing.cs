using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInOutExpoEasing : IEasing
    {
        public float Ease(float t)
        {
            if (Mathf.Approximately(t, 0f))
            {
                return 0f;
            }

            if (Mathf.Approximately(t, 1f))
            {
                return 1f;
            }

            if (t < 0.5f)
            {
                return Mathf.Pow(2f, 20f * t - 10f) * 0.5f;
            }

            return (2f - Mathf.Pow(2f, -20f * t + 10f)) * 0.5f;
        }
    }
}
