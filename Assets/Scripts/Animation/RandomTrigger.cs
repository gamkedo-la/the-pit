using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class RandomTrigger : MonoBehaviour
    {
        public Animator animator;
        public TriggerProbability[] triggers;

        private void FixedUpdate()
        {
            foreach (var tp in triggers)
            {
                if (Mathf.FloorToInt(Random.value * 60f * 100f) <= tp.probabilityPercent)
                {
                    animator.SetTrigger(tp.trigger);
                    break;
                }
            }
        }

        [Serializable]
        public struct TriggerProbability
        {
            public string trigger;
            [Range(0, 100)]
            public int probabilityPercent;
        }
        
    }
}