using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using NVenter.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NVenter.Sqlite.Core {
    public abstract class EventStream<TSqlParameters> : IEventStream<TSqlParameters> {
        private readonly ReadForwardEventStreamSettings _settings;
        private readonly ConnectionFactory _connectionFactory;

        public EventStream(ReadForwardEventStreamSettings settings, ConnectionFactory connectionFactory) {
            _settings = settings;
            _connectionFactory = connectionFactory;
        }

        public async Task<EventStreamSlice> GetEvents(TSqlParameters parameters) {
            string sql = @"SELECT * 
FROM EVENTS 
WHERE EventType in (@events) AND
      GlobalPosition > @GlobalPosition
ORDER BY GlobalPosition";

            using (var con = _connectionFactory.NewConnection()) {
                con.Open();
                var results = await con.QueryAsync<StoredEvent>(sql, parameters);

                var lastPosition = results.Max(_ => _.GlobalPosition);

                var eventWrappers = results.Select(_ => _.GetEventWrapper(_settings.TypesToStream.Single(t => _.EventType == t.FullName)));
                return new EventStreamSlice(eventWrappers, lastPosition);
            }
        }
    }

    public class SqliteEventStreamSettings {
        public string Sql { get; set; }
    }

    public class ReadForwardEventStreamSettings : SqliteEventStreamSettings {
        public IEnumerable<Type> TypesToStream { get; set; }
    }
}