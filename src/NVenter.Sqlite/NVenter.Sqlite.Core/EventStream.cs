using Dapper;
using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Sqlite.Core {
    internal class EventStream : IEventStream {
        private readonly ReadForwardEventStreamSettings _settings;
        private readonly ConnectionFactory _connectionFactory;
        private readonly CommandDefinition _commandDefinition;

        internal EventStream(ReadForwardEventStreamSettings settings, ConnectionFactory connectionFactory, CommandDefinition commandDefinition) {
            _settings = settings;
            _connectionFactory = connectionFactory;
            _commandDefinition = commandDefinition;
        }

        public async Task<EventStreamSlice> GetEvents() {
            using (var con = _connectionFactory.NewConnection()) {
                await con.OpenAsync();
                var results = await con.QueryAsync<StoredEvent>(_commandDefinition);

                var lastPosition = 0L;
                var eventWrappers = results.Select(_ => {
                    lastPosition = _.GlobalPosition;
                    return _.GetEventWrapper(_settings.TypesToStream.Single(t => _.EventType == t.FullName));
                });
                return new EventStreamSlice(eventWrappers, lastPosition);
            }
        }
    }

    public class ReadForwardEventStreamSettings {
        public IEnumerable<Type> TypesToStream { get; set; }
    }
}