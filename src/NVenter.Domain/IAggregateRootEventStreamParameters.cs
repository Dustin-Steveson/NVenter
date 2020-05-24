using NVenter.Core;
using System;

namespace NVenter.Domain
{
    public interface IAggregateRootEventStreamParameters<TAggregateRoot> where TAggregateRoot : AggregateRoot { }
}