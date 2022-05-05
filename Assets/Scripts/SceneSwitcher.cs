using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene("ResolutionTest");
        }
    }
}