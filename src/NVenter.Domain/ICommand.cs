using System;

namespace NVenter.Domain
{
    public interface ICommand
    {
        Guid AggregateId { get; set; }
        Guid Id { get; set; }
    }
}
