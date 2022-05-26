using System;
using UnityEngine;
using Variables;

public class UIToggler : MonoBehaviour
{
    public KeyAndTarget[] toggles;

    private void Awake()
    {
        foreach (var keyAndTarget in toggles)
        {
            if (keyAndTarget.defaultState == null) continue;
            
            var shouldBeActive = keyAndTarget.defaultState.Value;
            keyAndTarget.target.SetActive(shouldBeActive);
        }
    }

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
        public BoolVariable defaultState;
    }
}