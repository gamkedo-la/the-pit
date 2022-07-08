using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public class CreditsController
    {
        private readonly Button closeButton;
        private readonly Label creditDisplay;
        private readonly VisualElement buttonHolder;

        public CreditsController(VisualElement root, VisualTreeAsset creditButtonTemplate, string[] creditNames,
            string[] creditContents)
        {
            closeButton = root.Q<Button>("CloseButton");
            buttonHolder = root.Q<VisualElement>("NameButtons");
            creditDisplay = root.Q<Label>("Credits");
            
            for (var i = 0; i < creditNames.Length; i++)
            {
                var buttonElement = creditButtonTemplate.Instantiate();
                var button = buttonElement.Q<Button>();
                button.text = creditNames[i];
                var creditDisplayText = creditContents[i];
                button.clicked += () => { creditDisplay.text = creditDisplayText; };
                buttonHolder.Add(buttonElement);
            }
        }

        public void SetCloseAction(UnityEvent closeEvent)
        {
            closeButton.clicked += closeEvent.Invoke;
        }
    }
}