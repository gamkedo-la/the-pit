using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Util
{
    public class TriggerDelayed : MonoBehaviour
    {
        public UnityEvent onTrigger;
        
        public void SetActive(float delay)
        {
            StartCoroutine(DelayTrigger(delay));
        }

        private IEnumerator DelayTrigger(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            onTrigger.Invoke();
        }
    }
}