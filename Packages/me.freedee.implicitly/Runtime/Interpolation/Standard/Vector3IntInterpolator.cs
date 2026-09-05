using UnityEngine;

namespace Implicitly
{
    public sealed class Vector3IntInterpolator : IInterpolator<Vector3Int>
    {
        public Vector3Int LerpUnclamped(Vector3Int a, Vector3Int b, float t) =>
            new(
                Mathf.RoundToInt(Mathf.LerpUnclamped(a.x, b.x, t)),
                Mathf.RoundToInt(Mathf.LerpUnclamped(a.y, b.y, t)),
                Mathf.RoundToInt(Mathf.LerpUnclamped(a.z, b.z, t))
            );
    }
}
