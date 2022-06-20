using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class BabyTentacleBrain : MonoBehaviour
    {
        [Min(0)]
        public float aggressionRadius;
        [Min(0)]
        public float disengageRadius;
        [Min(0.1f)]
        public float targetUpdateTime = 0.5f;

        [Min(0f)]
        public float attackCooldownTime;

        [Tooltip("The distance at which the creature will jump at the player and cause damage")]
        [Range(0, 3f)]
        public float attackRadius;

        public Collider2D attackCollider;

        [Min(0)]
        public float movementSpeed;

        public bool InShadows { get; set; }
        
        private Transform target;
        private Rigidbody2D rb2d;
        
        private IEnumerator Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            InShadows = true;
            attackCollider.enabled = false;
            while (true)
            {
                yield return WaitForTarget();
                yield return ChaseTarget();
            }
        }

        private IEnumerator WaitForTarget()
        {
            while (target == null)
            {
                yield return new WaitForSeconds(targetUpdateTime);
                var obj = GameObject.FindWithTag("PlayerAttackTarget");
                if (obj == null) continue;
                if (Vector3.Distance(transform.position, obj.transform.position) > aggressionRadius) continue;
                target = obj.transform;
            }
        }

        private IEnumerator ChaseTarget()
        {
            while (target != null && Vector2.Distance(transform.position, target.position) <= disengageRadius)
            {

                Vector2 movementDirection = target.position - transform.position;
                var movementDistance = movementDirection.magnitude;
                if (movementDistance > disengageRadius) break;

                if (movementDistance <= attackRadius)
                {
                    rb2d.velocity = movementDirection * 10f;
                    attackCollider.enabled = true;
                    yield return new WaitForSeconds(0.1f);
                    attackCollider.enabled = false;
                    rb2d.velocity = movementDirection * -10;
                    yield return new WaitForSeconds(0.1f);
                    rb2d.velocity = Vector2.zero;
                    yield return new WaitForSeconds(attackCooldownTime);
                    continue;
                }
                
                if (!InShadows)
                {
                    rb2d.velocity = Vector2.zero;
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                rb2d.velocity = movementSpeed * movementDirection.normalized;
                yield return new WaitForFixedUpdate();
            }
            rb2d.velocity = Vector2.zero;
        }

    }
}