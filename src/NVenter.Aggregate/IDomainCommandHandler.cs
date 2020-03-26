using System.Threading.Tasks;

namespace NVenter.Aggregate
{
    public interface IDomainCommandHandler<TCommand, TAggregateRoot>
        where TCommand : ICommand
        where TAggregateRoot : AggregateRoot
    {
        Task HandleCommand(TCommand command, TAggregateRoot aggregateRoot);
    }
}
