using System.Threading.Tasks;

namespace NVenter.Core {
    public interface IMessageStateRepository<TMessage, TState> where TMessage : IMessage {
        Task<TState> Get(TMessage message);
        Task Save(TState state, MessageContext context);
    }
}