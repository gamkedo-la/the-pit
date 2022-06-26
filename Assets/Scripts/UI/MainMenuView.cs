using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    public class MainMenuView : MonoBehaviour
    {
        public UnityEvent onPlayGame;
        public UnityEvent onShowSettings;
        public UnityEvent onExitGame;

        private MainMenuController controller;

        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            controller = new MainMenuController(ui.rootVisualElement,
                onPlayGame, onShowSettings, onExitGame);
        }

        public void ExitGame()
        {
            #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
            #else
            Application.Quit();
            #endif
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }
    }

    public class MainMenuController
    {
        public MainMenuController(VisualElement root, UnityEvent playGameEvent, UnityEvent showSettingsEvent, UnityEvent exitGameEvent)
        {
            root.Q<Button>("Play").clicked += playGameEvent.Invoke;
            root.Q<Button>("Settings").clicked += showSettingsEvent.Invoke;
            root.Q<Button>("Exit").clicked += exitGameEvent.Invoke;
        }
    }
}