using System.Linq;
using Channels;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace UI
{
    public class CreditsView : MonoBehaviour
    {
        public TextAsset[] credits;
        public BoolChannel gameRunning;

        public VisualTreeAsset creditButtonTemplate;

        public UnityEvent onClose;

        private void OnEnable()
        {
            gameRunning.Push(false);
            var ui = GetComponent<UIDocument>();
            var creditNames = credits.Select(c => c.text.Split("\n")[0]).ToArray();
            var creditContents = credits.Select(c => c.text).ToArray();
            var controller = new CreditsController(ui.rootVisualElement, creditButtonTemplate, creditNames, creditContents);
            controller.SetCloseAction(onClose);
        }

        private void OnDisable()
        {
            gameRunning.Push(true);
        }
    }
}