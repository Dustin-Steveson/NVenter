using NVenter.Domain;
using NVenter.Sqlite.Core;
using System;
using System.Dynamic;

namespace NVenter.Sqlite.Domain {
    public class AggregateRootStreamSqlParameters<TAggregateRoot> : IAggregateRootEventStreamParameters<TAggregateRoot>, ISqlParamters
        where TAggregateRoot : AggregateRoot, new() {
        public AggregateRootStreamSqlParameters(Guid Id) {
            Parameters = new ExpandoObject();
            Parameters.StreamName = $"{typeof(TAggregateRoot).Name}-{Id}";
        }
        public dynamic Parameters { get; }
    }
}
