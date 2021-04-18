using System;
using System.Threading.Tasks;

namespace NVenter.Domain
{
    public interface IAggregateRootRepository
    {
        Task<TAggregate> Get<TAggregate>(Guid id) where TAggregate : AggregateRoot, new();
        Task Save<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot;
    }
}