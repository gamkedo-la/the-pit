using UnityEngine.UIElements;

namespace UI
{
    public class PlayerHealthController
    {
        private readonly VisualElement heartContainer;
        private readonly VisualTreeAsset heartTemplate;

        public PlayerHealthController(VisualElement root, VisualTreeAsset template)
        {
            heartContainer = root.Q<VisualElement>("HealthBar");
            heartTemplate = template;
        }

        public void ShowHealth(int value)
        {
            heartContainer.Clear();
            if (value <= 0) return;
            
            var hearts = value / 8;
            for (var i = 0; i < hearts; i++)
            {
                var fullHeart = heartTemplate.Instantiate();
                fullHeart.AddToClassList("heart-0");
                heartContainer.Add(fullHeart);
            }

            var remaining = value % 8;
            if (remaining <= 0) return;

            var partialHeart = heartTemplate.Instantiate();
            partialHeart.AddToClassList($"heart-{remaining}");
            heartContainer.Add(partialHeart);
        }
    }
}