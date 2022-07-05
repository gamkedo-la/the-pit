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
        private readonly Toggle skipIntroToggle;
        private readonly Button closeButton;

        public MissionBriefingController(VisualElement root)
        {
            instructionsLabel = root.Q<Label>("Instructions");
            briefingLabel = root.Q<Label>("Briefing");
            musicToggle = root.Q<Toggle>("MusicToggle");
            skipIntroToggle = root.Q<Toggle>("SkipIntro");
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

        public void SetMusicVolume(FloatVariable musicVolume)
        {
            musicToggle.value = musicVolume.Value > 0;
            musicToggle.RegisterValueChangedCallback(evt => musicVolume.Value = evt.newValue ? 1 : 0);
        }

        public void SetSkipIntro(BoolVariable skipIntro)
        {
            skipIntroToggle.value = skipIntro.Value;
            skipIntroToggle.RegisterValueChangedCallback(evt => skipIntro.Value = evt.newValue);
        }
    }
}