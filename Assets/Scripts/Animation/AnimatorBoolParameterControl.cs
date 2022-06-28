using System.Collections;
using Conditions;
using UnityEngine;
using Variables;

namespace Animation
{
    public class AnimatorBoolParameterControl : MonoBehaviour
    {
        public Animator animator;
        public string parameterName;
        public BoolVariable variable;
        public Condition condition;
        public bool update;

        private IEnumerator Start()
        {
            SetParameter();
            while (update)
            {
                yield return null;
                SetParameter();
            }
        }

        private void SetParameter()
        {
            animator.SetBool(parameterName, variable != null ? variable.Value : condition.Evaluate());
        }
    }
}