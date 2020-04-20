namespace NVenter.Core.EventStore
{
    public class EventStoreReadForwardEventStreamSettings
    {
        public int NumberOfEventsPerFetch { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
