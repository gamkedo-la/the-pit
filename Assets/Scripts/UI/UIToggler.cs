using System;
using UnityEngine;
using Variables;

public class UIToggler : MonoBehaviour
{
    public UI.Cursor defaultCursor;
    public KeyAndTarget[] toggles;

    private void Awake()
    {
        defaultCursor.Set();
        foreach (var keyAndTarget in toggles)
        {
            keyAndTarget.SetToDefaultState();
        }
    }

    private void Update()
    {
        foreach (var keyAndTarget in toggles)
        {
            if (keyAndTarget.IsKeyDown)
            {
                defaultCursor.Set();
                keyAndTarget.SetActive(!keyAndTarget.IsActive);
            }
        }
    }

    [Serializable]
    public struct KeyAndTarget
    {
        [SerializeField]
        private KeyCode key;
        [SerializeField]
        private GameObject target;
        [SerializeField]
        private BoolVariable defaultState;
        [SerializeField]
        private UI.Cursor cursor;

        public bool IsActive => target.activeSelf;
        public bool IsKeyDown => Input.GetKeyDown(key);

        public void SetActive(bool active)
        {
            target.SetActive(active);
            if (active && cursor != null) cursor.Set();
        }

        public void SetToDefaultState()
        {
            if (defaultState == null) return;
            SetActive(defaultState.Value);
        }

    }
}