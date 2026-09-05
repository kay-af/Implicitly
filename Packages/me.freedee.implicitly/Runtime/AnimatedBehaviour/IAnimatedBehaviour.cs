using UnityEngine.Events;

namespace Implicitly
{
    public interface IAnimatedBehaviour
    {
        public EasingMode EasingMode { get; }
        public StandardEasingType StandardEasingType { get; }
        public CustomEasing CustomEasing { get; }
        public float Delay { get; }
        public float Duration { get; }
        public bool PreserveDuration { get; }
        public bool UseUnscaledTime { get; }
        public bool IsInitialized { get; }
        public bool HasDifference { get; }
        public bool IsAnimating { get; }
        public void Initialize();
        public void AnimateDifference();
        public void AddAnimationStartListener(UnityAction listener);
        public void RemoveAnimationStartListener(UnityAction listener);
        public void AddAnimationCancelListener(UnityAction listener);
        public void RemoveAnimationCancelListener(UnityAction listener);
        public void AddAnimationEndListener(UnityAction listener);
        public void RemoveAnimationEndListener(UnityAction listener);
    }

    public interface IAnimatedBehaviour<T> : IAnimatedBehaviour
        where T : struct
    {
        public T CurrentValue { get; }
        public T TargetValue { get; }
        public IInterpolator<T> CustomInterpolator { get; }
        public void AddCurrentValueChangeListener(UnityAction<T> listener);
        public void RemoveCurrentValueChangeListener(UnityAction<T> listener);
    }
}
