using System.Collections.Generic;

namespace Implicitly
{
    public static class StandardEasingRegistry
    {
        private static readonly IReadOnlyDictionary<StandardEasingType, IEasing> s_easings =
            new Dictionary<StandardEasingType, IEasing>
            {
                { StandardEasingType.Linear, new LinearEasing() },
                { StandardEasingType.EaseIn, new EaseInEasing() },
                { StandardEasingType.EaseOut, new EaseOutEasing() },
                { StandardEasingType.EaseInOut, new EaseInOutEasing() },
                { StandardEasingType.EaseInQuad, new EaseInQuadEasing() },
                { StandardEasingType.EaseOutQuad, new EaseOutQuadEasing() },
                { StandardEasingType.EaseInOutQuad, new EaseInOutQuadEasing() },
                { StandardEasingType.EaseInCubic, new EaseInCubicEasing() },
                { StandardEasingType.EaseOutCubic, new EaseOutCubicEasing() },
                { StandardEasingType.EaseInOutCubic, new EaseInOutCubicEasing() },
                { StandardEasingType.EaseInQuart, new EaseInQuartEasing() },
                { StandardEasingType.EaseOutQuart, new EaseOutQuartEasing() },
                { StandardEasingType.EaseInOutQuart, new EaseInOutQuartEasing() },
                { StandardEasingType.EaseInQuint, new EaseInQuintEasing() },
                { StandardEasingType.EaseOutQuint, new EaseOutQuintEasing() },
                { StandardEasingType.EaseInOutQuint, new EaseInOutQuintEasing() },
                { StandardEasingType.EaseInSine, new EaseInSineEasing() },
                { StandardEasingType.EaseOutSine, new EaseOutSineEasing() },
                { StandardEasingType.EaseInOutSine, new EaseInOutSineEasing() },
                { StandardEasingType.EaseInExpo, new EaseInExpoEasing() },
                { StandardEasingType.EaseOutExpo, new EaseOutExpoEasing() },
                { StandardEasingType.EaseInOutExpo, new EaseInOutExpoEasing() },
                { StandardEasingType.EaseInCirc, new EaseInCircEasing() },
                { StandardEasingType.EaseOutCirc, new EaseOutCircEasing() },
                { StandardEasingType.EaseInOutCirc, new EaseInOutCircEasing() },
                { StandardEasingType.EaseInElastic, new EaseInElasticEasing() },
                { StandardEasingType.EaseOutElastic, new EaseOutElasticEasing() },
                { StandardEasingType.EaseInOutElastic, new EaseInOutElasticEasing() },
                { StandardEasingType.EaseInBack, new EaseInBackEasing() },
                { StandardEasingType.EaseOutBack, new EaseOutBackEasing() },
                { StandardEasingType.EaseInOutBack, new EaseInOutBackEasing() },
                { StandardEasingType.EaseInBounce, new EaseInBounceEasing() },
                { StandardEasingType.EaseOutBounce, new EaseOutBounceEasing() },
                { StandardEasingType.EaseInOutBounce, new EaseInOutBounceEasing() },
            };

        public static IEasing Get(StandardEasingType easingType) => s_easings[easingType];
    }
}
