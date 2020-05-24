using NVenter.Domain;
using System;

namespace NVenter.EventStore.Domain
{
    public class EventStoreAggregateParameters<TAggregateRoot> : NVenter.Domain.IAggregateRootEventStreamParameters<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        public EventStoreAggregateParameters(Guid id)
        {
            Id = id;
        }
        public Guid Id { get; }
        public string StreamName { get => $"{typeof(TAggregateRoot).Name}\\{Id}"; }
        public long Position => 0;
    }
}
