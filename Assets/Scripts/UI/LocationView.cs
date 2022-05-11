using Level;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class LocationView : MonoBehaviour
    {
        public PitLevel level;
        public Transform player;
        
        private LocationController locationController;
        
        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            locationController = new LocationController(ui.rootVisualElement);
        }

        private void LateUpdate()
        {
            if (level.FindRoom(player.position, out var room))
            {
                locationController.ShowLocation(room);
            }
        }
    }
}