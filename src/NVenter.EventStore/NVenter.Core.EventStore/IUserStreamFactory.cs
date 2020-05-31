using System;

namespace NVenter.Core.EventStore {
    public interface IUserStreamFactory {
        IEventStream MakeStream(Guid userId, long position, int eventCount);
    }
}