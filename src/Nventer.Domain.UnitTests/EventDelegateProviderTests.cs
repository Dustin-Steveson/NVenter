using NVenter.Domain;
using Xunit;
using System;
using System.Collections.Generic;
using Shouldly;

namespace Nventer.Domain.UnitTests
{
    public static class EventDelegateProviderTests
    {
        public class TryGetEventDelegate
        {
            [Fact]
            public void Should_return_false_when_initialized_with_no_types()
            {
                var provider = new EventDelegateProvider(new Dictionary<Type, IDictionary<Type, EventMethod>>());

                var didFindDelegate = provider.TryGetEventDelegate<StubAggregateRoot>(new StubEvent1(), out var @delegate);

                didFindDelegate.ShouldBeFalse();
            }
            
            [Fact]
            public void Should_return_false_when_aggregate_contains_no_delegates_for_the_given_event()
            {
                var provider = new EventDelegateProvider(new Dictionary<Type, IDictionary<Type, EventMethod>>
                {
                    [typeof(StubAggregateRoot)] = new Dictionary<Type, EventMethod>(),
                });

                var didFindDelegate = provider.TryGetEventDelegate<StubAggregateRoot>(new StubEvent1(), out var @delegate);

                didFindDelegate.ShouldBeFalse();
            }

            [Fact]
            public void Should_return_true_when_aggregate_contains_a_delegate_for_the_given_event()
            {
                var provider = new EventDelegateProvider(new Dictionary<Type, IDictionary<Type, EventMethod>>
                {
                    [typeof(StubAggregateRoot)] = new Dictionary<Type, EventMethod>
                    {
                        [typeof(StubEvent1)] = delegate { },
                    },
                });

                var didFindDelegate = provider.TryGetEventDelegate<StubAggregateRoot>(new StubEvent1(), out var @delegate);

                didFindDelegate.ShouldBeTrue();
            }

            [Fact]
            public void Should_provide_delegate_when_one_matches_the_given_event()
            {
                EventMethod matchingDelegate = delegate { };
                var provider = new EventDelegateProvider(new Dictionary<Type, IDictionary<Type, EventMethod>>
                {
                    [typeof(StubAggregateRoot)] = new Dictionary<Type, EventMethod>
                    {
                        [typeof(StubEvent1)] = matchingDelegate,
                    },
                });

                _ = provider.TryGetEventDelegate<StubAggregateRoot>(new StubEvent1(), out var @delegate);

                @delegate.ShouldBe(matchingDelegate);
            }
        }
    }
}