using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Domain
{
    public interface IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        Task Save(TAggregateRoot aggregateRoot);
        Task<TAggregateRoot> Get(Guid aggregateId, bool shouldExist);
    }
}