using System;

namespace NVenter.Domain
{
    public interface IAggregateRootEventStreamConfiguration<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        Guid Id { get; }
    }
}