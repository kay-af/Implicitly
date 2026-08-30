using UnityEngine;

namespace Implicitly
{
    public sealed class EaseInOutSineEasing : IEasing
    {
        public float Ease(float t) => -(Mathf.Cos(Mathf.PI * t) - 1f) * 0.5f;
    }
}
