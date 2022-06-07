using Player;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Elevator))]
    public class PlayerControlledElevator : MonoBehaviour
    {
        private Elevator elevator;

        private void Start()
        {
            elevator = GetComponent<Elevator>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            var controller = col.gameObject.GetComponentInParent<PlayerElevatorController>();
            if (controller != null)
            {
                controller.Elevator = elevator;
            }
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            var controller = col.gameObject.GetComponentInParent<PlayerElevatorController>();
            if (controller == null) return;
            if (controller.Elevator == elevator) controller.Elevator = null;
        }
        
    }
}