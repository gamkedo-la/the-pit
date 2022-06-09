using UnityEngine;

namespace Audio
{
    [CreateAssetMenu]
    public class AudioClipWithVolume : ScriptableObject
    {
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volumeScale = 1;
    }
}