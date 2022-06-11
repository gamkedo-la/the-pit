using Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class ActionView : MonoBehaviour
    {
        public PlayerActionController playerActionController;
        
        private ActionController actionController;
        
        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            actionController = new ActionController(ui.rootVisualElement, "E");
        }

        private void LateUpdate()
        {
            if (playerActionController.Interaction == null) actionController.ClearAction();
            else actionController.ShowAction(playerActionController.Interaction.actionDescription);
        }
    }
}