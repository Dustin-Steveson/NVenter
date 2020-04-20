using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Core
{
    public interface IEventStream<TStreamConfiguration>
    {
        Task<EventStreamSlice> GetEvents(TStreamConfiguration configuration, long position);
    }
}