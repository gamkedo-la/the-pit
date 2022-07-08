using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Variables;

namespace UI
{
    public class MainMenuView : MonoBehaviour
    {
        public BoolVariable hasSeenIntro;
        public BoolVariable skipIntro;
        
        public UnityEvent onPlayGame;
        public UnityEvent onShowSettings;
        public UnityEvent onShowCredits;
        public UnityEvent onExitGame;

        private MainMenuController controller;

        private void OnEnable()
        {
            var ui = GetComponent<UIDocument>();
            controller = new MainMenuController(ui.rootVisualElement,
                onPlayGame, onShowSettings, onShowCredits, onExitGame);

            if (hasSeenIntro.Value) return;
            // First time we see the intro, now set skip intro to true to disable it by default
            hasSeenIntro.Value = true;
            skipIntro.Value = true;
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
        public MainMenuController(VisualElement root, UnityEvent playGameEvent, UnityEvent showSettingsEvent, UnityEvent showCreditsEvent, UnityEvent exitGameEvent)
        {
            root.Q<Button>("Play").clicked += playGameEvent.Invoke;
            root.Q<Button>("Settings").clicked += showSettingsEvent.Invoke;
            root.Q<Button>("Credits").clicked += showCreditsEvent.Invoke;
            var exitButton = root.Q<Button>("Exit");
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                exitButton.RemoveFromHierarchy();
            }
            else
            {
                exitButton.clicked += exitGameEvent.Invoke;
            }
        }
    }
}