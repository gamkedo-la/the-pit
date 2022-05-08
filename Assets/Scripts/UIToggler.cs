using System;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    public KeyAndTarget[] toggles;
    
    private void Update()
    {
        foreach (var keyAndTarget in toggles)
        {
            if (Input.GetKeyDown(keyAndTarget.key))
            {
                keyAndTarget.target.SetActive(!keyAndTarget.target.activeSelf);
            }
        }
    }

    [Serializable]
    public struct KeyAndTarget
    {
        public KeyCode key;
        public GameObject target;
    }
}