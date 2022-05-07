using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class FPSController
    {
        private readonly Label fpsLabel;
        
        private float elapsed;
        private int frames;
        
        public FPSController(VisualElement root)
        {
            fpsLabel = root.Q<Label>("FPS");
            ShowFPS(0);
        }

        public void AddFrameTime(float frameTime)
        {
            frames++;
            elapsed += frameTime;

            if (!(elapsed > 1f)) return;
            
            var fps = Mathf.RoundToInt(frames / elapsed);
            frames = 0;
            elapsed = 0;
            ShowFPS(fps);
        }

        private void ShowFPS(int fps)
        {
            fpsLabel.text = $"FPS: {fps}";
        }
    }
}