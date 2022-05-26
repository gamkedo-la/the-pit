using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class MissionBriefingController
    {
        private readonly Label instructionsLabel;
        private readonly Label briefingLabel;
        private readonly Toggle showOnStartupToggle;

        public MissionBriefingController(VisualElement root)
        {
            instructionsLabel = root.Q<Label>("Instructions");
            briefingLabel = root.Q<Label>("Briefing");
            showOnStartupToggle = root.Q<Toggle>("ShowOnStartup");
        }

        public void SetBriefingText(string text)
        {
            briefingLabel.text = text;
        }

        public void SetInstructionsText(string text)
        {
            instructionsLabel.text = text;
        }

        public void SetShowOnStartup(BoolVariable showOnStartup)
        {
            showOnStartupToggle.value = showOnStartup.Value;
            showOnStartupToggle.RegisterValueChangedCallback((evt) => showOnStartup.Value = evt.newValue);
        }
    }
}