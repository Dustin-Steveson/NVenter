using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Core
{
    public interface IEventStream<TStreamParameters>
    {
        Task<EventStreamSlice> GetEvents(TStreamParameters parameters);
    }
}