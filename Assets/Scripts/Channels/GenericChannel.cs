using System.Collections.Generic;
using UnityEngine;

namespace Channels
{
    public class GenericChannel<T> : ScriptableObject
    {
        [SerializeField] private T defaultValue;
        
        private readonly List<ISubscriber<T>> listeners = new();
        public T Value { get; private set; }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            Value = defaultValue;
        }
        
        public void Push(T value)
        {
            if (EqualityComparer<T>.Default.Equals(value, Value)) return;
            
            for (var i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnReceive(value);
            }

            Value = value;
        }

        public void AddSubscriber(ISubscriber<T> listener)
        {
            listeners.Add(listener);
            listener.OnReceive(Value);
        }

        public void RemoveSubscriber(ISubscriber<T> listener)
        {
            listeners.Remove(listener);
        }
    }
}