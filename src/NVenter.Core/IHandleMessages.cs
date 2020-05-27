using System.Threading.Tasks;

namespace NVenter.Core {
    public interface IHandleMessages<TMessage>
        : IHandleMessages
        where TMessage : IMessage
    {
        Task Handle(TMessage message, MessageContext context);
    }

    public interface IHandleMessages { }
}