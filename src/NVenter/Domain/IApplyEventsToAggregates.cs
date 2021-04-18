using System.Collections.Generic;

namespace NVenter.Domain
{
    public interface IApplyEventsToAggregates
    {
        void ApplyEvents<TAggregate>(TAggregate aggregate, IEnumerable<IEvent> events) where TAggregate : AggregateRoot;
    }
}