using UnityEngine;

namespace Implicitly
{
    public sealed class IntegerInterpolator : IInterpolator<int>
    {
        public int LerpUnclamped(int a, int b, float t) =>
            Mathf.RoundToInt(Mathf.LerpUnclamped(a, b, t));
    }
}
