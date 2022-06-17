using Channels;
using UnityEngine;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class MissionBriefingView : MonoBehaviour
    {
        public TextAsset instructions;
        public TextAsset briefing;
        public BoolVariable showOnStartup;
        public BoolChannel gameRunning;

        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            var controller = new MissionBriefingController(ui.rootVisualElement);
            controller.SetInstructionsText(instructions.text);
            controller.SetBriefingText(briefing.text);
            controller.SetShowOnStartup(showOnStartup);
            gameRunning.Push(false);
        }

        private void OnDisable()
        {
            gameRunning.Push(true);
        }
    }
}