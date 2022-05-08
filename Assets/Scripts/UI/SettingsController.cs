using Graphics;
using UnityEngine;
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
            backgroundBrightnessSlider.value =
                PlayerPrefs.GetFloat(BackgroundBrightness.PrefsKey,
                    backgroundBrightnessSlider.value / backgroundBrightnessSlider.highValue) *
                backgroundBrightnessSlider.highValue;
            
            SetAmountLabel(backgroundBrightnessSlider.value);
        }

        public void RegisterCallbacks(UnityEvent<float> onBackgroundBrightnessChanged)
        {
            backgroundBrightnessSlider.RegisterValueChangedCallback(evt =>
            {
                SetAmountLabel(evt.newValue);
                onBackgroundBrightnessChanged.Invoke(evt.newValue / backgroundBrightnessSlider.highValue);
            });
        }

        private void SetAmountLabel(float amount)
        {
            amountLabel.text = $"{amount:0}";
        }
    }
}