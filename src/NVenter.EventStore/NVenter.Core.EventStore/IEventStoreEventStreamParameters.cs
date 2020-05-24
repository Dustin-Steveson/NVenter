namespace NVenter.Core.EventStore
{
    public interface IEventStoreEventStreamParameters
    {
        long Position { get; }
    }
}
