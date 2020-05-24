using Dapper.Contrib.Extensions;
using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NVenter.Sqlite.Core {
    public class EventWriter : IEventWriter {
        private readonly ConnectionFactory _connectionFactory;

        public EventWriter(ConnectionFactory connectionFactory) {
            _connectionFactory = connectionFactory;
        }

        public async Task SaveEvents(string streamName, IEnumerable<EventWrapper> events, uint expectedVersion) {
            using (var connection = _connectionFactory.NewConnection()) {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction()) {
                    foreach (var @event in events.Select(_ => _.GetStoredEvent(streamName))) {
                        await connection.InsertAsync(@event);
                        transaction.Commit();
                    }
                }
            }
        }
    }

    public class StreamNameEventStreamParameters {
        public StreamNameEventStreamParameters(string streamName) {
            StreamName = streamName;
        }

        public string StreamName { get; }
    }
}
