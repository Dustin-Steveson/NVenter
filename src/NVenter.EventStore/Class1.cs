using NVenter;
using System;
using System.Threading.Tasks;

namespace NVenter.EventStore
{
    //public class EventStoreEventReader : IReadEvents
    //{
    //    public EventStoreEventReader()
    //    {

    //    }

    //    public async Task<EventStreamSlice> GetEvents(TStreamConfiguration configuration, long position)
    //    {
    //        var settings = ConnectionSettings.Create();
    //        settings.SetDefaultUserCredentials(new UserCredentials(_settings.UserName, _settings.Password));
    //        using (var conn = EventStoreConnection.Create(settings, new IPEndPoint(IPAddress.Loopback, 1113)))
    //        {
    //            var streamSlice = await conn.ReadStreamEventsForwardAsync(configuration.StreamName, position, _settings.NumberOfEventsPerFetch, true);
    //            var events =
    //                streamSlice
    //                .Events
    //                .Select(_ => GetEventWrapperFromEventStoreEvent(_));

    //            return new EventStreamSlice(events, streamSlice.LastEventNumber);
    //        }
    //    }

    //    public static EventWrapper GetEventWrapperFromEventStoreEvent(ResolvedEvent resolvedEvent)
    //    {
    //        var @event = (IEvent)JsonConvert.DeserializeObject(Encoding.ASCII.GetString(resolvedEvent.Event.Data), Type.GetType(resolvedEvent.Event.EventType));
    //        var metaData = JsonConvert.DeserializeObject<Metadata>(Encoding.ASCII.GetString(resolvedEvent.Event.Metadata));
    //        return new EventWrapper(@event, metaData);
    //    }
    //}

    //public class EventStoreReadForwardEventStream : IEventStoreEventStreamConfiguration
    //{
    //    public EventStoreReadForwardEventStream(string streamName)
    //    {
    //        StreamName = streamName;
    //    }
    //}
}
