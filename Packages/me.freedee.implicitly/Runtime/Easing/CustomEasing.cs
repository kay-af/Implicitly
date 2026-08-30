using System;
using UnityEngine;

namespace Implicitly
{
    [Serializable]
    public sealed class CustomEasing : IEasing
    {
        [SerializeField]
        private AnimationCurve m_curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        public AnimationCurve Curve
        {
            get => m_curve;
            private set => m_curve = value ?? m_curve;
        }

        public float Ease(float t) => m_curve.Evaluate(t);
    }
}
