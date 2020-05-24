using NVenter.Domain;
using System;

namespace NVenter.Sqlite.Domain {
    public class AggregateRootEventStreamSqlParametersFactory<TAggregateRoot> : IAggregateRootEventStreamParametersFactory<AggregateRootStreamSqlParameters<TAggregateRoot>, TAggregateRoot> where TAggregateRoot : AggregateRoot, new() {

        // public IAggregateRootEventStreamParameters<TAggregateRoot> GetParameters<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot, new() {
        //     return new AggregateRootStreamSqlParameters<TAggregateRoot>(aggregateId);
        // }


        public AggregateRootStreamSqlParameters<TAggregateRoot> GetParameters(Guid aggregateId) {
            return new AggregateRootStreamSqlParameters<TAggregateRoot>(aggregateId);
        }
    }
//@"SELECT * 
//FROM EVENTS 
//WHERE StreamName = @StreamName
//ORDER BY StreamPosition";
}
