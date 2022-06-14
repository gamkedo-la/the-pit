using UnityEngine;

namespace Channels
{
    public abstract class GenericSubscriber<T> : MonoBehaviour, ISubscriber<T>
    {
        public GenericChannel<T> channel;

       private void OnEnable()
        {
            channel.AddSubscriber(this);
        }

        private void OnDisable()
        {
            channel.RemoveSubscriber(this);
        }

        public abstract void OnReceive(T value);
    }
}