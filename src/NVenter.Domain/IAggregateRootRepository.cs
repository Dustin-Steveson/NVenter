using NVenter.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Domain
{
    public interface IAggregateRootRepository
    {
        Task<TAggregate> Get<TAggregate>(Guid id) where TAggregate : AggregateRoot, new();
        Task Save<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot;
    }

    public interface IApplyEventsToAggregates
    {
        void ApplyEvents<TAggregate>(TAggregate aggregate, IEnumerable<IEvent> events);
    }

    /**
public async Task<TAggregate> Get<TAggregate>(long id, CancellationToken cancellationToken) where TAggregate : Aggregate, new() {
    var aggregate = _aggregateFactory.Make<TAggregate>();
    var events = await _eventStore.Events
        .Where(_ => _.AggregateId == id && _.Version.HasValue)
        .OrderBy(_ => _.Version)
        .ToListAsync(cancellationToken);
    if (!events.Any()) {
        return null;
    }

    Log.Logger.Debug($"Hydrating aggregate {id} from {events.Count} events.");
    var domainEvents = events.Select(_mapper.Map<IDomainEvent>);
    aggregate.LoadFromHistory(domainEvents.OfType<AggregateDomainEvent>());
    return aggregate;
}
     */

    /**
namespace DCM.MACS.Domain.Aggregates {
    public abstract class Aggregate {
        public virtual long Id { get;  protected internal set; }
        public virtual long Version { get; private set; }
        public virtual IList<IDomainEvent> UncommittedChanges { get; protected set; } = new List<IDomainEvent>();
        internal IApplyAggregateEvents AggregateEventApplicator { get; set; }
        internal IClock Clock { get; set; }

        protected void Raise<TEvent>(TEvent @event) where TEvent : IDomainEvent {
            Ensure.That(AggregateEventApplicator, nameof(AggregateEventApplicator),
                    opts => opts.WithMessage(
                        "Event applicator is null. Make sure you used IAggregateFactory to create your aggregate."))
                .IsNotNull();

            if (Id != default) {
                @event.AggregateId = Id;
            }

            var shouldTimestampEvent = @event.Timestamp == default;
            if (shouldTimestampEvent) {
                @event.Timestamp = Clock.GetCurrentInstant();
            }

            AggregateEventApplicator.Apply(this, @event);
            UncommittedChanges.Add(@event);
        }

        public virtual void CommitChanges() {
            Version += UncommittedChanges.Count;
            UncommittedChanges = new List<IDomainEvent>();
        }

        public virtual void LoadFromHistory(IEnumerable<IDomainEvent> events) {
            Ensure.That(AggregateEventApplicator, nameof(AggregateEventApplicator),
                    opts => opts.WithMessage(
                        "Event applicator is null. Make sure you used IAggregateFactory to create your aggregate."))
                .IsNotNull();

            var eventArray = events.ToArray();
            AggregateEventApplicator.Apply(this, eventArray);
            Version = eventArray.Length;
        }
    }
}
     */
}