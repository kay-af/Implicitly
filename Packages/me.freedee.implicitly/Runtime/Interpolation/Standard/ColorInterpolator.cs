using UnityEngine;

namespace Implicitly
{
    public sealed class ColorInterpolator : IInterpolator<Color>
    {
        public Color LerpUnclamped(Color a, Color b, float t) => Color.LerpUnclamped(a, b, t);
    }
}
