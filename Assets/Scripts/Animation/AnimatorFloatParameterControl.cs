using UnityEngine;

namespace Animation
{
    public class AnimatorFloatParameterControl : MonoBehaviour
    {
        public Animator animator;
        public string parameter;
        public float value;
        public bool random;
        public float randomVariance;

        private float Value => random ? Random.Range(value - randomVariance, value + randomVariance) : value;
        
        private void Update()
        {
            animator.SetFloat(parameter, Value);
        }
    }
}