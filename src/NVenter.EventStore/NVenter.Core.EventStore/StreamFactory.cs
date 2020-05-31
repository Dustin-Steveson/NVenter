using System;
using NVenter.Domain;

namespace NVenter.Core.EventStore {
    public class StreamFactory : IUserStreamFactory, IAggregateStreamFactory {
        private readonly string _username;
        private readonly string _password;
        private readonly int _pageSize;

        public StreamFactory(string username, string password, int pageSize) {
            _username = username;
            _password = password;
            _pageSize = pageSize;
        }

        public IEventStream MakeStream(Guid userId, long position, int eventCount) {
            var settings = new EventStoreReadForwardEventStreamSettings {
                UserName = _username,
                Password = _password,
                StreamName = $"user-{userId}",
                NumberOfEventsPerFetch = _pageSize,
            };
            return new EventStoreReadForwardEventStream(settings, position);
        }

        public IEventStream MakeStream<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot, new() {
            var settings = new EventStoreReadForwardEventStreamSettings {
                UserName = _username,
                Password = _password,
                StreamName = $"{typeof(TAggregateRoot)}-{aggregateId}",
                NumberOfEventsPerFetch = _pageSize,
            };
            return new EventStoreReadForwardEventStream(settings, 0);
        }
    }
}