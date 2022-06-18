using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Channels
{
    public class PlayerSpawnPointSubscriber : MonoBehaviour, ISubscriber<PlayerSpawnPoint>
    {
        public PlayerSpawnPointChannel channel;
        public UnityEvent<PlayerSpawnPoint> onReceive;
        
        public void OnReceive(PlayerSpawnPoint value)
        {
            onReceive.Invoke(value);
        }

        private void OnEnable()
        {
            channel.AddSubscriber(this);
        }

        private void OnDisable()
        {
            channel.RemoveSubscriber(this);
        }
    }
}