using UnityEngine.Events;

namespace Channels
{
    public class BoolSubscriber : GenericSubscriber<bool>
    {
        public UnityEvent onTrueReceived;
        public UnityEvent onFalseReceived;
        public UnityEvent<bool> onChange;


        public override void OnReceive(bool value)
        {
            onChange.Invoke(value);
            
            if (value) onTrueReceived.Invoke();
            else onFalseReceived.Invoke();
        }
    }
}