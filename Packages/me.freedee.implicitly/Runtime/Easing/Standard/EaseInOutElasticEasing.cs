using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInOutElasticEasing : IEasing
    {
        private const float C5 = 2f * Mathf.PI / 4.5f;

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
                return -(Mathf.Pow(2f, 20f * t - 10f) * Mathf.Sin((20f * t - 11.125f) * C5)) * 0.5f;
            }

            return Mathf.Pow(2f, -20f * t + 10f) * Mathf.Sin((20f * t - 11.125f) * C5) * 0.5f + 1f;
        }
    }
}
