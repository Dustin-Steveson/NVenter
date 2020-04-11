using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Core
{
    public interface IEventRepository
    {
        Task<IEnumerable<EventWrapper>> GetEvents<TAggregate>(string streamName);
        Task SaveEvents<TAggregate>(string streamName, IEnumerable<EventWrapper> events, uint expectedVersion);
    }
}