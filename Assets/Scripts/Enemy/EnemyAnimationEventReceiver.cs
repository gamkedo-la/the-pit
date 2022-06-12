using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class EnemyAnimationEventReceiver : MonoBehaviour
    {
        public UnityEvent onAttack;
        
        public void Attack()
        {
            onAttack.Invoke();
        }
        
    }
}