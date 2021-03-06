using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Variables;

namespace Level
{
    public class Room : MonoBehaviour
    {
        [HideInInspector]
        public Vector3 worldMin;
        [HideInInspector]
        public Vector3 worldMax;

        [Tooltip("Room name. Leave blank to use name of GameObject as room name")]
        [SerializeField]
        private string nameOverride;

        [Tooltip("Health bars to show when inside this room")]
        [SerializeField]
        private List<HealthVariable> healthBars;

        public string Name => string.IsNullOrEmpty(nameOverride) ? name : nameOverride;
        public bool Dirty { get; private set; }
        public IEnumerable<HealthVariable> HealthBars => healthBars;

        public void AddHealthBar(HealthVariable healthVariable)
        {
            if (healthBars.Contains(healthVariable)) return;
            
            healthBars.Add(healthVariable);
            Dirty = true;
        }

        public void RemoveHealthBar(HealthVariable healthVariable)
        {
            if (!healthBars.Contains(healthVariable)) return;

            healthBars.Remove(healthVariable);
            Dirty = true;
        }

        public void TranslateX(int step)
        {
            transform.Translate(new(step, 0, 0));
        }
        
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