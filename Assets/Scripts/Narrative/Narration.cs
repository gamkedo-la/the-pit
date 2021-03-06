using UnityEngine;

namespace Narrative
{
    [CreateAssetMenu]
    public class Narration : ScriptableObject
    {
        [TextArea]
        public string text;
        public AudioClip audioClip;
        [Min(0)]
        public float defaultDuration;

        [Header("Sequence")] 
        public Narration next;
        [Min(0)]
        public float delayUntilNext;
    }
}