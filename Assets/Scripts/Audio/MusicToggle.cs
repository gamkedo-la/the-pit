using System;
using UnityEngine;
using Variables;

namespace Audio
{
    public class MusicToggle : MonoBehaviour
    {
        [SerializeField] private BoolVariable playBackgroundMusic;
        [SerializeField] private AudioSource musicSource;

        private bool PlayingMusic
        {
            get => playBackgroundMusic.Value;
            set => playBackgroundMusic.Value = value;
        }

        private void Start()
        {
            musicSource.mute = !playBackgroundMusic.Value;
        }

        void Update()
        {
            if (PlayingMusic && Input.GetKeyDown(KeyCode.M))
            {
                MuteMusicAudio();
            }

            else if (!PlayingMusic && Input.GetKeyDown(KeyCode.M))
            {
                UnMuteMusicAudio();
            }
        }

        private void MuteMusicAudio()
        {
            PlayingMusic = false;
            musicSource.mute = true;
        }

        private void UnMuteMusicAudio()
        {
            PlayingMusic = true;
            musicSource.mute = false;
        }
    }
}