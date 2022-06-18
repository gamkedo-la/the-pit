using UnityEngine;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class PlayerHealthView : MonoBehaviour
    {
        public MinMaxIntVariable currentHealth;
        public VisualTreeAsset heartTemplate;
        
        private PlayerHealthController playerHealthController;
        private int lastDisplayedHealth;
        
        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            playerHealthController = new (ui.rootVisualElement, heartTemplate);
            lastDisplayedHealth = int.MinValue;
        }

        private void Update()
        {
            if (currentHealth.Value == lastDisplayedHealth) return;
            
            playerHealthController.ShowHealth(currentHealth.Value, currentHealth.MaxValue);
            lastDisplayedHealth = currentHealth.Value;
        }
    }
}