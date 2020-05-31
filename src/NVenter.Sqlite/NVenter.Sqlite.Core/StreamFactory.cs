using System;
using Dapper;
using NVenter.Core;
using NVenter.Domain;

namespace NVenter.Sqlite.Core {
    public class StreamFactory : IPositionalStreamFactory, IAggregateStreamFactory {
        private readonly ReadForwardEventStreamSettings _settings;
        private readonly ConnectionFactory _connectionFactory;

        public StreamFactory(ReadForwardEventStreamSettings settings, ConnectionFactory connectionFactory) {
            _settings = settings;
            _connectionFactory = connectionFactory;
        }

        public IEventStream MakeStream(long position) {
            const string sql = @"
                SELECT * 
                FROM EVENTS 
                WHERE EventType in (@events) AND
                    GlobalPosition > @GlobalPosition
                ORDER BY GlobalPosition";
            var query = new CommandDefinition(sql, new DynamicParameters(new { GlobalPosition = position}));
            return new EventStream(_settings, _connectionFactory, query);
        }

        public IEventStream MakeStream<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot, new() {
            const string sql = @"
                SELECT * 
                FROM event 
                WHERE StreamName = @StreamName
                ORDER BY StreamPosition";
            var streamName = $"{typeof(TAggregateRoot).Name}-{aggregateId.ToString()}";
            var query = new CommandDefinition(sql, new DynamicParameters(new { StreamName = streamName }));
            return new EventStream(_settings, _connectionFactory, query);
        }
    }
}