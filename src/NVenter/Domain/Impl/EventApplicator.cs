using NVenter;
using System.Collections.Generic;
using System.Linq;

namespace NVenter.Domain
{
    public class EventApplicator : IApplyEventsToAggregates
    {
        private readonly IProvideEventDelegates _eventDelegateProvider;

        public EventApplicator(IProvideEventDelegates eventDelegateProvider)
        {
            _eventDelegateProvider = eventDelegateProvider;
        }

        public void ApplyEvents<TAggregate>(TAggregate aggregate, IEnumerable<IEvent> events) where TAggregate : AggregateRoot
        {
            foreach (var @event in events)
            {
                if (_eventDelegateProvider.TryGetEventDelegate<TAggregate>(@event, out var @delegate))
                {
                    @delegate!.Invoke(aggregate, @event);
                }
            }

            aggregate.Version += events.Count();
        }
    }
}