using UnityEngine;

namespace Animation
{
    public class RandomRotation : MonoBehaviour
    {
        public AnimationCurve angleProbability = AnimationCurve.Linear(0, 0, 1, 360);

        private void OnEnable()
        {
            transform.Rotate(0, 0, angleProbability.Evaluate(Random.value));
        }
    }
}
