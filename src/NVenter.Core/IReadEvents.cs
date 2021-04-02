using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Core
{
    public interface IReadEvents
    {
        Task<IEnumerable<IEvent>> GetEvents(string streamName, int position);
    }
}