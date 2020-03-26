using NVenter.Core;
using System;
using System.Collections.Generic;
using static NVenter.Aggregate.AggregateRootInitializer;

namespace NVenter.Aggregate
{
    public abstract class AggregateRoot
    {
        internal static IDictionary<Type, IDictionary<Type, DynamicMethodDelegate>> EventMethods;
        public List<IEvent> _uncommitedEvents;
        public Guid Id { get; protected set; }
        public AggregateRoot()
        {
            _uncommitedEvents = new List<IEvent>();
        }

        public IEnumerable<IEvent> GetUncommittedChanges() => _uncommitedEvents;

        public void MarkChangesCommited()
        {
            _uncommitedEvents.Clear();
        }

        public void Apply(IEvent @event)
        {
            if(EventMethods.ContainsKey(this.GetType()) && EventMethods[GetType()].ContainsKey(@event.GetType()))
            {
                EventMethods[GetType()][@event.GetType()](this, @event);
            }
        }

        public uint Version { get; internal set; }
    }
}