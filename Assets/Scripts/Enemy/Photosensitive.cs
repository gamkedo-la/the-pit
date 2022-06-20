using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public class Photosensitive : MonoBehaviour
    {
        public UnityEvent<Vector2> onEnterLight;
        public UnityEvent<Vector2> onExitLight;

        public void EnterLight(Vector2 lightSource)
        {
            onEnterLight.Invoke(lightSource);
        }

        public void ExitLight(Vector2 lightSource)
        {
            onExitLight.Invoke(lightSource);
        }
        
    }
}