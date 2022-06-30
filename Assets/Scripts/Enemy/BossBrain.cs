using UnityEngine;
using UnityEngine.U2D.IK;

namespace Enemy
{
    public class BossBrain : MonoBehaviour
    {
        [Header("Movement")]
        public float movementRadius = 4;
        public float movementSpeed = 2;
        public AnimationCurve verticalMovement = AnimationCurve.EaseInOut(0, -1, 7, 1);
        public AnimationCurve horizontalMovement = AnimationCurve.EaseInOut(-5.5f, -1, 5.5f, 1);
        
        [Header("Parts")]
        public GameObject[] tentacles;

        private Animator animator;
        private Vector2 movementCenter;
        private float movementTime;

        private void Start()
        {
            animator = GetComponent<Animator>();
            verticalMovement.postWrapMode = WrapMode.PingPong;
            horizontalMovement.postWrapMode = WrapMode.PingPong;
        }

        public void ActivateTentacleIK()
        {
            foreach (var tentacle in tentacles)
            {
                if (tentacle.TryGetComponent<IKManager2D>(out var ikManager2D))
                {
                    ikManager2D.enabled = true;
                }
            }
        }

        public void MoveAbout()
        {
            movementCenter = transform.position;
            movementCenter += Vector2.up * movementRadius;
            movementTime = Time.time;
            animator.enabled = false;
        }

        private void Update()
        {
            if (animator.enabled) return;
            
            var t = (Time.time - movementTime) * movementSpeed;
            var x = horizontalMovement.Evaluate(t) * movementRadius;
            var y = verticalMovement.Evaluate(t) * movementRadius;
            var p = transform.position;
            p.x = movementCenter.x + x;
            p.y = movementCenter.y + y;
            transform.position = p;
        }
    }
}