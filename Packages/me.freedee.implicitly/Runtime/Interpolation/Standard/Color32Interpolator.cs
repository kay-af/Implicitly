using UnityEngine;

namespace Implicitly
{
    public sealed class Color32Interpolator : IInterpolator<Color32>
    {
        public Color32 LerpUnclamped(Color32 a, Color32 b, float t) =>
            Color32.LerpUnclamped(a, b, t);
    }
}
