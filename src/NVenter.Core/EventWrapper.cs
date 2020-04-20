namespace NVenter.Core
{
    public class EventWrapper<TEvent> : EventWrapper where TEvent : IEvent
    {
        public EventWrapper(TEvent @event, Metadata metadata, ulong position) : base(@event, metadata) { }
        public new TEvent Event { get; }
    }

    public class EventWrapper
    {
        public EventWrapper(IEvent @event, Metadata metadata)
        {
            Event = @event;
            Metadata = metadata;
        }

        public IEvent Event { get; }
        public Metadata Metadata { get; }
    }
}