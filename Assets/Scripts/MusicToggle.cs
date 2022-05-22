using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MusicToggle : MonoBehaviour
{
    Toggle musicToggle;
    void Start()
    {
        musicToggle = GetComponent<Toggle>();
    }

    void Update()
    {
        if (musicToggle.isOn == true && Input.GetKeyDown(KeyCode.M))
        {
            MuteMusicAudio();
        }

        else if (musicToggle.isOn == false && Input.GetKeyDown(KeyCode.M))
        {

            UnMuteMusicAudio();

        }

    }

    private void MuteMusicAudio()
    {
        musicToggle.isOn = false;
    }

    private void UnMuteMusicAudio()
    {
        musicToggle.isOn = true;
    }
}
