using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NVenter.Core.EventStore
{
    public class EventStoreReadForwardEventStream<TStreamParameters> : IEventStream<TStreamParameters>
        where TStreamParameters : IEventStoreEventStreamParameters
    {
        private readonly EventStoreReadForwardEventStreamSettings _settings;

        public EventStoreReadForwardEventStream(EventStoreReadForwardEventStreamSettings settings) {
            _settings = settings;
        }

        public async Task<EventStreamSlice> GetEvents(TStreamParameters parameters)
        {
            var eventStoreSettings = ConnectionSettings.Create();
            eventStoreSettings.SetDefaultUserCredentials(new UserCredentials(_settings.UserName, _settings.Password));
            using (var conn = EventStoreConnection.Create(eventStoreSettings, new IPEndPoint(IPAddress.Loopback, 1113)))
            {
                var streamSlice = await conn.ReadStreamEventsForwardAsync(_settings.StreamName, parameters.Position, _settings.NumberOfEventsPerFetch, true);
                var events =
                    streamSlice
                    .Events
                    .Select(_ => GetEventWrapperFromEventStoreEvent(_));

                return new EventStreamSlice(events, streamSlice.LastEventNumber);
            }
        }

        public static EventWrapper GetEventWrapperFromEventStoreEvent(ResolvedEvent resolvedEvent) {
            var @event = (IEvent)JsonConvert.DeserializeObject(Encoding.ASCII.GetString(resolvedEvent.Event.Data), Type.GetType(resolvedEvent.Event.EventType));
            var metaData = JsonConvert.DeserializeObject<Metadata>(Encoding.ASCII.GetString(resolvedEvent.Event.Metadata));
            return new EventWrapper(@event, metaData);
        }
    }

    public class EventStoreReadForwardEventStream : IEventStoreEventStreamParameters
    {
        public EventStoreReadForwardEventStream(long position)
        {
            Position = position;
        }
        public long Position { get; }
    }
}
