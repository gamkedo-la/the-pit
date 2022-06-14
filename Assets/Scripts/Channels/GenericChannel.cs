using System.Collections.Generic;
using UnityEngine;

namespace Channels
{
    public class GenericChannel<T> : ScriptableObject
    {
       private readonly List<ISubscriber<T>> listeners = new ();

        public void Push(T value = default)
        {
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnReceive(value);
            }
        }

        public void AddSubscriber(ISubscriber<T> listener)
        {
            listeners.Add(listener);
        }

        public void RemoveSubscriber(ISubscriber<T> listener)
        {
            listeners.Remove(listener);
        } 
    }
}