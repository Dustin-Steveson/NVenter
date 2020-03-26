using System;

namespace NVenter.Aggregate
{
    public interface ICommand
    {
        Guid AggregateId { get; set; }
        Guid Id { get; set; }
    }
}
