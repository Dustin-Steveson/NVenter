using NVenter.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Domain {

    public class AggregateRootRepository<TAggregateRoot> : IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IAggregateStreamFactory _streamFactory;
        private readonly IEventWriter _eventWriter;
        private readonly IBuildAggregateStreamNames _aggregateRootNameBuilder;

        public AggregateRootRepository(
            IAggregateStreamFactory streamFactory,
            IEventWriter eventWriter,
            IBuildAggregateStreamNames aggregateRootNameBuilder)
        {
            _streamFactory = streamFactory;
            _eventWriter = eventWriter;
            _aggregateRootNameBuilder = aggregateRootNameBuilder;
        }

        public async Task<TAggregateRoot> Get(Guid aggregateId, bool shouldExist) {

            var eventStream = _streamFactory.MakeStream<TAggregateRoot>(aggregateId);
            var eventStreamSlice = await eventStream.GetEvents();

            if (shouldExist && eventStreamSlice.Events.Any() == false)
                throw new InvalidOperationException($"No events found when attempting to hydrate aggregate with id: {aggregateId.ToString()}");

            if (shouldExist == false && eventStreamSlice.Events.Any())
                throw new InvalidOperationException($"Unexpected eventstream found for aggregate with id: {aggregateId.ToString()}");

            var aggregate = new TAggregateRoot();

            foreach (var @event in eventStreamSlice.Events)
            {
                aggregate.Apply(@event.Event);
            }

            return aggregate;
        }

        public async Task Save(TAggregateRoot aggregateRoot, MessageContext messageContext)
        {
            await _eventWriter.SaveEvents(
                _aggregateRootNameBuilder.GetStreamName(aggregateRoot),
                aggregateRoot.GetUncommittedChanges().Select((_, i) => new EventWrapper(_,
                new Metadata {
                    Id = messageContext.Id,
                    CausationId = messageContext.CausationId,
                    CorrelationId = messageContext.CoorelationId,
                    Created = messageContext.Created,
                    StreamPosition = aggregateRoot.Version + i
                })));
        }
    }
}