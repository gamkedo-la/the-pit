using System;
using UnityEngine;

namespace Level
{
    public class PitLevel : MonoBehaviour
    {
        [Min(1)]
        [Tooltip("Circumference of the pit, in units")]
        public int circumference = 408;

        [Min(1)]
        [Tooltip("Height of the level, in units")]
        public int height = 40;

        public CameraRig cameraRig;

        private Room[] rooms;
        private int leftMost, rightMost;
        private float lastCameraX;

        private float Circumference => circumference;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, height  * transform.up + circumference  * transform.right);
        }

        private void Start()
        {
            rooms = GetComponentsInChildren<Room>();
            foreach (var room in rooms)
            {
                room.CalculateExtents();
            }
            
            Array.Sort(rooms, (r1, r2) => r1.worldMin.x.CompareTo(r2.worldMin.x));
            leftMost = 0;
            rightMost = rooms.Length - 1;
        }

        private void Update()
        {
            var cameraPos = cameraRig.transform.position;
            var cameraBounds = cameraRig.Bounds;
            var delta = cameraPos.x - lastCameraX;
            lastCameraX = cameraPos.x;
            if (delta < 0)
            {
                var room = rooms[rightMost];
                if (room.worldMin.x > cameraBounds.max.x)
                {
                    var p = room.transform.position;
                    p.x -= circumference;
                    room.transform.position = p;
                    room.CalculateExtents();
                    leftMost = rightMost;
                    rightMost = (rightMost + rooms.Length - 1) % rooms.Length;
                }
            } else if (delta > 0)
            {
                var room = rooms[leftMost];
                if (room.worldMax.x < cameraBounds.min.x)
                {
                    var p = room.transform.position;
                    p.x += circumference;
                    room.transform.position = p;
                    room.CalculateExtents();
                    rightMost = leftMost;
                    leftMost = (leftMost + 1) % rooms.Length;
                }
            }
            
        }
    }
}