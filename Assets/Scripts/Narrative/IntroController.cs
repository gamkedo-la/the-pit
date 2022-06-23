using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Narrative
{
    public class IntroController : MonoBehaviour
    {
        public TimedAction[] actions;

        private IEnumerator Start()
        {
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

        [Serializable]
        public class TimedAction
        {
            public float time;
            public UnityEvent action;
        }
    }
}