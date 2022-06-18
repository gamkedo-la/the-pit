using Player;
using UnityEngine;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class ActionView : MonoBehaviour
    {
        public GameObjectVariable player;
        
        private ActionController actionController;
        
        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            actionController = new ActionController(ui.rootVisualElement, "E");
        }

        private void LateUpdate()
        {
            if (player.Value == null) return;
            var playerActionController = player.Value.GetComponent<PlayerActionController>();
            if (playerActionController.Interaction == null || string.IsNullOrEmpty(playerActionController.Interaction.actionDescription)) actionController.ClearAction();
            else actionController.ShowAction(playerActionController.Interaction.actionDescription);
        }
    }
}