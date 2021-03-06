using Audio;
using UnityEngine;
using Variables;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private MinMaxIntVariable playerHealth;

        [SerializeField, Tooltip("Player minimum health.")]
        private int playerMinHealth = 0;

        [SerializeField, Tooltip("Player maximum health.")]
        private int playerMaxHealth = 100;

        [SerializeField, Tooltip("How long is player invulnerable after taking damage?")]
        private float damageCooldown = 0.5f;

        [SerializeField, Tooltip("Lowest collision impulse that causes damage")]
        private float minCollisionImpulse;

        [SerializeField,
         Tooltip("Scale of impulse collision damage to health points, set to 0 to not take collision damage")]
        private float collisionImpulseScale;

        [SerializeField] private float defaultKnockback;

        [SerializeField] private SpriteRenderer[] spriteRenderers;

        [SerializeField] private float hurtFlashFrequency = 25f;

        [SerializeField] private Color hurtFlashColor = new Color(1f, 1f, 1f, 0.2f);

        [SerializeField] private AudioClipWithVolume hurtClip;

        private float damageTimer = 0;
        private DamageInflicted damage;
        private Rigidbody2D rb2d;
        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            rb2d = GetComponent<Rigidbody2D>();
            // initialize playerHealth variable
            playerHealth.MinValue = playerMinHealth;
            playerHealth.MaxValue = playerMaxHealth;
            playerHealth.Value = playerMaxHealth;
        }

        private void Update()
        {
            if (damageTimer > 0)
            {
                damageTimer = Mathf.Max(damageTimer - Time.deltaTime, 0f);
                var isOn = Mathf.Repeat(damageTimer * hurtFlashFrequency, 1f) < 0.5f;
                foreach (var spriteRenderer in spriteRenderers)
                {
                    spriteRenderer.color = isOn ? Color.white : hurtFlashColor;
                }
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                ApplyDamage(100);
            }
        }


        public void ApplyDamage(int damage)
        {
            playerHealth.Subtract(damage);
            audioSource.PlayOneShot(hurtClip);

            if (playerHealth.Value <= 0)
            {
                BroadcastMessage("OnDeath");
                return;
            }

            damageTimer = damageCooldown;
        }


        private void Heal(int healing)
        {
            playerHealth.Add(healing);
        }

        private void ApplyKnockback(Vector2 fromPosition, float force)
        {
            var direction = new Vector2(fromPosition.x < transform.position.x ? 1 : -1, 1f);
            rb2d.velocity = Vector2.zero;
            rb2d.AddForce(direction * force, ForceMode2D.Impulse);
        }

        // ******************************* Collision Handling ********************************
        private void OnTriggerEnter2D(Collider2D other) { CheckForAttackDamage(other, other.transform.position); }

        private void OnCollisionEnter2D(Collision2D other)
        {
            CheckForDamage(other);
        }

        private void CheckForDamage(Collision2D collision)
        {
            if (damageTimer > 0) return;
            var contact = collision.GetContact(0);
            if (CheckForAttackDamage(collision.collider, contact.point)) return;
            // Don't apply collision damage if there was an attack damage
            CheckForCollisionDamage(contact.normalImpulse);
        }

        private void CheckForCollisionDamage(float impulse)
        {
            if (impulse < minCollisionImpulse) return;
            ApplyDamage(Mathf.RoundToInt((impulse - minCollisionImpulse) * collisionImpulseScale));
        }

        private bool CheckForAttackDamage(Collider2D coll, Vector2 hitPos)
        {
            if (coll.TryGetComponent<DamageInflicted>(out damage))
            {
                ApplyDamage(damage.Damage);
                if (damage.HasKnockback)
                {
                    ApplyKnockback(hitPos, damage.Knockback);
                }
                else
                {
                    ApplyKnockback(hitPos, defaultKnockback);
                }

                return true;
            }

            return false;
        }
    }
}