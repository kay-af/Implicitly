using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInSineEasing : IEasing
    {
        public float Ease(float t) => 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
    }
}
