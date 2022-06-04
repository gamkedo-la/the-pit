using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject btn1;
    [SerializeField] GameObject btn2;


    private void Start()
    {
        btn1.SetActive(false);
        btn2.SetActive(false);


    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            btn1.SetActive(true);
            btn2.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void OnResume()
    {
        btn1.SetActive(false);
        btn2.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnQuite()
    {
        Application.Quit();
    }
}
