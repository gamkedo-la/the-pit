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

        private void Update()
        {
            switch (PlayingMusic)
            {
                case true when Input.GetKeyDown(KeyCode.M):
                    MuteMusicAudio();
                    break;
                case false when Input.GetKeyDown(KeyCode.M):
                    UnMuteMusicAudio();
                    break;
                default:
                    if (musicSource.mute == PlayingMusic)
                    {
                        musicSource.mute = !PlayingMusic;
                    }

                    break;
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