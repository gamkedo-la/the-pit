using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public GameObject toggleTarget;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            toggleTarget.SetActive(!toggleTarget.activeSelf);
        }
    }
}