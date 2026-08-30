using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Implicitly
{
    [Serializable]
    public class Animated<T> : IDisposable
    {
        [SerializeField]
        private T m_currentValue = default;
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

                NotifyCurrentValueChanged();

                AnimateDelta();
            }
        }

        [SerializeField]
        private T m_targetValue = default;
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

                AnimateDelta();
            }
        }

        [field: SerializeField]
        public EasingMode EasingMode { get; private set; } = EasingMode.Standard;

        [field: SerializeField]
        public StandardEasingType StandardEasingType { get; private set; } =
            StandardEasingType.Linear;

        [field: SerializeField]
        public CustomEasing CustomEasing { get; private set; } = new CustomEasing();

        [field: Min(0f)]
        [field: SerializeField]
        public float Delay { get; private set; } = 0f;

        [field: Min(0f)]
        [field: SerializeField]
        public float Duration { get; private set; } = 0.5f;

        [field: SerializeField]
        public bool PreserveDuration { get; private set; } = true;

        [field: SerializeField]
        public bool UseUnscaledTime { get; private set; } = false;

        [SerializeField]
        private UnityEvent<T> m_onCurrentValueChanged;

        public IInterpolator<T> CustomInterpolator { get; private set; } = null;

        private bool m_isInitialized;
        private Coroutine m_activeRoutine;
        private bool m_isDisposed;

        private IEasing EffectiveEasing =>
            EasingMode switch
            {
                EasingMode.Standard => StandardEasingRegistry.Get(StandardEasingType),
                EasingMode.Custom => CustomEasing,
                _ => throw new NotImplementedException(),
            };

        private IInterpolator<T> EffectiveInterpolator =>
            CustomInterpolator ?? InterpolatorRegistry.Get<T>();

        private IEqualityComparer<T> EffectiveComparer =>
            EqualityComparerRegistry.TryGet<T>(out var comparer)
                ? comparer
                : EqualityComparer<T>.Default;

        public bool IsAnimating => m_activeRoutine != null;

        public void Initialize()
        {
            ThrowIfDisposed();

            if (m_isInitialized)
            {
                return;
            }

            m_isInitialized = true;

            AnimateDelta();
        }

        public void SetEasingMode(
            EasingMode easingMode,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (EasingMode == easingMode)
            {
                return;
            }

            EasingMode = easingMode;

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetStandardEasingType(
            StandardEasingType standardEasingType,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (StandardEasingType == standardEasingType)
            {
                return;
            }

            StandardEasingType = standardEasingType;

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetCustomEasing(
            CustomEasing customEasing,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (CustomEasing == customEasing)
            {
                return;
            }

            CustomEasing = customEasing;

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetDelay(
            float delay,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (Delay == delay)
            {
                return;
            }

            Delay = Mathf.Max(0f, delay);

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetDuration(
            float duration,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (Duration == duration)
            {
                return;
            }

            Duration = Mathf.Max(0f, duration);

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetPreserveDuration(
            bool preserveDuration,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (PreserveDuration == preserveDuration)
            {
                return;
            }

            PreserveDuration = preserveDuration;

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetUseUnscaledTime(
            bool useUnscaledTime,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (UseUnscaledTime == useUnscaledTime)
            {
                return;
            }

            UseUnscaledTime = useUnscaledTime;

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void SetCustomInterpolator(
            IInterpolator<T> customInterpolator,
            UpdateParameterMode updateParameterMode = UpdateParameterMode.NonIntrusive
        )
        {
            ThrowIfDisposed();

            if (CustomInterpolator == customInterpolator)
            {
                return;
            }

            CustomInterpolator = customInterpolator;

            if (updateParameterMode == UpdateParameterMode.Intrusive)
            {
                AnimateDelta();
            }
        }

        public void AddCurrentValueChangedListener(UnityAction<T> listener)
        {
            ThrowIfDisposed();

            m_onCurrentValueChanged.AddListener(listener);
        }

        public void RemoveCurrentValueChangedListener(UnityAction<T> listener)
        {
            ThrowIfDisposed();

            m_onCurrentValueChanged.RemoveListener(listener);
        }

        private void AnimateDelta()
        {
            if (!m_isInitialized)
            {
                return;
            }

            if (EffectiveComparer.Equals(m_currentValue, m_targetValue))
            {
                return;
            }

            StopActiveRoutine();

            m_activeRoutine = CoroutineRunner.Run(CoAnimateDelta());
        }

        private IEnumerator CoAnimateDelta()
        {
            NotifyCurrentValueChanged();

            if (Delay > 0f)
            {
                yield return YieldInstructionCache.WaitForSeconds(Delay);
            }

            var elapsed = 0f;
            while (elapsed < Duration)
            {
                elapsed += UseUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                var t = elapsed / Duration;

                m_currentValue = EffectiveInterpolator.Lerp(
                    m_currentValue,
                    m_targetValue,
                    EffectiveEasing.Ease(t)
                );

                NotifyCurrentValueChanged();

                yield return null;
            }

            m_currentValue = m_targetValue;

            NotifyCurrentValueChanged();

            m_activeRoutine = null;
        }

        private void NotifyCurrentValueChanged()
        {
            try
            {
                m_onCurrentValueChanged.Invoke(m_currentValue);
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"An exception occurred while invoking current value changed event: {ex}"
                );
            }
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

            throw new ObjectDisposedException(nameof(Animated<T>));
        }

        public void Dispose()
        {
            if (m_isDisposed)
            {
                return;
            }

            StopActiveRoutine();
            m_onCurrentValueChanged.RemoveAllListeners();
            m_isDisposed = true;
        }
    }
}
