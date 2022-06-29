using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Variables;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicSource : MonoBehaviour
    {
        public FloatVariable mainMusicVolume;

        [FormerlySerializedAs("fadeDuration")]
        [Header("Fade")] 
        [Min(0)]
        public float fadeOutDuration;

        [Min(0)] 
        public float fadeInDuration;
        
        public AnimationCurve fadeCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private AudioSource audioSource;
        private int fadeDirection = 0;
        private float fadePoint = 1f;
        private float fadeTotalTime = 1f;
        private float fadeVolumeMultiplier;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private IEnumerator FadeOut(float duration)
        {
            if (duration <= 0)
            {
                fadePoint = 0;
                fadeDirection = 0;
                yield break;
            }
            
            fadeDirection = -1;
            fadeTotalTime = duration;

            while (fadeDirection != 0)
            {
                yield return null;
            }
        }

        private IEnumerator FadeIn(float duration)
        {
            if (duration <= 0)
            {
                fadePoint = 1;
                fadeDirection = 0;
                yield break;
            }

            fadeDirection = 1;
            fadeTotalTime = duration;

            while (fadeDirection != 0)
            {
                yield return null;
            }
        }

        private IEnumerator SwitchTo(AudioClipWithVolume newClip)
        {
            yield return FadeOut(fadeOutDuration);
            audioSource.Stop();
            audioSource.PlayOneShot(newClip);
            yield return FadeIn(fadeInDuration);
        }

        public void Play(AudioClipWithVolume clip)
        {
            StartCoroutine(SwitchTo(clip));
        }

        private void AdjustVolume()
        {
            audioSource.volume = mainMusicVolume.Value * fadeVolumeMultiplier;
        }

        private void CalculateFade()
        {
            if (fadeDirection == 0) return;
            
            fadePoint = Mathf.Clamp01(fadePoint + fadeDirection * Time.deltaTime / fadeTotalTime);
            fadeVolumeMultiplier = fadeCurve.Evaluate(fadePoint);
            if (fadeDirection == -1 && fadePoint <= 0) fadeDirection = 0;
            if (fadeDirection == 1 && fadePoint >= 1) fadeDirection = 0;
        }
        
        private void Update()
        {
            CalculateFade();
            AdjustVolume();
        }
    }
}