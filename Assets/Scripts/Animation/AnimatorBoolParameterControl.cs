using UnityEngine;
using Variables;

namespace Animation
{
    public class AnimatorBoolParameterControl : MonoBehaviour
    {
        public Animator animator;
        public string parameterName;
        public BoolVariable variable;

        private void Start()
        {
            animator.SetBool(parameterName, variable.Value);
        }
    }
}