using UnityEngine;
using Variables;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private MinMaxIntVariable playerHealth;

    [SerializeField, Tooltip("Player minimum health.")]
    private int playerMinHealth = 0;

    [SerializeField, Tooltip("Player maximum health.")]
    private int playerMaxHealth = 100;

    [SerializeField, Tooltip("How long is player invulnerable after taking damage?")]
    private float damageCooldown = 0.5f;

    [SerializeField]
    private float defaultKnockback;

    [SerializeField] 
    private SpriteRenderer[] spriteRenderers;

    [SerializeField]
    private float hurtFlashFrequency = 25f;

    [SerializeField]
    private Color hurtFlashColor = new Color(1f, 1f, 1f, 0.2f);
    
    private float damageTimer = 0;
    private DamageInflicted damage;
    private Rigidbody2D rb2d;

    void Start()
    {
        // initialize playerHealth variable
        rb2d = GetComponent<Rigidbody2D>();
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
    }


    private void ApplyDamage(int damage)
    {
        playerHealth.Subtract(damage);

        if (playerHealth.Value <= 0)
        {
            Debug.Log("You have died");
        }
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
    private void OnTriggerEnter2D(Collider2D other) { CheckForDamage(other, other.transform.position); }
    private void OnTriggerStay2D(Collider2D other) { CheckForDamage(other, other.transform.position); }
    private void OnCollisionEnter2D(Collision2D other) { CheckForDamage(other.collider, other.GetContact(0).point); }
    private void OnCollisionStay2D(Collision2D other) { CheckForDamage(other.collider, other.GetContact(0).point); }

    private void CheckForDamage(Collider2D collider, Vector2 hitPos)
    {
        if (collider.CompareTag("Enemy") && damageTimer <= 0)
        {
            if (collider.TryGetComponent<DamageInflicted>(out damage))
            {
                ApplyDamage(damage.Damage);
                damageTimer = damageCooldown;
                if (damage.HasKnockback)
                {
                    ApplyKnockback(hitPos, damage.Knockback);
                }
                else
                {
                    ApplyKnockback(hitPos, defaultKnockback);
                }
            }
        }
    }
}
