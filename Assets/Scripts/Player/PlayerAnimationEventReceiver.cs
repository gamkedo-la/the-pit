using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerAnimationEventReceiver : MonoBehaviour
    {
        public UnityEvent onJumpTakeoff;
        
        public void JumpTakeoff()
        {
            Debug.Log("Jump takeoff!");
            onJumpTakeoff.Invoke();
        }
    }
}