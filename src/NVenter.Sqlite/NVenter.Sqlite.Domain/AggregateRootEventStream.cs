using NVenter.Domain;
using NVenter.Sqlite.Core;

namespace NVenter.Sqlite.Domain {
    public class AggregateRootEventStream<TAggregateRoot> : EventStream
    where TAggregateRoot : AggregateRoot, new() {
        public AggregateRootEventStream(ReadForwardEventStreamSettings settings, ConnectionFactory connectionFactory)
            : base(settings, connectionFactory) {
        }
    }
}