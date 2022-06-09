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
        public int minAim = -90;
        [Range(-90, 90)]
        public int maxAim = 90;

        [Range(0, 1)] public float flipDeadZone = 0.1f;

        [Header("Sound")] 
        public AudioSource audioPlayer;
        public AudioClip shootSound;

        [Header("Links")] 
        public Animator bodyAnimator;
        public Transform aimRotationPoint;
        public Transform muzzle;
        public Animator muzzleFlash;
        public GameObject projectileImpactPrefab;
        [Tooltip("How long the projectile impact should last as a game object before being removed")]
        public float projectileImpactDuration;

        private float activeCooldown = 0;
        private static readonly int Shoot = Animator.StringToHash("Shoot");
        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (activeCooldown > 0)
            {
                activeCooldown -= Time.deltaTime;
            }
            var aimPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var direction = transform.localScale.x;
            if (direction > 0 && aimPosition.x < transform.position.x - flipDeadZone)
            {
                direction = -1;
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else if (direction < 0 && aimPosition.x > transform.position.x + flipDeadZone)
            {
                direction = 1;
                transform.localScale = Vector3.one;
            }

            var aimRotationPosition = aimRotationPoint.transform.position;
            Vector2 shotVector = aimPosition - aimRotationPosition;
            var shotAngle = Vector2.SignedAngle(Vector2.right * direction, shotVector)*Mathf.Sign(direction);
            var shotAngle01 = Mathf.Clamp01(Mathf.InverseLerp(maxAim, minAim, shotAngle));
            bodyAnimator.SetFloat("Aim Angle", shotAngle01);
            if (Input.GetMouseButtonDown(0) || (automaticFire && Input.GetMouseButton(0)))
            {
                FireIfPossible(aimPosition, shotAngle);
            }
        }

        private void FireIfPossible(Vector2 aimPoint, float angle)
        {
            if (activeCooldown > 0) return;
            if (angle < minAim || angle > maxAim) return;
            
            Vector2 muzzlePos = muzzle.transform.position;

            var shotVector = aimPoint - muzzlePos;
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
            health.Damage(damage, hitObject, hitPoint);
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var p0 = aimRotationPoint.transform.position;
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