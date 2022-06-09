using UnityEngine;

namespace Audio
{
    public static class AudioExtensions
    {
        public static void PlayOneShot(this AudioSource audioSource, AudioClipWithVolume clip)
        {
            audioSource.PlayOneShot(clip.clip, clip.volumeScale);
        }
        
    }
}