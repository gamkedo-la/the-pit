using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public class SettingsView : MonoBehaviour
    {
        public GameBuildInfo gameBuildInfo;
        public UnityEvent<float> onBackgroundBrightnessChanged;
        private SettingsController controller;
        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            controller = new(ui.rootVisualElement, gameBuildInfo);
            controller.RegisterCallbacks(onBackgroundBrightnessChanged);
        }
    }
}