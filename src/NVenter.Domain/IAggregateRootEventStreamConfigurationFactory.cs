using System;

namespace NVenter.Domain
{
    public interface IAggregateRootEventStreamConfigurationFactory
    {
        IAggregateRootEventStreamConfiguration<TAggregateRoot> GetConfiguration<TAggregateRoot>(Guid aggregateId) where TAggregateRoot : AggregateRoot;
    }
}