using Level;
using UnityEngine;

namespace Player
{
    public class PlayerElevatorController : MonoBehaviour
    {
        public Elevator Elevator { get; set; }

        private Rigidbody2D playerRigidbody;

        private void Start()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (Elevator == null) return;

            var vert = Input.GetAxis("Vertical");
            if (vert == 0) return;

            var actualDelta = Elevator.Move(vert * Time.deltaTime);
            if (actualDelta != 0)
            {
                playerRigidbody.MovePosition(playerRigidbody.position + Vector2.up * actualDelta);
            }
        }
    }
}