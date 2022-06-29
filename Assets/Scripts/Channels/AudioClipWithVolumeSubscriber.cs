using Audio;
using UnityEngine;
using UnityEngine.Events;

namespace Channels
{
    public class AudioClipWithVolumeSubscriber : MonoBehaviour, ISubscriber<AudioClipWithVolume>
    {
        public AudioClipWithVolumeChannel channel;
        public UnityEvent<AudioClipWithVolume> onReceive;
        
        public void OnReceive(AudioClipWithVolume value)
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