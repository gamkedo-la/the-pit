using UnityEngine;
using UnityEngine.Events;
using Variables;

namespace Combat
{
    public class Health : MonoBehaviour
    {
        [Tooltip("Track health in this health variable")]
        public HealthVariable trackedHealth;

        [SerializeField] private int initialHealth;

        public UnityEvent<int, int, int> onHealthDecreased;
        public UnityEvent onHealthDepleted;

        public GameObject impactPrefab;
        public float impactDuration;


        private int actualHealth;

        public int InitialHealth
        {
            get => trackedHealth == null ? initialHealth : trackedHealth.MaxHealth;
        }

        public int ActualHealth
        {
            get => trackedHealth == null ? actualHealth : trackedHealth.Health;
            set
            {
                if (trackedHealth == null) actualHealth = value;
                else trackedHealth.Health = value;
            }
        }

        private void Start()
        {
            ActualHealth = InitialHealth;
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

            onHealthDecreased.Invoke(damage, ActualHealth, InitialHealth);
            ActualHealth -= damage;
            if (ActualHealth <= 0)
            {
                ActualHealth = 0;
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