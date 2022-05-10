using System;
using UnityEngine;

namespace Level
{
    public class CameraRig : MonoBehaviour
    {
        [SerializeField]
        private Bounds localBounds;

        [SerializeField]
        private PitLevel activeLevel;

        [SerializeField]
        private Transform followTarget;

        [SerializeField]
        private Camera frontCamera;

        [SerializeField] 
        [Range(0, 1)]
        private float lerpFactor = 1;

        private Vector3 targetPos;

        public Bounds Bounds => new(transform.position + localBounds.center, localBounds.size);

        private void OnDrawGizmos()
        {
            var b = Bounds;
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(b.center, b.size);
        }

        private void FixedUpdate()
        {
            targetPos = followTarget.transform.position;
            var cameraPos = frontCamera.transform.position;
            var halfHeight = frontCamera.orthographicSize;
            var targetBottom = targetPos.y - halfHeight;

            var levelBottom = activeLevel.transform.position.y - activeLevel.height / 2f;

            targetPos.y = Mathf.Max(targetBottom, levelBottom) + halfHeight;
            targetPos.z = cameraPos.z;
            cameraPos = Vector3.Lerp(cameraPos, targetPos, lerpFactor);
            
            transform.position = cameraPos;
            
            activeLevel.UpdateRooms(this);
        }
    }
}