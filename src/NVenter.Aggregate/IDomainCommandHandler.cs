using System.Threading.Tasks;

namespace NVenter.Domain
{
    public interface IDomainCommandHandler<TCommand, TAggregateRoot>
        where TCommand : ICommand
        where TAggregateRoot : AggregateRoot
    {
        Task HandleCommand(TCommand command, TAggregateRoot aggregateRoot);
    }
}
