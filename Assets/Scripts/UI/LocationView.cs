using Level;
using UnityEngine;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class LocationView : MonoBehaviour
    {
        public PitLevel level;
        public GameObjectVariable player;
        public VisualTreeAsset enemyHealthBarTemplate;
        
        private LocationController locationController;
        
        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            locationController = new LocationController(ui.rootVisualElement, enemyHealthBarTemplate);
        }

        private void LateUpdate()
        {
            if (player.Value == null) return;
            
            if (level.FindRoom(player.Value.transform.position, out var room))
            {
                locationController.ShowLocation(room);
            }

            locationController.Update();
        }
    }
}