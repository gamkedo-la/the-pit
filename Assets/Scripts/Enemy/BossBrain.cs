using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D.IK;

namespace Enemy
{
    public class BossBrain : MonoBehaviour
    {
        [Header("Movement")] public float movementRadius = 4;
        public float movementSpeed = 2;
        public AnimationCurve verticalMovement = AnimationCurve.EaseInOut(0, -1, 7, 1);
        public AnimationCurve horizontalMovement = AnimationCurve.EaseInOut(-5.5f, -1, 5.5f, 1);

        [Header("Attack")] 
        public float initialAttackDelay = 4;
        public float attackCooldown;
        public AttackParameters[] attackSequence;

        [Header("Events")] 
        public UnityEvent onAttackWindup;
        public UnityEvent onAttackEnd;
        public UnityEvent onDeath;

        [Header("Parts")] 
        public GameObject[] tentacles;

        private Animator animator;
        private Vector2 movementCenter;
        private float movementTime;
        private int attackSequenceIndex = 0;

        public bool ExecutingAttack { get; set; }

        private void Start()
        {
            animator = GetComponent<Animator>();
            verticalMovement.postWrapMode = WrapMode.PingPong;
            horizontalMovement.postWrapMode = WrapMode.PingPong;
            enabled = false;
        }

        private void ActivateTentacleIK()
        {
            foreach (var tentacle in tentacles)
            {
                if (tentacle.TryGetComponent<IKManager2D>(out var ikManager2D))
                {
                    ikManager2D.enabled = true;
                }
            }
        }

        public void Engage()
        {
            movementCenter = transform.position;
            movementCenter += Vector2.up * movementRadius;
            movementTime = Time.time;
            animator.enabled = false;
            enabled = true;
            ActivateTentacleIK();
            StartCoroutine(AttackSequence());
        }

        public void Enrage()
        {
        }

        public void Die()
        {
            animator.enabled = true;
            enabled = false;
            onDeath.Invoke();
        }

        private IEnumerator AttackSequence()
        {
            yield return new WaitForSeconds(initialAttackDelay);
            attackSequenceIndex = 0;
            while (true)
            {
                ExecutingAttack = true;
                onAttackWindup.Invoke();

                yield return attackSequence[attackSequenceIndex].Perform();

                attackSequenceIndex = (attackSequenceIndex + 1) % attackSequence.Length;
                
                onAttackEnd.Invoke();
                ExecutingAttack = false;
                yield return new WaitForSeconds(attackCooldown);
            }
        }


        private void Update()
        {
            MoveAbout();
        }

        private void MoveAbout()
        {
            if (ExecutingAttack)
            {
                // Pause movement
                movementTime += Time.deltaTime;
                return;
            }

            var t = (Time.time - movementTime) * movementSpeed;
            var x = horizontalMovement.Evaluate(t) * movementRadius;
            var y = verticalMovement.Evaluate(t) * movementRadius;
            var p = transform.position;
            p.x = movementCenter.x + x;
            p.y = movementCenter.y + y;
            transform.position = p;
        }

        [Serializable]
        public class AttackParameters
        {
            public float attackWindup;
            public int numberOfBolts;
            public float boltSpreadDegrees;
            public float boltSpreadSeconds;
            public float boltShootingSpeed;
            public Transform boltSpawnPoint;

            [Header("Events")] 
            public UnityEvent onAttackBegin;
            public UnityEvent onBoltSpawned;

            [Header("Prefabs")] public GameObject boltPrefab;

            public AttackParameters()
            {
                boltSpawnPoint = null;
                boltShootingSpeed = 2f;
                boltSpreadSeconds = 0.2f;
                boltSpreadDegrees = 15;
                numberOfBolts = 5;
            }

            public IEnumerator Perform()
            {
                yield return new WaitForSeconds(attackWindup);
                
                var numberOfSpreads = numberOfBolts / 2;
                var player = GameObject.FindGameObjectWithTag("PlayerAttackTarget");
                var boltDirection = (player.transform.position - boltSpawnPoint.position).normalized;
                onAttackBegin.Invoke();
                for (var i = 0; i < numberOfBolts; i++)
                {
                    var spread = ((i + 1) / 2) * (i % 2 == 0 ? -1 : 1) * boltSpreadDegrees / numberOfSpreads;
                    SpawnBolt(Quaternion.Euler(0, 0, spread) * boltDirection);
                    yield return new WaitForSeconds(boltSpreadSeconds);
                }

            }

            private void SpawnBolt(Vector3 boltDirection)
            {
                var bolt = Instantiate(boltPrefab, boltSpawnPoint.position, Quaternion.identity);
                bolt.GetComponent<Rigidbody2D>().velocity = boltShootingSpeed * boltDirection;
                onBoltSpawned.Invoke();
            }
        }
    }
}