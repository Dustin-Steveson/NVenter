using NVenter.Core;
using System.Linq;
using System.Threading.Tasks;

namespace NVenter.Domain
{
    public class AgggregateRootMessageStateRepository<TMessage, TAggregateRoot>
        : IMessageStateRepository<TMessage, TAggregateRoot>
        where TMessage : ICommand
        where TAggregateRoot : AggregateRoot, new()
    {
        private readonly IAggregateRootRepository<TAggregateRoot> _repository;
        public AgggregateRootMessageStateRepository(IAggregateRootRepository<TAggregateRoot> repository)
        {
            _repository = repository;
        }

        public Task<TAggregateRoot> Get(TMessage message)
        {
            return _repository.Get(message.AggregateId, message is IAggregateCreationCommand == false);
        }

        public Task Save(TAggregateRoot state, MessageContext context)
        {
            return _repository.Save(state, context);
        }
    }
}