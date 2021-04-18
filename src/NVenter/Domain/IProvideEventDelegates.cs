namespace NVenter.Domain
{
    public delegate void EventMethod(AggregateRoot aggregateRoot, IEvent @event);
    
    public interface IProvideEventDelegates
    {
        bool TryGetEventDelegate<TAggregateRoot>(IEvent eventType, out EventMethod? eventMethod) where TAggregateRoot : AggregateRoot;
    }
}