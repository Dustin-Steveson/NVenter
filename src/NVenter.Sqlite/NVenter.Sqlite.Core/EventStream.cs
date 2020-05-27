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
    public abstract class EventStream : IEventStream<IDictionary<string, object>> {
        private readonly ReadForwardEventStreamSettings _settings;
        private readonly ConnectionFactory _connectionFactory;

        public EventStream(ReadForwardEventStreamSettings settings, ConnectionFactory connectionFactory) {
            _settings = settings;
            _connectionFactory = connectionFactory;
        }

        public async Task<EventStreamSlice> GetEvents(IDictionary<string, object> parameters) {
            string sql = @"SELECT * 
FROM EVENTS 
WHERE EventType in (@events) AND
      GlobalPosition > @GlobalPosition
ORDER BY GlobalPosition";

            using (var con = _connectionFactory.NewConnection()) {
                await con.OpenAsync();

                var paramys = new DynamicParameters();

                foreach (var pair in (IDictionary<string, object>)parameters) paramys.Add(pair.Key, pair.Value);

                var results = await con.QueryAsync<StoredEvent>(_settings.Sql, paramys);

                var lastPosition = results.Any() ? results.Max(_ => _.GlobalPosition) : 0;

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