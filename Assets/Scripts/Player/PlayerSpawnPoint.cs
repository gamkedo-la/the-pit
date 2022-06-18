using Channels;
using UnityEngine;

namespace Player
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public PlayerSpawnPointChannel channel;

        public void Unlock()
        {
            channel.Push(this);
        }
    }
}