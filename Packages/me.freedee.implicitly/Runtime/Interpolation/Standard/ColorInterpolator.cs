using UnityEngine;

namespace Implicitly
{
    public sealed class ColorInterpolator : IInterpolator<Color>
    {
        public Color Lerp(Color a, Color b, float t) => Color.LerpUnclamped(a, b, t);
    }
}
