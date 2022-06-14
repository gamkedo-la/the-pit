namespace Channels
{
    public interface ISubscriber<in T>
    {
        public void OnReceive(T value);
    }
}