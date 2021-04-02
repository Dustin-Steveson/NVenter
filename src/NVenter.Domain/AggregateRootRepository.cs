using NVenter.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Domain
{
    public class AggregateRootRepository : IAggregateRootRepository
    {
        private readonly IReadEvents _eventReader;
        private readonly IWriteEvents _eventWriter;

        public AggregateRootRepository(IReadEvents eventReader, IWriteEvents eventWriter)
        {
            _eventReader = eventReader;
            _eventWriter = eventWriter;
        }

        public async Task<TAggregate> Get<TAggregate>(Guid id)
            where TAggregate : AggregateRoot, new()
        {
            var events = await _eventReader.GetEvents($"{typeof(TAggregate).Name}-{id}", 0);

            if (events.Any() == false)
                throw new AggregateNotFoundException($"No events found when attempting to hydrate aggregate with id: {id}");

            var aggregate = new TAggregate();

            foreach (var @event in events)
            {
                aggregate.Apply(@event);
            }

            return aggregate;
        }

        public Task Save<TAggregate>(TAggregate aggregate)
            where TAggregate : AggregateRoot
        {
            return _eventWriter.SaveEvents( 
                $"{typeof(TAggregate).Name}-{aggregate.Id}",
                aggregate.UncommittedChanges(),
                aggregate.Version);
        }

        class AggregateNotFoundException : Exception
        {
            public AggregateNotFoundException(string message) : base(message) { }
        }
    }
}