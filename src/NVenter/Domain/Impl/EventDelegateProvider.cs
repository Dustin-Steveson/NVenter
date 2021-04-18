using System;
using System.Collections.Generic;

namespace NVenter.Domain
{
    internal class EventDelegateProvider : IProvideEventDelegates
    {
        private readonly IDictionary<Type, IDictionary<Type, EventMethod>> _eventDelegates;

        public EventDelegateProvider(IDictionary<Type, IDictionary<Type, EventMethod>> eventDelegates)
        {
            _eventDelegates = eventDelegates;
        }
        
        public bool TryGetEventDelegate<TAggregateRoot>(IEvent eventType, out EventMethod? eventMethod) where TAggregateRoot : AggregateRoot
        {
            eventMethod = null;
            return _eventDelegates.TryGetValue(typeof(TAggregateRoot), out var eventMethods) &&
                   eventMethods.TryGetValue(eventType.GetType(), out eventMethod);
        }

    }
}