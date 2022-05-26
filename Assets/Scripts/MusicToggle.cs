using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    private bool PlayingMusic
    {
        get => !musicSource.mute;
        set => musicSource.mute = !value;
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
    }

    private void UnMuteMusicAudio()
    {
        PlayingMusic = true;
    }
}