using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        public int initialHealth;

        public UnityEvent<int, int, int> onHealthDecreased;
        public UnityEvent onHealthDepleted;

        public GameObject impactPrefab;
        public float impactDuration;

        private int actualHealth;

        private void Start()
        {
            actualHealth = initialHealth;
        }

        public void Damage(int damage, Vector2 hitPoint)
        {
            if (damage <= 0) return;
            if (impactPrefab != null)
            {
                var impact = Instantiate(impactPrefab);
                Vector3 impactPoint = hitPoint;
                impactPoint.z = gameObject.transform.position.z;
                impact.transform.position = impactPoint;
                Destroy(impact, impactDuration);
            }

            onHealthDecreased.Invoke(damage, actualHealth, initialHealth);
            actualHealth -= damage;
            if (actualHealth <= 0)
            {
                actualHealth = 0;
                onHealthDepleted.Invoke();
            }
        }

        /*
         * Convenience method to destroy game object from unity event
         */
        public void DestroyDelayed(float delay)
        {
            Destroy(gameObject, delay);
        }
        
    }
}