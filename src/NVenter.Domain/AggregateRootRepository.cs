using NVenter.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Domain {

    public class AggregateRootRepository<TAggregateRoot> : IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IEventStream<IAggregateRootEventStreamParameters<TAggregateRoot>> _eventStream;
        private readonly IAggregateRootEventStreamParametersFactory _eventStreamParametersFactory;
        private readonly IEventWriter _eventWriter;
        private readonly IBuildAggregateStreamNames _aggregateRootNameBuilder;

        public AggregateRootRepository(
            IEventStream<IAggregateRootEventStreamParameters<TAggregateRoot>> eventStream,
            IAggregateRootEventStreamParametersFactory eventStreamParametersFactory,
            IEventWriter eventWriter,
            IBuildAggregateStreamNames aggregateRootNameBuilder)
        {
            _eventStream = eventStream;
            _eventStreamParametersFactory = eventStreamParametersFactory;
            _eventWriter = eventWriter;
            _aggregateRootNameBuilder = aggregateRootNameBuilder;
        }

        public async Task<TAggregateRoot> Get(Guid aggregateId, bool shouldExist)
        {
            var eventStreamSlice = await _eventStream.GetEvents(_eventStreamParametersFactory.GetParameters<TAggregateRoot>(aggregateId));

            if (shouldExist && eventStreamSlice.Events.Any() == false)
                throw new InvalidOperationException($"No events found when attempting to hydrate aggregate with id: {aggregateId}");

            if (shouldExist == false && eventStreamSlice.Events.Any())
                throw new InvalidOperationException($"Unexpected eventstream found for aggregate with id: {aggregateId}");

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
                aggregateRoot.GetUncommittedChanges().Select(_ => new EventWrapper(_, 
                new Metadata { 
                    Id = messageContext.Id, 
                    CausationId = messageContext.CausationId,
                    CorrelationId = messageContext.CoorelationId,
                    Created = messageContext.Created
                })),
                aggregateRoot.Version);
        }
    }
}