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

        private Label diagnosticInfoLabel;

        public SettingsController(VisualElement root)
        {
            backgroundBrightnessSlider = root.Q<Slider>("BackgroundBrightness");
            amountLabel = backgroundBrightnessSlider.Q<Label>();
            backgroundBrightnessSlider.value = BackgroundBrightness.Value * backgroundBrightnessSlider.highValue;
            
            SetAmountLabel(backgroundBrightnessSlider.value);

            diagnosticInfoLabel = root.Q<Label>("DiagnosticInfo");
            diagnosticInfoLabel.text =
                $"{Application.productName} {Application.version} | {Application.unityVersion} | {SystemInfo.operatingSystemFamily} | {SystemInfo.operatingSystem}";
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