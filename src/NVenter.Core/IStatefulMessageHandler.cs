using System.Threading.Tasks;

namespace NVenter.Core {
    public interface IStatefulMessageHandler<TMessage, TState> {
        Task<TState> Handle(TMessage message, TState state, MessageContext context);
    }
}