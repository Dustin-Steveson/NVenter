using NVenter.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Domain
{

    public class AggregateRootRepository<TAggregateRoot> : IAggregateRootRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IEventStream<IAggregateRootEventStreamConfiguration<TAggregateRoot>> _eventStream;
        private readonly IAggregateRootEventStreamConfigurationFactory _eventStreamConfigurationFactory;
        private readonly IEventWriter _eventWriter;

        public AggregateRootRepository(IEventStream<
            IAggregateRootEventStreamConfiguration<TAggregateRoot>> eventStream,
            IAggregateRootEventStreamConfigurationFactory eventStreamConfigurationFactory,
            IEventWriter eventWriter)
        {
            _eventStream = eventStream;
            _eventStreamConfigurationFactory = eventStreamConfigurationFactory;
            _eventWriter = eventWriter;
        }

        public async Task<TAggregateRoot> Get(Guid aggregateId, bool shouldExist)
        {
            var eventStreamSlice = await _eventStream.GetEvents(_eventStreamConfigurationFactory.GetConfiguration<TAggregateRoot>(aggregateId), 0);

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

        public async Task Save(TAggregateRoot aggregateRoot)
        {
            await _eventWriter.SaveEvents(
                typeof(TAggregateRoot).Name,
                aggregateRoot.GetUncommittedChanges().Select(_ => new EventWrapper(_, new Metadata())),
                aggregateRoot.Version);
        }
    }
}