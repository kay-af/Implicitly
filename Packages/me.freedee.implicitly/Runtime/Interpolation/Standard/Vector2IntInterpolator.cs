using UnityEngine;

namespace Implicitly
{
    public sealed class Vector2IntInterpolator : IInterpolator<Vector2Int>
    {
        public Vector2Int LerpUnclamped(Vector2Int a, Vector2Int b, float t) =>
            new(
                Mathf.RoundToInt(Mathf.LerpUnclamped(a.x, b.x, t)),
                Mathf.RoundToInt(Mathf.LerpUnclamped(a.y, b.y, t))
            );
    }
}
