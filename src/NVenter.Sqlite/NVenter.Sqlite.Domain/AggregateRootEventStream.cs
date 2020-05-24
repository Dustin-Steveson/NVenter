using NVenter.Domain;
using NVenter.Sqlite.Core;

namespace NVenter.Sqlite.Domain {
    public class AggregateRootEventStream<TStreamParameters, TAggregateRoot> : EventStream<TStreamParameters>
    where TStreamParameters : IAggregateRootEventStreamParameters<TAggregateRoot>
    where TAggregateRoot : AggregateRoot, new() {
        public AggregateRootEventStream(ReadForwardEventStreamSettings settings, ConnectionFactory connectionFactory)
            : base(settings, connectionFactory) {
        }
    }
}