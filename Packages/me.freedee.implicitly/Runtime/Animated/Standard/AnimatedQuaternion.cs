using UnityEngine;

namespace Implicitly
{
    [AddComponentMenu("Implicitly/Animated Quaternion")]
    public class AnimatedQuaternion : Animated<Quaternion>
    {
        private static readonly IInterpolator<Quaternion> s_lerpInterpolator =
            new QuaternionLerpInterpolator();

        [SerializeField]
        private bool _useLerpInterpolator = false;
        public bool UseLerp
        {
            get => _useLerpInterpolator;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (_useLerpInterpolator == value)
                {
                    return;
                }

                _useLerpInterpolator = value;

                SetupInterpolator();

                AnimateDifferenceInternal();
            }
        }

        protected virtual void OnValidate()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (CheckDestroyed())
            {
                return;
            }

            SetupInterpolator();
        }

        protected virtual void Awake() => SetupInterpolator();

        private void SetupInterpolator() =>
            CustomInterpolator = _useLerpInterpolator ? s_lerpInterpolator : null;
    }
}
