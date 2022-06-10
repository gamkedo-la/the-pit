using UnityEngine;

public class DamageInflicted : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce;

    public int Damage => damage;
    public bool HasKnockback => knockbackForce > 0;
    public float Knockback => knockbackForce;
}
