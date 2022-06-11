using UnityEngine.UIElements;

namespace UI
{
    public class ActionController
    {
        private readonly Label actionLabel;
        private readonly string actionButtonName;

        public ActionController(VisualElement root, string actionButtonName)
        {
            this.actionButtonName = actionButtonName;
            actionLabel = root.Q<Label>("Action");
        }

        public void ClearAction()
        {
            actionLabel.text = "";
        }

        public void ShowAction(string actionDescription)
        {
            actionLabel.text = $"[{actionButtonName}] {actionDescription}";
        }
    }
}