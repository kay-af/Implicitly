using UnityEngine;

namespace Implicitly
{
    public sealed class EaseOutElasticEasing : IEasing
    {
        private const float C4 = 2f * Mathf.PI / 3f;

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

            return Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * C4) + 1f;
        }
    }
}
