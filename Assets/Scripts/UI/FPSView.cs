using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FPSView : MonoBehaviour
    {
        public UIDocument ui;
        private FPSController controller;
        private void OnEnable()
        {
            controller = new(ui.rootVisualElement);
        }

        private void Update()
        {
            controller.AddFrameTime(Time.deltaTime);
        }
    }
}