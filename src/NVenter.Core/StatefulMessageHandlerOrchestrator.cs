using System.Threading.Tasks;

namespace NVenter.Core {
    public class StatefulMessageHandlerOrchestrator<TMessage, TState>
        : IHandleMessages<TMessage>
        where TMessage : IMessage
    {
        private readonly IMessageStateRepository<TMessage, TState> _stateRepository;
        private readonly IStatefulMessageHandler<TMessage, TState> _handler;

        public StatefulMessageHandlerOrchestrator(IMessageStateRepository<TMessage, TState> stateRepository, IStatefulMessageHandler<TMessage, TState> handler)
        {
            _stateRepository = stateRepository;
            _handler = handler;
        }

        public async Task Handle(TMessage message, MessageContext context)
        {
            var state = await _stateRepository.Get(message);
            var newState = await _handler.Handle(message, state, context);
            await _stateRepository.Save(newState, context);
        }
    }
}