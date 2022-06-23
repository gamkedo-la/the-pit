using UnityEngine;

namespace Animation
{
    public class AnimatorFloatParameterControl : MonoBehaviour
    {
        public Animator animator;
        public string parameter;
        public float value;

        private void Update()
        {
            animator.SetFloat(parameter, value);
        }
    }
}