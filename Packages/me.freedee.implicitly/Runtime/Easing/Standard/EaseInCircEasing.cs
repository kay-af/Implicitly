using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInCircEasing : IEasing
    {
        public float Ease(float t) => 1f - Mathf.Sqrt(1f - t * t);
    }
}
