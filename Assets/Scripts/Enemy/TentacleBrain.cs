using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class TentacleBrain : MonoBehaviour
    {
        [FormerlySerializedAs("detectionRadius")] 
        [Min(0)]
        public float aggressionRadius;

        [Min(0)]
        public float disengageRadius;

        [SerializeField]
        private float targetUpdateTime = 1f;

        [SerializeField]
        private float attackPrepareSpeed = 2.5f;

        [SerializeField]
        private float attackPrepareDistance = 8f;

        [SerializeField] 
        private float attackChargeTime = 0.2f;

        [SerializeField] 
        private float attackExecutionSpeed = 10f;

        [SerializeField]
        private float attackPunchThroughDistance = 4f;

        [SerializeField]
        private float attackCooldownTime = 1f;

        [SerializeField]
        private Transform attackPoint;

        [SerializeField] 
        private DamageInflicted attackExecutor;

        private Transform target;
        private TentacleMovement movement;

        private IEnumerator Start()
        {
            movement = GetComponent<TentacleMovement>();
            SetAttackActive(false);
            while (true)
            {
                yield return WaitForTarget();
                yield return AttackTarget();
            }
        }

        private void SetAttackActive(bool active)
        {
            attackExecutor.gameObject.SetActive(active);
        }

        private IEnumerator WaitForTarget()
        {
            while (target == null)
            {
                movement.MoveToRandomPoint();
                yield return new WaitForSeconds(targetUpdateTime);
                var obj = GameObject.FindWithTag("PlayerAttackTarget");
                if (obj == null) continue;
                if (Vector3.Distance(transform.position, obj.transform.position) > aggressionRadius) continue;
                target = obj.transform;
            }
        }

        private IEnumerator AttackTarget()
        {
            while (target != null && Vector3.Distance(transform.position, target.position) <= disengageRadius)
            {
                
                var launchPosition = Vector3.MoveTowards(target.position, transform.position, attackPrepareDistance);
                movement.MoveTo(launchPosition, attackPrepareSpeed);
                while (movement.distance > 0)
                {
                    yield return null;
                }

                yield return new WaitForSeconds(attackChargeTime);
                
                SetAttackActive(true);
                
                var direction = target.position - attackPoint.position;
                var attackEndPosition = attackPoint.position + direction + direction.normalized * attackPunchThroughDistance;
                movement.MoveTo(attackEndPosition, attackExecutionSpeed);
                while (movement.distance > 0)
                {
                    yield return null;
                }
                
                SetAttackActive(false);

                yield return new WaitForSeconds(attackCooldownTime);
            }

            target = null;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, aggressionRadius);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, disengageRadius);
        }
    }
}