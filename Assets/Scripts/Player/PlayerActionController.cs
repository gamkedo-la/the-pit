using Level;
using UnityEngine;

namespace Player
{
    public class PlayerActionController : MonoBehaviour
    {
        public float actionCooldown = 0.5f;
        private float onCooldown;

        public Interaction Interaction { get; set; }

        private void Update()
        {
            if (onCooldown > 0)
            {
                onCooldown -= Time.deltaTime;
            }
            
            // Todo: Define button in input
            if (Input.GetKeyDown(KeyCode.E) && Interaction != null)
            {
                Interaction.Perform();
                onCooldown = actionCooldown;
            }
        }
    }
}