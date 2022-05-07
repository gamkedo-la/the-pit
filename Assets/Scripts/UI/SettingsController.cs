using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public class SettingsController
    {
        private Slider backgroundBrightnessSlider;
        private Label amountLabel;

        public SettingsController(VisualElement root)
        {
            backgroundBrightnessSlider = root.Q<Slider>("BackgroundBrightness");
            amountLabel = backgroundBrightnessSlider.Q<Label>();
        }

        public void RegisterCallbacks(UnityEvent<float> onBackgroundBrightnessChanged)
        {
            backgroundBrightnessSlider.RegisterValueChangedCallback(evt =>
            {
                amountLabel.text = $"{evt.newValue:0}";
                onBackgroundBrightnessChanged.Invoke(evt.newValue / backgroundBrightnessSlider.highValue);
            });
        }
    }
}