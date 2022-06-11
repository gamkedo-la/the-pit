using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SimpleEnemyMovement : MonoBehaviour
    {
        public Vector2 direction = Vector2.up;

        private Rigidbody2D rb2d;

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                return;
            }
            direction = -direction;
        }

        private void FixedUpdate()
        {
            rb2d.velocity = direction;
        }
    }
    
    
}