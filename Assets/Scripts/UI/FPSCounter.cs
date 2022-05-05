using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class FPSCounter : MonoBehaviour
    {
        [Min(1)]
        public float updateFrequency = 1;
    
        private Text text;
        private float lastFrameTime;
        private int frames = 0;
    
        void Start()
        {
            text = GetComponent<Text>();
            lastFrameTime = Time.time;
            frames = 0;
        }

        void Update()
        {
            frames++;
            var currTime = Time.time;
            var elapsed = currTime - lastFrameTime;

            if (elapsed > 1f / updateFrequency)
            {
                var fps = Mathf.RoundToInt(frames / elapsed);
                frames = 0;
                lastFrameTime = currTime;
                text.text = $"FPS: {fps}";
            }
        }
    }
}
