using UnityEngine.Events;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class MissionBriefingController
    {
        private readonly Label instructionsLabel;
        private readonly Label briefingLabel;
        private readonly Toggle musicToggle;
        private readonly Button closeButton;

        public MissionBriefingController(VisualElement root)
        {
            instructionsLabel = root.Q<Label>("Instructions");
            briefingLabel = root.Q<Label>("Briefing");
            musicToggle = root.Q<Toggle>("MusicToggle");
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

        public void SetMusicToggle(BoolVariable toggle)
        {
            musicToggle.value = toggle.Value;
            musicToggle.RegisterValueChangedCallback(evt => toggle.Value = evt.newValue);
        }

        public void SetCloseAction(UnityEvent closeEvent)
        {
            closeButton.clicked += closeEvent.Invoke;
        }
    }
}