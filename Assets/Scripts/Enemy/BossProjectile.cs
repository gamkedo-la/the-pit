using Combat;
using UnityEngine;

namespace Enemy
{
    public class BossProjectile : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.TryGetComponent<BossProjectileTarget>(out var target))
            {
                target.HitByProjectile();
                if (TryGetComponent<ExplodeOnImpact>(out var explode))
                {
                    explode.Explode(transform.position);
                }
            }
        }
    }
}