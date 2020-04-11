using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Domain
{
    public class AggregateRootRepository<TAggregateRoot> : IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IEventRepository _eventRepository;

        public AggregateRootRepository(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<TAggregateRoot> Get(Guid aggregateId, bool shouldExist)
        {
            var events = await _eventRepository.GetEvents<TAggregateRoot>(StreamName(aggregateId));
            var aggregate = new TAggregateRoot();

            if (shouldExist && events.Any() == false)
                throw new InvalidOperationException($"No events found when attempting to hydrate aggregate with id: {aggregateId}");

            if (shouldExist == false && events.Any())
                throw new InvalidOperationException($"Unexpected eventstream found for aggregate with id: {aggregateId}");

            foreach (var @event in events)
            {
                aggregate.Apply(@event.Event);
            }

            return aggregate;
        }

        public async Task Save(TAggregateRoot aggregateRoot)
        {
            await _eventRepository.SaveEvents<TAggregateRoot>(
                StreamName(aggregateRoot.Id),
                aggregateRoot.GetUncommittedChanges().Select(_ => new EventWrapper(_, new Metadata())),
                aggregateRoot.Version);
        }

        private string StreamName(Guid aggregateId) => $"{typeof(TAggregateRoot).Name}\\{aggregateId}";
    }
}