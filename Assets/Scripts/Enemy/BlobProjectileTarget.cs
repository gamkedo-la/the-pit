using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class BlobProjectileTarget : MonoBehaviour
    {
        public int hitsToTrigger = 1;
        public UnityEvent onTriggered;
        public bool removeAfterTrigger;

        private int hits;
        
        public void HitByProjectile()
        {
            hits++;
            if (hits != hitsToTrigger) return;
            
            onTriggered.Invoke();

            if (removeAfterTrigger)
            {
                Destroy(this);
            }
        }
    }
}