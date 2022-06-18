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

        public void ShowHealth(int value, int maxValue)
        {
            heartContainer.Clear();
            var totalHearts = (maxValue+7) / 8;
            for (var i = 0; i < totalHearts; i++)
            {
                var heart = heartTemplate.Instantiate();
                heart.AddToClassList(value switch
                {
                    <= 0 => "heart-empty",
                    < 8 => $"heart-{value}",
                    _ => "heart-full"
                });
                heartContainer.Add(heart);
                value -= 8;
            }
        }
    }
}