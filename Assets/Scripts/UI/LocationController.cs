using Level;
using UnityEngine.UIElements;

namespace UI
{
    public class LocationController
    {
        private readonly Label locationLabel;
        private readonly VisualElement topBar;
        private readonly VisualTreeAsset enemyHealthBarTemplate;
        private Room currentRoom;

        public LocationController(VisualElement root, VisualTreeAsset enemyHealthBarTemplate)
        {
            this.enemyHealthBarTemplate = enemyHealthBarTemplate;
            locationLabel = root.Q<Label>("Location");
            topBar = root.Q<VisualElement>("TopBar");
        }

        public void ShowLocation(Room room)
        {
            if (room == currentRoom) return;

            currentRoom = room;
            locationLabel.text = room.Name;
            
            topBar.Clear();
            foreach (var healthVariable in room.healthBars)
            {
                if (healthVariable.Health <= 0) continue;
                
                var enemyHealthBar = enemyHealthBarTemplate.Instantiate();
                var controller = new EnemyHealthBarController(enemyHealthBar, healthVariable);
                enemyHealthBar.userData = controller;
                topBar.Add(enemyHealthBar);
            }
        }

        public void Update()
        {
            foreach (var element in topBar.Children())
            {
                if (element.userData is not EnemyHealthBarController controller) continue;
                controller.Update();
                if (controller.IsDepleted())
                {
                    // Forces a refresh next frame, which will remove depleted health bars
                    currentRoom = null;
                }
            }
        }
    }
}