using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public class MissionBriefingController
    {
        private readonly Label instructionsLabel;
        private readonly Label briefingLabel;
        private readonly Button closeButton;

        public MissionBriefingController(VisualElement root)
        {
            instructionsLabel = root.Q<Label>("Instructions");
            briefingLabel = root.Q<Label>("Briefing");
            closeButton = root.Q<Button>("CloseButton");
        }

        public void SetBriefingText(string text)
        {
            briefingLabel.text = text;
        }

        public void SetInstructionsText(string text)
        {
            instructionsLabel.text = text;
        }

        public void SetCloseAction(UnityEvent closeEvent)
        {
            closeButton.clicked += closeEvent.Invoke;
        }
    }
}