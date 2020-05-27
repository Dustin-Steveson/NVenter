using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Core
{

    public interface IEventWriter
    {
        Task SaveEvents(string streamName, IEnumerable<EventWrapper> events);
    }
}