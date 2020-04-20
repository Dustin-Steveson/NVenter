using NVenter.Domain;
using System;

namespace NVenter.EventStore.Domain
{
    public class EventStoreAggregateConfiguration<TAggregateRoot> : IAggregateRootEventStreamConfiguration<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        public EventStoreAggregateConfiguration(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
        public string StreamName { get => $"{typeof(TAggregateRoot).Name}\\{Id}"; }
    }
}
