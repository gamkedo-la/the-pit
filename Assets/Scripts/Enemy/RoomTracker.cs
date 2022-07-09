using Level;
using UnityEngine;

namespace Enemy
{
    public class RoomTracker : MonoBehaviour
    {
        public PitLevel level;

        private void Start()
        {
            if (level == null)
            {
                level = FindObjectOfType<PitLevel>();
            }
        }

        private void LateUpdate()
        {
            if (!level.FindRoom(transform.position, out var room)) return;

            if (room.transform != transform.parent)
            {
                transform.parent = room.transform;
            }
        }
    }
}