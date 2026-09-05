using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Implicitly
{
    public abstract class AnimatedBehaviour<T> : MonoBehaviour, IAnimatedBehaviour<T>
        where T : struct
    {
        [SerializeField]
        private bool m_autoInitialize = true;

        [SerializeField]
        private T m_currentValue = default;
        public T CurrentValue
        {
            get => m_currentValue;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (EffectiveComparer.Equals(m_currentValue, value))
                {
                    return;
                }

                m_currentValue = value;

                NotifyCurrentValueChange();

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private T m_targetValue = default;
        public T TargetValue
        {
            get => m_targetValue;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (EffectiveComparer.Equals(m_targetValue, value))
                {
                    return;
                }

                m_targetValue = value;

                AnimateDifferenceInternal();
            }
        }

        private IInterpolator<T> m_customInterpolator = null;
        public IInterpolator<T> CustomInterpolator
        {
            get => m_customInterpolator;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_customInterpolator == value)
                {
                    return;
                }

                m_customInterpolator = value;

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private EasingMode m_easingMode = EasingMode.Standard;
        public EasingMode EasingMode
        {
            get => m_easingMode;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_easingMode == value)
                {
                    return;
                }

                m_easingMode = value;

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private StandardEasingType m_standardEasingType;
        public StandardEasingType StandardEasingType
        {
            get => m_standardEasingType;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_standardEasingType == value)
                {
                    return;
                }

                m_standardEasingType = value;

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private CustomEasing m_customEasing = new();
        public CustomEasing CustomEasing
        {
            get => m_customEasing;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_customEasing == value)
                {
                    return;
                }

                m_customEasing = value;

                AnimateDifferenceInternal();
            }
        }

        [Min(0f)]
        [SerializeField]
        private float m_delay = 0f;
        public float Delay
        {
            get => m_delay;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_delay == value)
                {
                    return;
                }

                m_delay = Mathf.Max(0f, value);

                AnimateDifferenceInternal();
            }
        }

        [Min(0f)]
        [SerializeField]
        private float m_duration = 1f;
        public float Duration
        {
            get => m_duration;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_duration == value)
                {
                    return;
                }

                m_duration = Mathf.Max(0f, value);

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private bool m_preserveDuration = true;
        public bool PreserveDuration
        {
            get => m_preserveDuration;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_preserveDuration == value)
                {
                    return;
                }

                m_preserveDuration = value;

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private bool m_useUnscaledTime = false;
        public bool UseUnscaledTime
        {
            get => m_useUnscaledTime;
            set
            {
                if (CheckDestroyed())
                {
                    return;
                }

                if (m_useUnscaledTime == value)
                {
                    return;
                }

                m_useUnscaledTime = value;

                AnimateDifferenceInternal();
            }
        }

        [SerializeField]
        private UnityEvent<T> m_onCurrentValueChange;

        [SerializeField]
        private UnityEvent m_onAnimationStart;

        [SerializeField]
        private UnityEvent m_onAnimationCancel;

        [SerializeField]
        private UnityEvent m_onAnimationEnd;

        private bool m_isInitialized = false;
        private float m_activeElapsed = 0f;
        private Coroutine m_activeRoutine = null;

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

        public bool IsInitialized
        {
            get
            {
                if (CheckDestroyed())
                {
                    return false;
                }

                return m_isInitialized;
            }
        }

        public bool HasDifference
        {
            get
            {
                if (CheckDestroyed())
                {
                    return false;
                }

                return !EffectiveComparer.Equals(m_currentValue, m_targetValue);
            }
        }

        public bool IsAnimating
        {
            get
            {
                if (CheckDestroyed())
                {
                    return false;
                }

                return m_activeRoutine != null;
            }
        }

        protected virtual void Awake()
        {
            if (m_autoInitialize)
            {
                Initialize();
            }
        }

        protected virtual void OnEnable() => AnimateDifferenceInternal();

        protected virtual void OnDisable() => StopActiveRoutine();

        protected virtual void OnDestroy()
        {
            StopActiveRoutine();
            m_onCurrentValueChange.RemoveAllListeners();
        }

        public void Initialize()
        {
            if (CheckDestroyed())
            {
                return;
            }

            if (m_isInitialized)
            {
                return;
            }

            m_isInitialized = true;

            AnimateDifferenceInternal();
        }

        public void AddCurrentValueChangeListener(UnityAction<T> listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onCurrentValueChange.AddListener(listener);
        }

        public void RemoveCurrentValueChangeListener(UnityAction<T> listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onCurrentValueChange.RemoveListener(listener);
        }

        public void AddAnimationStartListener(UnityAction listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onAnimationStart.AddListener(listener);
        }

        public void RemoveAnimationStartListener(UnityAction listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onAnimationStart.RemoveListener(listener);
        }

        public void AddAnimationCancelListener(UnityAction listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onAnimationCancel.AddListener(listener);
        }

        public void RemoveAnimationCancelListener(UnityAction listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onAnimationCancel.RemoveListener(listener);
        }

        public void AddAnimationEndListener(UnityAction listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onAnimationEnd.AddListener(listener);
        }

        public void RemoveAnimationEndListener(UnityAction listener)
        {
            if (CheckDestroyed())
            {
                return;
            }

            m_onAnimationEnd.RemoveListener(listener);
        }

        public void AnimateDifference()
        {
            if (CheckDestroyed())
            {
                return;
            }

            if (!m_isInitialized)
            {
                Debug.LogWarning(
                    "Cannot animate difference because the animated behaviour is not initialized!",
                    this
                );

                return;
            }

            AnimateDifferenceInternal();
        }

        private void AnimateDifferenceInternal()
        {
            if (!m_isInitialized)
            {
                return;
            }

            if (!HasDifference)
            {
                return;
            }

            StopActiveRoutine();

            m_activeRoutine = StartCoroutine(CoAnimateDifference());
        }

        private IEnumerator CoAnimateDifference()
        {
            var startValue = m_currentValue;
            var endValue = m_targetValue;

            var easing = EffectiveEasing;
            var interpolator = EffectiveInterpolator;

            var duration = Duration;
            var delay = Delay;
            var preserveDuration = PreserveDuration;
            var useUnscaledTime = UseUnscaledTime;

            NotifyAnimationStart();

            NotifyCurrentValueChange();

            if (delay > 0f)
            {
                yield return YieldInstructionCache.WaitForSeconds(delay);
            }

            NotifyCurrentValueChange();

            if (!preserveDuration)
            {
                m_activeElapsed = 0f;
            }

            while (m_activeElapsed < duration)
            {
                m_activeElapsed += useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

                var t = m_activeElapsed / duration;

                m_currentValue = interpolator.Lerp(startValue, endValue, easing.Ease(t));

                NotifyCurrentValueChange();

                yield return null;
            }

            m_activeElapsed = 0f;

            m_currentValue = endValue;

            NotifyCurrentValueChange();

            m_activeRoutine = null;

            NotifyAnimationEnd();
        }

        private void NotifyCurrentValueChange()
        {
            try
            {
                m_onCurrentValueChange.Invoke(m_currentValue);
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"An exception occurred while invoking current value changed event: {ex}",
                    this
                );
            }
        }

        private void NotifyAnimationStart()
        {
            try
            {
                m_onAnimationStart.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"An exception occurred while invoking animation start event: {ex}",
                    this
                );
            }
        }

        private void NotifyAnimationCancel()
        {
            try
            {
                m_onAnimationCancel.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"An exception occurred while invoking animation cancel event: {ex}",
                    this
                );
            }
        }

        private void NotifyAnimationEnd()
        {
            try
            {
                m_onAnimationEnd.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError(
                    $"An exception occurred while invoking animation end event: {ex}",
                    this
                );
            }
        }

        private void StopActiveRoutine()
        {
            if (m_activeRoutine == null)
            {
                return;
            }

            StopCoroutine(m_activeRoutine);

            m_activeRoutine = null;

            NotifyAnimationCancel();
        }

        private bool CheckDestroyed()
        {
            if (this == null)
            {
                Debug.LogWarning("Cannot access animated behaviour because it is destroyed!", this);
                return true;
            }

            return false;
        }
    }
}
