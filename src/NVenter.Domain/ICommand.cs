using NVenter.Core;
using System;

namespace NVenter.Domain
{
    public interface ICommand : IMessage
    {
        Guid AggregateId { get; set; }
    }
}
