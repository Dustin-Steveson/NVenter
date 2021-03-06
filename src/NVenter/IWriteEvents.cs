﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace NVenter
{
    public interface IWriteEvents
    {
        Task SaveEvents(string streamName, IEnumerable<IEvent> events, int expectedVersion);
        Task SaveEvents(string streamName, IEnumerable<IEvent> events);
    }
}