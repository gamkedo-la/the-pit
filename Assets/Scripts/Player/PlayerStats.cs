using System.Collections;
using System.Collections.Generic;
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

    private float damageTimer = 0;
    private DamageInflicted damage;

    void Start()
    {
        // initialize playerHealth variable
        playerHealth.MinValue = playerMinHealth;
        playerHealth.MaxValue = playerMaxHealth;
        playerHealth.Value = playerMaxHealth;
    }

    private void Update()
    {
        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
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


    // ******************************* Collision Handling ********************************
    private void OnTriggerEnter2D(Collider2D other) { CheckForDamage(other); }
    private void OnTriggerStay2D(Collider2D other) { CheckForDamage(other); }
    private void OnCollisionEnter2D(Collision2D other) { CheckForDamage(other.collider); }
    private void OnCollisionStay2D(Collision2D other) { CheckForDamage(other.collider); }

    private void CheckForDamage(Collider2D collider)
    {
        if (collider.CompareTag("Enemy") && damageTimer <= 0)
        {
            if (collider.TryGetComponent<DamageInflicted>(out damage))
            {
                ApplyDamage(damage.Damage);
                damageTimer = damageCooldown;
            }
        }
    }
}
