using Conditions;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Level
{
    // An interaction that is enabled when the player is close enough
    public class Interaction : MonoBehaviour
    {
        public string actionDescription;
        public Condition requiredCondition;

        public UnityEvent onInteraction;
        public UnityEvent onPlayerApproach;
        public UnityEvent onPlayerDepart;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (requiredCondition != null && !requiredCondition.Evaluate()) return;
            if (!other.gameObject.CompareTag("Player")) return;
            if (!other.gameObject.TryGetComponent<PlayerActionController>(out var pac)) return;

            pac.Interaction = this;
            onPlayerApproach.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (!other.gameObject.TryGetComponent<PlayerActionController>(out var pac)) return;

            onPlayerDepart.Invoke();
            if (pac.Interaction == this) pac.Interaction = null;
        }
        
        public void Perform()
        {
            if (requiredCondition != null && !requiredCondition.Evaluate()) return;
            onInteraction.Invoke();
        }
    }
}