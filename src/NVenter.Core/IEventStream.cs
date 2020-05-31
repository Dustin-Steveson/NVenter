using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter.Core
{
    public interface IEventStreamReader<TEventStream> {
        Task<EventStreamSlice> ReadStreamForward(long position, TEventStream eventStream);
    }
    public interface IEventStream {
        Task<EventStreamSlice> GetEvents();
    }
}