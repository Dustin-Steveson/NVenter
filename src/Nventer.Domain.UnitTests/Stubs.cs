using NVenter;
using NVenter.Domain;
using System;

namespace Nventer.Domain.UnitTests
{
    public class StubEvent1 : IEvent { }

    public class StubEvent2 : IEvent { }

    public class StubAggregateRoot : AggregateRoot
    {
        public Guid Id { get; set; }

        private void ApplyStubEvent(StubEvent1 @event)
        {
            Id = Guid.NewGuid();
        }

        public void DoAThing() { }
    }
    
    public class StubAggregateRootWithPublicEventMethod : AggregateRoot
    {
        public void ApplyStubEvent(StubEvent1 @event) { }
    }
}
