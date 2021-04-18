using NVenter.Domain;
using Shouldly;
using System;
using Xunit;

namespace Nventer.Domain.UnitTests
{
    public static class AggregateDelegateFactoryTests
    {
        public class GetEventApplicationMethods
        {
            private readonly AggregateDelegateFactory _delegateFactory = new AggregateDelegateFactory();

            [Fact]
            public void Should_build_correct_delegates_for_aggregate()
            {
                var result = _delegateFactory.GetEventApplicationMethods(typeof(StubAggregateRoot));
                result.ShouldContain(kvp => kvp.Key == typeof(StubAggregateRoot));
                result[typeof(StubAggregateRoot)].ShouldContain(kvp => kvp.Key == typeof(StubEvent1));
                result[typeof(StubAggregateRoot)].ShouldNotContain(kvp => kvp.Key == typeof(StubEvent2));
            }

            [Fact]
            public void Should_correctly_construct_delegates()
            {
                var aggregate = new StubAggregateRoot();

                var result = _delegateFactory.GetEventApplicationMethods(typeof(StubAggregateRoot));
                var aggregate2Delegates = result[typeof(StubAggregateRoot)];
                var @delegate = aggregate2Delegates[typeof(StubEvent1)];
                @delegate.Invoke(aggregate, new StubEvent1());

                aggregate.Id.ShouldNotBe(default);
            }

            [Fact]
            public void Should_not_allow_public_event_event_handlers_on_aggregates()
            {
                Should.Throw<AggregateException>(() =>
                    _delegateFactory.GetEventApplicationMethods(typeof(StubAggregateRootWithPublicEventMethod)));
            }
        }
    }
}