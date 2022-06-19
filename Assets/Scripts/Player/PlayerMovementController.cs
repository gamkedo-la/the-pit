using Audio;
using JetBrains.Annotations;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public float horizontalSpeed;
        [Tooltip("Multiply with horizontal speed when backing up")]
        [Range(0, 1)]
        public float backingUpSpeedFactor = 0.5f;
        [Range(0, 2)]
        public float stepFrequency = 1;

        [Min(0)]
        public float jumpForce;

        [Header("Sound")] 
        public AudioSource audioSource;
        public AudioClipWithVolume step;
        public AudioClipWithVolume jump;

        [Header("Ground sensor")] 
        [Min(1)]
        public int minHitsToBeGrounded = 1;
        public Transform groundSensorContainer;
        public float groundDetectionDistance = 0.05f;
        public LayerMask groundLayers;
        
        [Header("Animation")]
        public Animator animator;

        [Header("Diagnostic")] 
        public bool onGround;

        public bool JumpTakeoff { get; set; }
        
        private float stepTime = 0;
        private float horizontal;
        private Rigidbody2D rb2d;

        private Transform[] groundSensors;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            groundSensors = new Transform[groundSensorContainer.childCount];
            for (var i = 0; i < groundSensors.Length; i++)
            {
                groundSensors[i] = groundSensorContainer.GetChild(i);
            }
        }

        [UsedImplicitly]
        private void OnDeath()
        {
            animator.SetTrigger("Die");
            Destroy(this);
        }

        private void Update()
        {
            horizontal = Input.GetAxis("Horizontal") * horizontalSpeed;
            var movement = horizontal * Mathf.Sign(transform.localScale.x);
            if (movement < 0)
            {
                movement *= backingUpSpeedFactor;
                horizontal *= backingUpSpeedFactor;
            }
            
            animator.SetFloat(Horizontal, movement);
            var absHorizontal = Mathf.Abs(horizontal);
            
            if (absHorizontal > 0)
            {
                stepTime += absHorizontal * stepFrequency * Time.deltaTime;
                if (stepTime >= 1f)
                {
                    audioSource.PlayOneShot(step);
                    stepTime -= 1f;
                }
            }
            else
            {
                stepTime = 0;
            }

            if (Input.GetButtonDown("Jump") && onGround)
            {
                animator.SetTrigger("Jump");
            }
        }

        private void FixedUpdate()
        {
            DetectGround();
            if (JumpTakeoff)
            {
                JumpTakeoff = false;
                rb2d.AddForce(jumpForce * rb2d.gravityScale * Vector2.up);
                audioSource.PlayOneShot(jump);
            }
            
            MoveHorizontal();
        }

        private void DetectGround()
        {
            var count = 0;
            foreach (var sensor in groundSensors)
            {
                var hit = Physics2D.Raycast(sensor.position, Vector2.down, groundDetectionDistance, groundLayers);
                if (hit.collider != null)
                {
                    count++;
                    if (count >= minHitsToBeGrounded) break;
                }
            }

            onGround = count >= minHitsToBeGrounded;
            animator.SetBool("On Ground", onGround);
        }

        private void MoveHorizontal()
        {
            var v = rb2d.velocity;
            v.x = horizontal;
            rb2d.velocity = v;
        }
    }
}