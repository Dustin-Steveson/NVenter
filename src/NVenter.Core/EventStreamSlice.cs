using System.Collections.Generic;

namespace NVenter.Core
{
    public class EventStreamSlice
    {
        public EventStreamSlice(IEnumerable<EventWrapper> events, long lastPosition)
        {
            Events = events;
            LastPosition = lastPosition;
        }

        public IEnumerable<EventWrapper> Events { get; }
        public long LastPosition { get; }
    }
}