using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class BlobbyBrain : MonoBehaviour
    {
        [Min(0)]
        public float aggressionRadius;
        
        [Min(0)]
        public float angleSpread;

        [Min(0.1f)]
        public float attackCooldown = 0.5f;

        [Min(0.1f)]
        public float targetUpdateTime = 0.5f;

        [Min(0.1f)]
        public float shootingVelocity = 1f;

        public Transform mouth;

        public Animator animator;

        public Rigidbody2D projectilePrefab;

        private float cooldownExpire;
        private Transform target;
        private Vector2 initialVelocity;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return WaitForTarget();
                yield return AttackTarget();
            }
        }

        private IEnumerator WaitForTarget()
        {
            while (target == null)
            {
                yield return new WaitForSeconds(targetUpdateTime);
                var obj = GameObject.FindWithTag("PlayerAttackTarget");
                if (obj == null) continue;
                if (Vector3.Distance(mouth.position, obj.transform.position) > aggressionRadius) continue;
                target = obj.transform;
            }
        }

        private IEnumerator AttackTarget()
        {
            while (target != null && Vector3.Distance(mouth.position, target.position) <= aggressionRadius)
            {
                if (cooldownExpire > Time.time)
                {
                    yield return new WaitForSeconds(cooldownExpire - Time.time);
                    continue;
                }

                Vector2 attackVector = target.position - mouth.position;
                Vector2 forwardVector = mouth.right;

                var angle = Vector2.SignedAngle(forwardVector, attackVector);
                if (angle > angleSpread || angle < -angleSpread)
                {
                    yield return null;
                    continue;
                }

                animator.SetTrigger("Shoot");
                cooldownExpire = Time.time + attackCooldown;
                initialVelocity = shootingVelocity * attackVector.normalized;
            }

            target = null;
        }

        public void LaunchProjectile()
        {
            var projectile = Instantiate(projectilePrefab);
            projectile.position = mouth.position;
            projectile.velocity = initialVelocity;
        }
        
        private void OnDrawGizmosSelected()
        {
            var baseAngle = mouth.rotation.eulerAngles.z;
            Gizmos.color = Color.magenta;
            GizmoHelper.DrawArc(mouth.transform.position, aggressionRadius, baseAngle - angleSpread, baseAngle + angleSpread);
        }
    }
}