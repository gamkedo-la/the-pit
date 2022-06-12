using UnityEngine;
using UnityEngine.Events;

namespace Conditions
{
    public class ConditionListener : MonoBehaviour
    {
        public Condition condition;
        public UnityEvent onConditionTrue;
        public UnityEvent onConditionFalse;

        private bool currentValue;
        
        private void OnEnable()
        {
            currentValue = condition.Evaluate();
            DispatchEvent();
        }

        private void DispatchEvent()
        {
            if (currentValue) onConditionTrue.Invoke();
            else onConditionFalse.Invoke();
        }

        private void FixedUpdate()
        {
            // TODO: listen to condition for changes instead, if and when that is possible
            var newValue = condition.Evaluate();
            if (newValue == currentValue) return;

            currentValue = newValue;
            DispatchEvent();
        }
    }
}