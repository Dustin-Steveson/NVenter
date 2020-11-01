using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using NVenter.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NVenter.Sqlite.Core {
    public abstract class EventStream : IEventStream<IDictionary<string, object>> {
        private readonly EventStreamSettings _settings;
        private readonly ConnectionFactory _connectionFactory;

        public EventStream(EventStreamSettings settings, ConnectionFactory connectionFactory) {
            _settings = settings;
            _connectionFactory = connectionFactory;
        }

        public async Task<EventStreamSlice> GetEvents(IDictionary<string, object> parameters) {

            using (var con = _connectionFactory.NewConnection()) {
                await con.OpenAsync();

                var paramys = new DynamicParameters();

                foreach (var pair in parameters) paramys.Add(pair.Key, pair.Value);

                foreach (var pair in StreamSpecificParameters) paramys.Add(pair.Key, pair.Value);

                var results = await con.QueryAsync<StoredEvent>(Sql, paramys);

                var lastPosition = results.Any() ? results.Max(_ => _.GlobalPosition) : 0;

                var eventWrappers = results.Select(_ => _.GetEventWrapper(_settings.TypesToStream.Single(t => _.EventType == t.FullName)));
                return new EventStreamSlice(eventWrappers, lastPosition);
            }
        }

        public abstract IDictionary<string, object> StreamSpecificParameters { get; }

        public abstract string Sql { get; }
    }
}