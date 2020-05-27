using System;

namespace NVenter.Domain
{
    public interface IAggregateRootEventStreamParametersFactory<TStreamParams, TAggregateRoot> where TAggregateRoot : AggregateRoot, new() 
    {
        TStreamParams GetParameters(Guid aggregateId);
    }
}