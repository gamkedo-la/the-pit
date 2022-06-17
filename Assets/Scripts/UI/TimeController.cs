using UnityEngine;

namespace UI
{
    public class TimeController : MonoBehaviour
    {
        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
        }
    }
}