using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Aggregate
{
    public interface IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        Task Save(IEnumerable<EventWrapper> events, Guid id, uint expectedVersion);
        Task<TAggregateRoot> Get(Guid aggregateId, bool shouldExist);
    }
}