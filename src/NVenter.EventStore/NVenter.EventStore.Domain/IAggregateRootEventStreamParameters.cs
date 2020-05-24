using NVenter.Core.EventStore;

namespace NVenter.EventStore.Domain
{
    public interface IAggregateRootEventStreamParameters<TAggregateRoot> : IEventStoreEventStreamParameters
    {
    }
}
