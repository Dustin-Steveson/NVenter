using NVenter.Core.EventStore;

namespace NVenter.EventStore.Domain
{
    public interface IAggregateRootEventStreamConfiguration<TAggregateRoot> : IEventStoreEventStreamConfiguration
    {
    }
}
