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

        protected void AddEvents(IEnumerable<IEvent> events)
        {
            _uncommittedEvents.AddRange(events);
        }
    }
}