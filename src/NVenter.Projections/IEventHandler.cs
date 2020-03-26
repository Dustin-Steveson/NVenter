using NVenter.Core;
using System.Threading.Tasks;

namespace NVenter.Projections
{
    public interface IEventHandler<TEvent> : IEventHandler where TEvent: IEvent
    {
        Task Handle(EventWrapper<TEvent> eventWrapper);
    }

    public interface IEventHandler { }
}
