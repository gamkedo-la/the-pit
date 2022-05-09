using UnityEngine;
using UnityEngine.Tilemaps;

namespace Level
{
    public class Room : MonoBehaviour
    {
        public Vector3 worldMin;
        public Vector3 worldMax;

        [ContextMenu("Calculate Extents")]
        public void CalculateExtents()
        {
            worldMax = Vector3.negativeInfinity;
            worldMin = Vector3.positiveInfinity;
            var tilemaps = GetComponentsInChildren<Tilemap>();
            foreach (var tilemap in tilemaps)
            {
                tilemap.CompressBounds();
                var cellBounds = tilemap.cellBounds;
                worldMax = Vector3.Max(worldMax, tilemap.CellToWorld(cellBounds.max));
                worldMin = Vector3.Min(worldMin, tilemap.CellToWorld(cellBounds.min));
            }
        }

    }
}