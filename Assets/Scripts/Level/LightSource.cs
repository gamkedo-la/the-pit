using Enemy;
using UnityEngine;

namespace Level
{
    public class LightSource : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Photosensitive>(out var p))
            {
                p.EnterLight(transform.position);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent<Photosensitive>(out var p))
            {
                p.ExitLight(transform.position);
            }
        }
    }
}