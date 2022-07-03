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
        public float attackWindup = 2;
        public float attackCooldown = 10;
        public int numberOfBolts = 5;
        public float boltSpreadDegrees = 15;
        public float boltSpreadSeconds = 0.2f;
        public float boltShootingSpeed = 2f;
        public Transform boltSpawnPoint;

        [Header("Events")] public UnityEvent onAttackWindup;
        public UnityEvent onAttackBegin;
        public UnityEvent onBoltSpawned;
        public UnityEvent onAttackEnd;

        [Header("Parts")] public GameObject[] tentacles;

        [Header("Prefabs")] public GameObject boltPrefab;

        private Animator animator;
        private Vector2 movementCenter;
        private float movementTime;
        private bool executingAttack;

        private void Start()
        {
            animator = GetComponent<Animator>();
            verticalMovement.postWrapMode = WrapMode.PingPong;
            horizontalMovement.postWrapMode = WrapMode.PingPong;
            enabled = false;
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

        private IEnumerator AttackSequence()
        {
            yield return new WaitForSeconds(initialAttackDelay);
            while (true)
            {
                executingAttack = true;
                onAttackWindup.Invoke();
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

                executingAttack = false;
                onAttackEnd.Invoke();
                yield return new WaitForSeconds(attackCooldown);
            }
        }

        private void SpawnBolt(Vector3 boltDirection)
        {
            var bolt = Instantiate(boltPrefab, boltSpawnPoint.position, Quaternion.identity);
            bolt.GetComponent<Rigidbody2D>().velocity = boltShootingSpeed * boltDirection;
            onBoltSpawned.Invoke();
        }


        private void Update()
        {
            MoveAbout();
        }

        private void MoveAbout()
        {
            if (executingAttack)
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
    }
}