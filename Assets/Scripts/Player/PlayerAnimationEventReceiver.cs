using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerAnimationEventReceiver : MonoBehaviour
    {
        [Tooltip("Called when player leaves the ground when jumping")]
        public UnityEvent onJumpTakeoff;
        [Tooltip("Called when player death animation has completed")]
        public UnityEvent onDeath;
        [Tooltip("Called when player puts a foot down in a step")]
        public UnityEvent onStep;
        
        public void JumpTakeoff()
        {
            onJumpTakeoff.Invoke();
        }

        public void Death()
        {
            onDeath.Invoke();
        }

        public void Step()
        {
            onStep.Invoke();
        }
    }
}