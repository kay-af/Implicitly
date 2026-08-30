using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Implicitly
{
    [Serializable]
    public class AnimatedProperty<T> : IDisposable
    {
        [SerializeField]
        private T m_currentValue;
        public T CurrentValue
        {
            get => m_currentValue;
            set
            {
                ThrowIfDisposed();

                if (EffectiveComparer.Equals(m_currentValue, value))
                {
                    return;
                }

                m_currentValue = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private T m_targetValue;
        public T TargetValue
        {
            get => m_targetValue;
            set
            {
                ThrowIfDisposed();

                if (EffectiveComparer.Equals(m_targetValue, value))
                {
                    return;
                }

                m_targetValue = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private EasingMode m_easingMode;
        public EasingMode EasingMode
        {
            get => m_easingMode;
            private set
            {
                ThrowIfDisposed();

                if (m_easingMode == value)
                {
                    return;
                }

                m_easingMode = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private StandardEasingType m_standardEasingType;
        public StandardEasingType StandardEasingType
        {
            get => m_standardEasingType;
            private set
            {
                ThrowIfDisposed();

                if (m_standardEasingType == value)
                {
                    return;
                }

                m_standardEasingType = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private CustomEasing m_customEasing;
        public CustomEasing CustomEasing
        {
            get => m_customEasing;
            private set
            {
                ThrowIfDisposed();

                if (m_customEasing == value)
                {
                    return;
                }

                m_customEasing = value;

                AnimateDifference();
            }
        }

        [Min(0f)]
        [SerializeField]
        private float m_delay = 0f;
        public float Delay
        {
            get => m_delay;
            private set
            {
                ThrowIfDisposed();

                if (m_delay == value)
                {
                    return;
                }

                m_delay = Mathf.Max(0f, value);

                AnimateDifference();
            }
        }

        [Min(0f)]
        [SerializeField]
        private float m_duration = 0f;
        public float Duration
        {
            get => m_duration;
            private set
            {
                ThrowIfDisposed();

                if (m_duration == value)
                {
                    return;
                }

                m_duration = Mathf.Max(0f, value);

                AnimateDifference();
            }
        }

        [SerializeField]
        private bool m_useUnscaledTime = false;
        public bool UseUnscaledTime
        {
            get => m_useUnscaledTime;
            private set
            {
                ThrowIfDisposed();

                if (m_useUnscaledTime == value)
                {
                    return;
                }

                m_useUnscaledTime = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private bool m_preserveDelay = false;
        public bool PreserveDelay
        {
            get => m_preserveDelay;
            private set
            {
                ThrowIfDisposed();

                if (m_preserveDelay == value)
                {
                    return;
                }

                m_preserveDelay = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private bool m_preserveDuration = false;
        public bool PreserveDuration
        {
            get => m_preserveDuration;
            private set
            {
                ThrowIfDisposed();

                if (m_preserveDuration == value)
                {
                    return;
                }

                m_preserveDuration = value;

                AnimateDifference();
            }
        }

        [SerializeField]
        private UnityEvent<T> m_onValueChanged;

        private IInterpolator<T> m_CustomInterpolator;
        public IInterpolator<T> CustomInterpolator
        {
            get => m_CustomInterpolator;
            private set
            {
                ThrowIfDisposed();

                if (m_CustomInterpolator == value)
                {
                    return;
                }

                m_CustomInterpolator = value;

                AnimateDifference();
            }
        }

        private IEasing EffectiveEasing =>
            m_easingMode switch
            {
                EasingMode.Standard => StandardEasingRegistry.Get(m_standardEasingType),
                EasingMode.Custom => m_customEasing,
                _ => throw new NotImplementedException(),
            };

        private IInterpolator<T> EffectiveInterpolator =>
            m_CustomInterpolator ?? InterpolatorRegistry.Get<T>();

        private IEqualityComparer<T> EffectiveComparer =>
            EqualityComparerRegistry.TryGet<T>(out var comparer)
                ? comparer
                : EqualityComparer<T>.Default;

        private Coroutine m_activeRoutine;
        private bool m_isDisposed;

        public void SetImmediate(T value)
        {
            ThrowIfDisposed();

            m_currentValue = value;
            m_targetValue = value;

            StopActiveRoutine();

            m_onValueChanged.Invoke(value);
        }

        public void AddListener(UnityAction<T> listener)
        {
            ThrowIfDisposed();

            m_onValueChanged.AddListener(listener);
        }

        public void RemoveListener(UnityAction<T> listener)
        {
            ThrowIfDisposed();

            m_onValueChanged.RemoveListener(listener);
        }

        public void AnimateDifference()
        {
            ThrowIfDisposed();

            if (EffectiveComparer.Equals(m_currentValue, m_targetValue))
            {
                return;
            }

            StopActiveRoutine();

            m_activeRoutine = CoroutineRunner.Run(IEAnimateDifference());
        }

        private IEnumerator IEAnimateDifference()
        {
            m_onValueChanged.Invoke(m_currentValue);

            if (m_delay > 0f)
            {
                yield return YieldInstructionCache.WaitForSeconds(m_delay);
            }

            m_onValueChanged.Invoke(m_currentValue);

            var elapsed = 0f;
            while (elapsed < m_duration)
            {
                elapsed += m_useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                var t = elapsed / m_duration;

                m_currentValue = EffectiveInterpolator.Lerp(
                    m_currentValue,
                    m_targetValue,
                    EffectiveEasing.Ease(t)
                );

                m_onValueChanged.Invoke(m_currentValue);

                yield return null;
            }

            m_currentValue = m_targetValue;

            m_onValueChanged.Invoke(m_currentValue);

            m_activeRoutine = null;
        }

        private void StopActiveRoutine()
        {
            if (m_activeRoutine == null)
            {
                return;
            }

            CoroutineRunner.Stop(m_activeRoutine);

            m_activeRoutine = null;
        }

        private void ThrowIfDisposed()
        {
            if (!m_isDisposed)
            {
                return;
            }

            throw new ObjectDisposedException(nameof(AnimatedProperty<T>));
        }

        public void Dispose()
        {
            if (m_isDisposed)
            {
                return;
            }

            StopActiveRoutine();
            m_onValueChanged.RemoveAllListeners();
            m_isDisposed = true;
        }
    }
}
