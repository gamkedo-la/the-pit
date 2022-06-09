using Audio;
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
        
        public Animator animator;

        public bool JumpTakeoff { get; set; }
        
        private float stepTime = 0;
        private float horizontal;
        private Rigidbody2D rb2d;

        private static readonly int Horizontal = Animator.StringToHash("Horizontal");

        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
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

            if (Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Jump");
            }
        }

        private void FixedUpdate()
        {
            if (JumpTakeoff)
            {
                JumpTakeoff = false;
                rb2d.AddForce(Vector2.up * jumpForce);
                audioSource.PlayOneShot(jump);
            }
            
            MoveHorizontal();
        }

        private void MoveHorizontal()
        {
            if (horizontal == 0) return;

            var v = rb2d.velocity;
            v.x = horizontal;
            rb2d.velocity = v;
        }
    }
}