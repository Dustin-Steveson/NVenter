using System;

namespace NVenter.Domain
{
    public interface IAggregateRootEventStreamParametersFactory
    {
        IAggregateRootEventStreamParameters<TAggregateRoot> GetParameters<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot, new();
    }
}