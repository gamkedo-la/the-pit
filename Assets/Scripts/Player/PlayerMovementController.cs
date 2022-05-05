using UnityEngine;

namespace Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public float horizontalSpeed;
        [Range(0, 2)]
        public float stepFrequency = 1;

        [Min(0)]
        public float jumpForce;

        [Header("Sound")] 
        public AudioSource audioSource;
        public AudioClip step;
        public AudioClip jump;
        
        public Animator animator;

        private float stepTime = 0;
        private bool wantToJump;
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
            var absHorizontal = Mathf.Abs(horizontal);
            animator.SetFloat(Horizontal, absHorizontal);
            
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
            wantToJump |= Input.GetButtonDown("Jump");
        }

        private void FixedUpdate()
        {
            if (wantToJump)
            {
                wantToJump = false;
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

            if (horizontal > 0)
                transform.localScale = Vector3.one;
            else 
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}