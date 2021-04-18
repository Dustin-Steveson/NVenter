using Moq;
using NVenter;
using NVenter.Domain;
using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;

namespace Nventer.Domain.UnitTests
{
    delegate void TryGetDelegate(IEvent @event, ref EventMethod @delegate);
    public class AggregateRootEventApplicatorTests
    {
        private IDictionary<Type, EventMethod> _eventDelegates = new Dictionary<Type, EventMethod>();
        private readonly Mock<IProvideEventDelegates> _mockDelegateProvider;
        private readonly IApplyEventsToAggregates _eventApplicator;

        public AggregateRootEventApplicatorTests()
        {
            _mockDelegateProvider = new Mock<IProvideEventDelegates>();
            _eventApplicator = new EventApplicator(_mockDelegateProvider.Object);
        }

        [Fact]
        public void Should_invoke_delegates_matching_given_events()
        {
            bool didInvokeDelegateForStub1 = false, didInvokeDelegateForStub2 = false;
            StubCallback<StubEvent1>(false, () => didInvokeDelegateForStub1 = true);
            StubCallback<StubEvent2>(true, () => didInvokeDelegateForStub2 = true);

            _eventApplicator.ApplyEvents(new StubAggregateRoot(), new IEvent[] { new StubEvent1(), new StubEvent2() });

            didInvokeDelegateForStub1.ShouldBeFalse();
            didInvokeDelegateForStub2.ShouldBeTrue();
        }

        [Fact]
        public void Should_increment_aggregate_version_for_every_event_applied_to_an_aggregate()
        {
            var aggregate = new StubAggregateRoot();

            _eventApplicator.ApplyEvents(aggregate, new IEvent[] { new StubEvent1(), new StubEvent2() });

            aggregate.Version.ShouldBe(2);
        }

        private void StubCallback<TEvent>(bool result, Action callback) where TEvent : IEvent
        {
            _mockDelegateProvider.Setup(
                    provider => provider.TryGetEventDelegate<StubAggregateRoot>(It.IsAny<TEvent>(), out It.Ref<EventMethod>.IsAny))
                .Callback(new TryGetDelegate((IEvent @event, ref EventMethod eventMethod2) =>
                {
                    eventMethod2 = delegate { callback(); };
                })).Returns(result);
        }
    }
}
