using UnityEngine;
using UnityEngine.Events;

namespace Animation
{
    public class GenericAnimationEventReceiver : MonoBehaviour
    {
        public UnityEvent onEventTriggered;

        public void TriggerEvent()
        {
            onEventTriggered.Invoke();
        }
    }
}