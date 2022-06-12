﻿using System.Collections.Generic;
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
                SelectWeapon(weapons.First());
                return;
            }

            var idx = weapons.FindIndex(w => w.id == weapon.id);
            idx = (idx + 1) % weapons.Count;
            
            SelectWeapon(weapons[idx]);
        }

        private void SelectWeapon(Weapon selectedWeapon)
        {
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
            GizmoHelper.DrawArc(aimRotationPoint.transform.position, weapon.maxRange, minAim, maxAim);
        }
    }
}