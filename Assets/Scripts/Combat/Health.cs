using System;
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

        public Impact[] impacts;

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

        public void Damage(int damage, GameObject hitObject, Vector2 hitPoint)
        {
            if (damage <= 0) return;
            foreach (var impact in impacts)
            {
                var obj = Instantiate(impact.prefab);
                if (impact.follow) obj.transform.parent = hitObject.transform;
                Vector3 impactPoint = hitPoint;
                impactPoint.z = gameObject.transform.position.z;
                obj.transform.position = impactPoint;
                Destroy(obj, impact.duration);
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

        [Serializable]
        public struct Impact
        {
            public GameObject prefab;
            public bool follow;
            public float duration;
        }
    }
}