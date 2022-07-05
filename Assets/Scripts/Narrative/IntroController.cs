using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Variables;

namespace Narrative
{
    public class IntroController : MonoBehaviour
    {
        public TimedAction[] actions;
        public BoolVariable skipIntro;
        public KeyCode skipIntroKey;
        public UnityEvent onSkipIntro;

        private IEnumerator Start()
        {
            if (skipIntro.Value)
            {
                onSkipIntro.Invoke();
                yield break;
            }
            
            var sortedActions = new List<TimedAction>(actions);
            sortedActions.Sort((ta1, ta2) => ta2.time.CompareTo(ta1.time));

            while (sortedActions.Count > 0)
            {
                var next = sortedActions[^1];
                sortedActions.RemoveAt(sortedActions.Count-1);

                var wait = next.time - Time.time;
                if (wait > 0) yield return new WaitForSeconds(wait);
                next.action.Invoke();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(skipIntroKey))
            {
                StopAllCoroutines();
                onSkipIntro.Invoke();
                Destroy(this);
            }
        }

        [Serializable]
        public class TimedAction
        {
            public float time;
            public UnityEvent action;
        }
    }
}