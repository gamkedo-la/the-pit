using UnityEngine;
using UnityEngine.Events;

namespace Animation
{
    public class ColorFluctuation : MonoBehaviour
    {
        [Tooltip("Gradient to vary color along. For best result, make sure it ends with the same color as it starts with.")]
        public Gradient gradient;
        [Tooltip("Number of seconds it takes to fluctuate across entire gradient")]
        public float duration;
        [Tooltip("Function(s) to call when the color changes")]
        public UnityEvent<Color> onValueChange;

        private float elapsedTime;

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > duration)
            {
                elapsedTime -= duration;
            }

            var t = elapsedTime / duration;
            var color = gradient.Evaluate(t);
        
            onValueChange.Invoke(color);
        }
    }
}