using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Aggregate
{
    public interface IEventRepository
    {
        Task<IEnumerable<EventWrapper>> GetEvents<TAggregate>(Guid aggregateId) where TAggregate : AggregateRoot;
        Task SaveEvents<TAggregate>(IEnumerable<EventWrapper> events, uint expectedVersion) where TAggregate : AggregateRoot;
    }
}