using Channels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class MissionBriefingView : MonoBehaviour
    {
        public TextAsset instructions;
        public TextAsset briefing;
        public BoolChannel gameRunning;
        public FloatVariable musicVolume;

        public UnityEvent onClose;

        private void OnEnable()
        {
            gameRunning.Push(false);
            var ui = GetComponent<UIDocument>();
            var controller = new MissionBriefingController(ui.rootVisualElement);
            controller.SetInstructionsText(instructions.text);
            controller.SetBriefingText(briefing.text);
            controller.SetMusicVolume(musicVolume);
            controller.SetCloseAction(onClose);
        }

        private void OnDisable()
        {
            gameRunning.Push(true);
        }
    }
}