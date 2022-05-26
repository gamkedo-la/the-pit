using UnityEngine.UIElements;

namespace UI
{
    public class MissionBriefingController
    {
        private readonly Label instructionsLabel;
        private readonly Label briefingLabel;

        public MissionBriefingController(VisualElement root)
        {
            instructionsLabel = root.Q<Label>("Instructions");
            briefingLabel = root.Q<Label>("Briefing");
        }

        public void SetBriefingText(string text)
        {
            briefingLabel.text = text;
        }

        public void SetInstructionsText(string text)
        {
            instructionsLabel.text = text;
        }
    }
}