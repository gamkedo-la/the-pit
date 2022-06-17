using UnityEngine;
using UnityEngine.Events;

namespace Channels
{
    public class BoolSubscriber : MonoBehaviour, ISubscriber<bool>
    {
        public BoolChannel channel;

        public UnityEvent onTrueReceived;
        public UnityEvent onFalseReceived;
        public UnityEvent<bool> onChange;

       private void OnEnable()
        {
            channel.AddSubscriber(this);
        }

        private void OnDisable()
        {
            channel.RemoveSubscriber(this);
        }

        public void OnReceive(bool value)
        {
            onChange.Invoke(value);
            
            if (value) onTrueReceived.Invoke();
            else onFalseReceived.Invoke();
        }
    }
}