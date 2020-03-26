using NVenter.Core;
using System;
using Xunit;

namespace NVenter.Aggregate.UnitTests
{
    public class AggregateRootTests
    {
        [Fact]
        public void Test1()
        {
            var testAggregate = new AggregateRootStub();
            AggregateRootInitializer.Initialize(this.GetType().Assembly);
            testAggregate.Apply(new TestEvent());

            Assert.Equal(1, testAggregate.TestEventApplyCalls);
        }
    }

    public class AggregateRootStub : AggregateRoot
    {
        public int TestEventApplyCalls { get; private set; }
        private void Apply(TestEvent @event) { TestEventApplyCalls++; }
    }

    public class TestEvent : IEvent
    {

    }
}