using System;
using NVenter.Core;

namespace NVenter.Domain {
    public interface IAggregateStreamFactory {
        IEventStream MakeStream<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot, new();
    }
}