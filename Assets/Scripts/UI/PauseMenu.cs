using Channels;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject btn1;
    [SerializeField] GameObject btn2;

    public BoolChannel gameRunning;

    private void Start()
    {
        btn1.SetActive(false);
        btn2.SetActive(false);

        gameRunning.Push(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            btn1.SetActive(true);
            btn2.SetActive(true);
            gameRunning.Push(false);
        }
    }

    public void OnResume()
    {
        btn1.SetActive(false);
        btn2.SetActive(false);
        gameRunning.Push(true);
    }

    public void OnQuite()
    {
        Application.Quit();
    }
}
