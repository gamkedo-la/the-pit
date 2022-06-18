using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerAnimationEventReceiver : MonoBehaviour
    {
        public UnityEvent onJumpTakeoff;
        public UnityEvent onDeath;
        
        public void JumpTakeoff()
        {
            onJumpTakeoff.Invoke();
        }

        public void Death()
        {
            onDeath.Invoke();
        }
    }
}