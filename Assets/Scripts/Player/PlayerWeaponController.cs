using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {

        [Header("Weapon stats")] 
        [Range(0, 100)]
        public float maxRange = 10;
        [Min(1)]
        public int damage;

        public float cooldownBetweenShots = 0.2f;
        public bool automaticFire;
    
        [Header("Aim arc in degrees")]
        [Range(-90, 90)]
        public int minAim = -90, maxAim = 90;

        [Header("Sound")] 
        public AudioSource audioPlayer;
        public AudioClip shootSound;

        public Transform muzzle;
        public Animator muzzleFlash;
        public GameObject projectileImpactPrefab;
        [Tooltip("How long the projectile impact should last as a game object before being removed")]
        public float projectileImpactDuration;

        private float activeCooldown = 0;
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private void Update()
        {
            if (activeCooldown > 0)
            {
                activeCooldown -= Time.deltaTime;
            }
            if (Input.GetMouseButtonDown(0) || (automaticFire && Input.GetMouseButton(0)))
            {
                FireIfPossible();
            }
        }

        private void FireIfPossible()
        {
            if (activeCooldown > 0) return;
            Vector2 aimPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 muzzlePos = muzzle.transform.position;

            var shotVector = aimPoint - muzzlePos;
        
            var angle = Vector2.SignedAngle(Vector2.right * transform.localScale.x, shotVector);
            if (angle < minAim || angle > maxAim) return;

            var shotDirection = shotVector.normalized;
        
            // Adjust shot point to max range
            var dist = shotVector.magnitude;
            if (dist > maxRange)
            {
                shotVector = shotDirection * maxRange;
            }

            activeCooldown = cooldownBetweenShots;
            muzzleFlash.transform.rotation = Quaternion.Euler(0, 0, angle);
            muzzleFlash.SetTrigger(Shoot);
            
            audioPlayer.PlayOneShot(shootSound);

            Vector3 impactPos = muzzlePos + shotVector;
            
            var hit = Physics2D.Raycast(muzzlePos, shotDirection);
            if (hit.collider != null)
            {
                impactPos = hit.point;
                if (CheckWhatWasHit(hit.collider.gameObject, hit.point))
                {
                    return;
                }
            }
            
            // Wasteful, use object pool
            var projectileImpact = Instantiate(projectileImpactPrefab);
            projectileImpact.transform.position = impactPos;
            Destroy(projectileImpact, projectileImpactDuration);
        }

        private bool CheckWhatWasHit(GameObject hitObject, Vector2 hitPoint)
        {
            var health = hitObject.GetComponentInParent<Health>();
            if (health == null) return false;
            health.Damage(damage, hitPoint);
            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var p0 = muzzle.transform.position;
            var arcLen = maxRange;
            var p1 = p0 + arcLen * new Vector3(Mathf.Cos(minAim * Mathf.Deg2Rad), Mathf.Sin(minAim * Mathf.Deg2Rad), 0);
            var p2 = p0 + arcLen * new Vector3(Mathf.Cos(maxAim * Mathf.Deg2Rad), Mathf.Sin(maxAim * Mathf.Deg2Rad), 0);
            Gizmos.DrawLine(p0, p1);
            Gizmos.DrawLine(p0, p2);

            for (var i = 1; i <= 30; i++)
            {
                var angle = minAim + i * (maxAim - minAim) / 30f;
                p2 = p0 + arcLen * new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
                Gizmos.DrawLine(p1, p2);
                p1 = p2;
            }
        }
    }
}