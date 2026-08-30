using UnityEngine;

namespace Implicitly
{
    public sealed class SineInEasing : IEasing
    {
        public float Ease(float t) => Mathf.Sin(t * Mathf.PI * 0.5f);
    }
}
