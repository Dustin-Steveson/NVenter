namespace NVenter.Domain {
    public interface IBuildAggregateStreamNames {
        string GetStreamName<TAggregateRoot>(TAggregateRoot aggregateRoot) where TAggregateRoot : AggregateRoot, new();
    }
}