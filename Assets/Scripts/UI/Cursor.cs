using UnityEngine;

namespace UI
{
    [CreateAssetMenu]
    public class Cursor : ScriptableObject
    {
        public Texture2D texture;
        public Vector2 offset;
        public CursorMode cursorMode = CursorMode.Auto;

        public void Set()
        {
            UnityEngine.Cursor.SetCursor(texture, offset, cursorMode);
        }
    }
}