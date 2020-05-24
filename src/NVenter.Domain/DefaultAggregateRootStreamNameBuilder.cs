namespace NVenter.Domain {
    public class DefaultAggregateRootStreamNameBuilder : IBuildAggregateStreamNames {
        public string GetStreamName<TAggregateRoot>(TAggregateRoot aggregateRoot) 
            where TAggregateRoot : AggregateRoot, new() {
            return $"{typeof(TAggregateRoot).FullName}-{aggregateRoot.Id}";
        }
    }
}