using System.Collections;
using Conditions;
using UnityEngine;

namespace Level
{
    public class Elevator : MonoBehaviour
    {
        [Min(0)]
        public float maxHeight;
        public float currentHeight;
        public float speed;

        public Condition requiredCondition;

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

        private bool CanMove()
        {
            return requiredCondition == null || requiredCondition.Evaluate();
        }

        public float Move(float delta)
        {
            if (!CanMove()) return 0;
            var newHeight = Mathf.Clamp(currentHeight + delta * speed, 0, maxHeight);
            var actualDelta = newHeight - currentHeight;
            currentHeight = newHeight;
            UpdatePlatformPosition();
            return actualDelta;
        }

        public void MoveTo(float height)
        {
            if (!CanMove()) return;
            StartCoroutine(MoveToAsync(Mathf.Clamp(height, 0, maxHeight)));
        }

        private IEnumerator MoveToAsync(float destinationHeight)
        {
            while (Mathf.Abs(destinationHeight - currentHeight) > 1e-3)
            {
                yield return new WaitForFixedUpdate();
                var maxDistanceToMove = Mathf.Min(speed * Time.fixedDeltaTime, Mathf.Abs(destinationHeight - currentHeight)) * Mathf.Sign(destinationHeight - currentHeight);
                currentHeight += maxDistanceToMove;
                UpdatePlatformPosition();
            }
        }
    }
}
