using Level;
using UnityEngine.UIElements;

namespace UI
{
    public class LocationController
    {
        private readonly Label locationLabel;

        public LocationController(VisualElement root)
        {
            locationLabel = root.Q<Label>("Location");
        }

        public void ShowLocation(Room room)
        {
            locationLabel.text = room.Name;
        }

    }
}