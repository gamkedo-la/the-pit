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

        private Room[] rooms;
        private int leftMost, rightMost;

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

        public bool FindRoom(Vector2 worldPosition, out Room room)
        {
            foreach (var r in rooms)
            {
                Vector2 min = r.worldMin;
                if (min.x > worldPosition.x || min.y > worldPosition.y) continue;
                Vector2 max = r.worldMax;
                if (max.x < worldPosition.x || max.y < worldPosition.y) continue;
                room = r;
                return true;
            }

            room = default;
            return false;
        }

        public void UpdateRooms(CameraRig cameraRig)
        {
            var cameraBounds = cameraRig.Bounds;

            if (cameraBounds.min.x < rooms[leftMost].worldMin.x)
            {
                // Empty space to the left of the camera. Move rightmost room to the left edge
                rooms[rightMost].TranslateX(-circumference);
                rooms[rightMost].CalculateExtents();
                leftMost = rightMost;
                rightMost = (leftMost + rooms.Length - 1) % rooms.Length;
            }

            if (cameraBounds.max.x > rooms[rightMost].worldMax.x)
            {
                // Empty space to the right of the camera. Move leftmost room to the right edge
                rooms[leftMost].TranslateX(circumference);
                rooms[leftMost].CalculateExtents();
                rightMost = leftMost;
                leftMost = (rightMost + 1) % rooms.Length;
            }
        }
    }
}