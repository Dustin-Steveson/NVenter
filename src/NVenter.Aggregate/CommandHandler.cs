using Microsoft.Extensions.Logging;
using NVenter.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Aggregate
{
    public class CommandHandler<TCommand, TAggregateRoot>
        where TCommand : ICommand
        where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IAggregateRootRepository<TAggregateRoot> _aggregateRootRepository;
        private readonly IDomainCommandHandler<TCommand, TAggregateRoot> _commandHandler;

        public CommandHandler(
            IAggregateRootRepository<TAggregateRoot> aggregateRootRepository,
            IDomainCommandHandler<TCommand, TAggregateRoot> commandHandler,
            ILogger logger)
        {
            _aggregateRootRepository = aggregateRootRepository;
            _commandHandler = commandHandler;
        }

        public async Task Run(Guid aggregateId, TCommand command)
        {
            var aggregate = await _aggregateRootRepository.Get(aggregateId, command is IAggregateCreationCommand);
            await _commandHandler.HandleCommand(command, aggregate);
            await _aggregateRootRepository.Save(
                aggregate
                    .GetUncommittedChanges()
                    .Select(e => new EventWrapper(e, new Metadata())),
                aggregateId,
                aggregate.Version);

            aggregate.MarkChangesCommited();
        }
    }
}
