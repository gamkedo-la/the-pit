using Audio;
using UnityEngine;
using Util;

namespace Combat
{
    public class Weapon : MonoBehaviour
    {
        [Min(0)]
        public int id;
        
        [Header("Weapon stats")] 
        [Range(0, 100)]
        public float maxRange = 10;
        [Min(1)]
        public int damage;

        public float cooldownBetweenShots = 0.2f;
        public bool automaticFire;
        
        [Header("Sound")] 
        public AudioSource audioPlayer;
        public AudioClipWithVolume shootSound;
        
        public Transform muzzle;
        public Animator muzzleFlash;
        
        public GameObjectPool projectileImpactPool;

        [Header("Layers")] 
        public LayerMask shotBlockingLayers;
        
        private float activeCooldown;
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private void Start()
        {
            audioPlayer = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (activeCooldown > 0)
            {
                activeCooldown -= Time.deltaTime;
            }
        }

        public void FireIfPossible(Vector2 aimPoint, float angle)
        {
            if (activeCooldown > 0) return;
            
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
            muzzleFlash.SetTrigger(Shoot);
            
            audioPlayer.PlayOneShot(shootSound);

            Vector3 impactPos = muzzlePos + shotVector;
            
            var hit = Physics2D.Raycast(muzzlePos, shotDirection, Mathf.Min(dist, maxRange), shotBlockingLayers);
            if (hit.collider != null)
            {
                impactPos = hit.point;
                if (CheckWhatWasHit(hit.collider.gameObject, hit.point))
                {
                    return;
                }
            }

            impactPos.z = transform.position.z;
            
            // Wasteful, use object pool
            var projectileImpact = projectileImpactPool.Get();
            projectileImpact.transform.position = impactPos;
        }
        
        private bool CheckWhatWasHit(GameObject hitObject, Vector2 hitPoint)
        {
            var health = hitObject.GetComponentInParent<Health>();
            if (health == null) return false;
            health.Damage(damage, hitObject, hitPoint);
            return true;
        }
    }
}