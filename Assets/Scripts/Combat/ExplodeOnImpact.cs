using UnityEngine;

namespace Combat
{
    public class ExplodeOnImpact : MonoBehaviour
    {
        public float explosionTimeToLive = 1;
        public GameObject[] explosions;

        private void OnCollisionEnter2D(Collision2D col)
        {
            var point = col.GetContact(0).point;
            Explode(point);
        }

        public void Explode(Vector2 point)
        {
            foreach (var explosion in explosions)
            {
                var o = Instantiate(explosion);
                o.transform.position = point;
                Destroy(o, explosionTimeToLive);
            }

            Destroy(gameObject);
        }
    }
}