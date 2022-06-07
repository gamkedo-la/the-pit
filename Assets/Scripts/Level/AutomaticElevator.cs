using System.Collections;
using UnityEngine;

namespace Level
{
    [RequireComponent(typeof(Elevator))]
    public class AutomaticElevator : MonoBehaviour
    {
        [Min(0)]
        public float pauseTime;

        private Elevator elevator;
        private int direction = 1;

        private IEnumerator Start()
        {
            elevator = GetComponent<Elevator>();

            do
            {
                yield return new WaitForSeconds(pauseTime);
                while (elevator.Move(Time.fixedDeltaTime * direction) != 0)
                {
                    yield return new WaitForFixedUpdate();
                }

                direction = -direction;
            } while (true);
        }
        
    }
}