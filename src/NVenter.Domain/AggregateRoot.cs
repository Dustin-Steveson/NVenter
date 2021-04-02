using NVenter.Core;
using System;
using System.Collections.Generic;

namespace NVenter.Domain
{
    public abstract class AggregateRoot
    {
        public Guid Id { get; protected set; }
        public int Version { get; internal set; }
        private List<IEvent> _uncommittedEvents = new List<IEvent>();
        public IReadOnlyList<IEvent> UncommittedChanges() => _uncommittedEvents;

        protected void Raise<TEvent>(TEvent @event) where TEvent : IEvent
        {
            _uncommittedEvents.Add(@event);
        }

        public virtual void ApplyChanges(IApplyEventsToAggregates eventApplicator)
        {
            eventApplicator.ApplyEvents(this, _uncommittedEvents);
            _uncommittedEvents.Clear();
        }
    }
}