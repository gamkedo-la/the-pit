using UnityEngine.UIElements;

namespace UI
{
    public class MissionBriefingController
    {
        private readonly Label textLabel;

        public MissionBriefingController(VisualElement root)
        {
            textLabel = root.Q<Label>("Briefing");
        }

        public void SetBriefingText(string text)
        {
            textLabel.text = text;
        }
    }
}