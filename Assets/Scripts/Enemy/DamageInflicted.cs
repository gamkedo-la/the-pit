using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageInflicted : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    public int Damage => damage;
}
