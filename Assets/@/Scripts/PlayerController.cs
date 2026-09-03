using UnityEngine;

namespace Implicitly.Samples
{
    [AddComponentMenu("Implicitly/Samples/Player Controller")]
    public class PlayerController : MonoBehaviour
    {
        public void SetPosition(Vector2 position) => transform.position = position;
    }
}
