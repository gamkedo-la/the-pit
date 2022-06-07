using UnityEngine;

namespace Level
{
    public class Elevator : MonoBehaviour
    {
        [Min(0)]
        public float maxHeight;
        public float currentHeight;
        public float speed;

        [Header("Components")] 
        public Rigidbody2D platform;
        public Transform shaft;

        private float zeroPosition;


        private void Start()
        {
            zeroPosition = platform.position.y;
            UpdatePlatformPosition();
        }

        private void UpdatePlatformPosition()
        {
            var p = platform.position;
            p.y = currentHeight + zeroPosition;
            platform.MovePosition(p);

            var s = shaft.localScale;
            s.y = currentHeight;
            shaft.localScale = s;
        }

        public float Move(float delta)
        {
            var newHeight = Mathf.Clamp(currentHeight + delta * speed, 0, maxHeight);
            var actualDelta = newHeight - currentHeight;
            currentHeight = newHeight;
            UpdatePlatformPosition();
            return actualDelta;
        }
    }
}
