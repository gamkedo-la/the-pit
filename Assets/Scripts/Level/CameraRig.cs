using UnityEngine;

namespace Level
{
    public class CameraRig : MonoBehaviour
    {
        [SerializeField]
        private Bounds localBounds;

        public Bounds Bounds => new(transform.position + localBounds.center, localBounds.size);

        private void OnDrawGizmos()
        {
            var b = Bounds;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(b.center, b.size);
        }
    }
}