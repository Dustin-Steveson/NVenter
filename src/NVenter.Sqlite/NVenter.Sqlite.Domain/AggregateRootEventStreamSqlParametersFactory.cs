using NVenter.Domain;
using System;
using System.Collections.Generic;

namespace NVenter.Sqlite.Domain {
    public class AggregateRootEventStreamSqlParametersFactory<TAggregateRoot> 
        : IAggregateRootEventStreamParametersFactory<IDictionary<string, object>, TAggregateRoot>
        where TAggregateRoot : AggregateRoot, new() {


        public IDictionary<string, object> GetParameters(Guid aggregateId) {
            return new Dictionary<string, object> { ["StreamName"] = $"{typeof(TAggregateRoot).Name}-{aggregateId}" };
        }
    }
}
