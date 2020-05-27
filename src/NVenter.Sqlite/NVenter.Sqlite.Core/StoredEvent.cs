using Newtonsoft.Json;
using NVenter.Core;
using System;
using System.Linq;

namespace NVenter.Sqlite.Core {
    public class StoredEvent {
        public long GlobalPosition { get; set; }
        public int StreamPosition { get; set; }
        public string Data { get; set; }
        public string MetaData { get; set; }
        public DateTime Created { get; set; }
        public string EventType { get; set; }
        public string EventId { get; set; }
        public string StreamName { get; set; }
    }

    public static class EventWrapperExtensions {
        public static EventWrapper GetEventWrapper(this StoredEvent storedEvent, Type eventType) {
            var @event = (IEvent)JsonConvert.DeserializeObject(storedEvent.Data, eventType);
            var metaData = JsonConvert.DeserializeObject<Metadata>(storedEvent.MetaData);
            return new EventWrapper(@event, metaData);
        }
    }

    public static class StoredEventExtensions {
        public static StoredEvent GetStoredEvent(this EventWrapper eventWrapper, string streamName) {
            var @event = JsonConvert.SerializeObject(eventWrapper.Event);
            var metaData = JsonConvert.SerializeObject(eventWrapper.Metadata);

            return new StoredEvent {
                StreamName = streamName,
                EventType = eventWrapper.Event.GetType().Name,
                Created = eventWrapper.Metadata.Created.UtcDateTime,
                Data = @event,
                MetaData = metaData,
                EventId = eventWrapper.Metadata.Id.ToString(),
                StreamPosition = eventWrapper.Metadata.StreamPosition
            };
        }
    }
}
