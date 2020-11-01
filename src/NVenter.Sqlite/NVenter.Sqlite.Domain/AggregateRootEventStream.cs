using NVenter.Core;
using NVenter.Domain;
using NVenter.Sqlite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NVenter.Sqlite.Domain {
    public class AggregateRootEventStream<TAggregateRoot> : EventStream
    where TAggregateRoot : AggregateRoot, new() {
        public AggregateRootEventStream(AggregateRootEventTypeFetcher aggregateRootEventTypeFetcher, ConnectionFactory connectionFactory)
            : base(new EventStreamSettings { TypesToStream = aggregateRootEventTypeFetcher.GetTypes<TAggregateRoot>() }, connectionFactory) { }

        public override IDictionary<string, object> StreamSpecificParameters => new Dictionary<string, object> { };

        public override string Sql => @"
SELECT * 
FROM event 
WHERE StreamName = @StreamName
ORDER BY StreamPosition";
    }

    public class AggregateRootEventTypeFetcher {
        public IEnumerable<Type> GetTypes<TAggregate>() {
            return typeof(TAggregate)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Select(_ => _.GetParameters())
                .Where(_ => _.Count() == 1)
                .Select(_ => _.Single().ParameterType)
                .Where(_ => typeof(IEvent).IsAssignableFrom(_));
        }
    }
}