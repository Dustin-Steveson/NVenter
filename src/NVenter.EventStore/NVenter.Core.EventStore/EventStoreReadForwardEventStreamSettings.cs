namespace NVenter.Core.EventStore
{
    public class EventStoreReadForwardEventStreamSettings
    {
        public string StreamName { get; set; }
        public int NumberOfEventsPerFetch { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
