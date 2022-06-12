using System.Collections.Generic;
using System.Linq;
using Combat;
using UnityEngine;

namespace Player
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("Available weapons")] 
        public List<Weapon> weapons;

        [Header("Aim arc in degrees")]
        [Range(-90, 90)]
        public int minAim = -90;
        [Range(-90, 90)]
        public int maxAim = 90;

        [Range(0, 1)] public float flipDeadZone = 0.1f;

        [Header("Links")] 
        public Animator bodyAnimator;
        public Transform aimRotationPoint;
        public Transform weaponGrip;

        private Camera mainCamera;
        private Weapon weapon;

        private void Start()
        {
            mainCamera = Camera.main;
            NextWeapon();
        }

        private void NextWeapon()
        {
            if (weapon == null)
            {
                Debug.Log("Selected first weapon");
                SelectWeapon(weapons.First());
                return;
            }

            var idx = weapons.FindIndex(w => w.id == weapon.id);
            Debug.Log("1. idx=" + idx);
            idx = (idx + 1) % weapons.Count;
            Debug.Log("2. idx=" + idx);
            
            SelectWeapon(weapons[idx]);
        }

        private void SelectWeapon(Weapon selectedWeapon)
        {
            Debug.Log("Select " + selectedWeapon.name);
            if (weapon != null && selectedWeapon.id == weapon.id) return;
            
            if (weapon != null) Destroy(weapon.gameObject);
            weapon = Instantiate(selectedWeapon, weaponGrip);
        }


        private void Update()
        {
            // TODO: Define button in input manager
            if (Input.GetKeyDown(KeyCode.Q)) NextWeapon();
            
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
            var aimFrame = shotAngle switch
            {
                > 33.75f => 1,
                > 11.25f => 2,
                > -11.25f => 3,
                > -33.75f => 4,
                _ => 5
            };
            bodyAnimator.SetInteger("Aim Frame", aimFrame);
            if (shotAngle < minAim || shotAngle > maxAim) return;
            if (Input.GetMouseButtonDown(0) || (weapon.automaticFire && Input.GetMouseButton(0)))
            {
                weapon.FireIfPossible(aimPosition, shotAngle);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            var p0 = aimRotationPoint.transform.position;
            var arcLen = weapon.maxRange;
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