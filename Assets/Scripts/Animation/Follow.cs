using UnityEngine;

namespace Animation
{
    public class Follow : MonoBehaviour
    {
        [Tooltip("Target to follow")]
        public Transform target;

        [Header("Axis Lock")] 
        [Tooltip("Do not move in the X axis")]
        public bool x;
        [Tooltip("Do not move in the Y axis")]
        public bool y;
        [Tooltip("Do not move in the Z axis")]
        public bool z;

        private void LateUpdate()
        {
            var targetPos = target.position;
            var currentPos = transform.position;

            if (!x) currentPos.x = targetPos.x;
            if (!y) currentPos.y = targetPos.y;
            if (!z) currentPos.z = targetPos.z;
        
            transform.position = currentPos;
        }
    }
}