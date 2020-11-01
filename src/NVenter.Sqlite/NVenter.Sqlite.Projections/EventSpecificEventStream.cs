using NVenter.Sqlite.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NVenter.Sqlite.Projections {
    public class EventSpecificEventStream : EventStream {
        private readonly EventStreamSettings _settings;

        public EventSpecificEventStream(EventStreamSettings settings, ConnectionFactory connectionFactory)
            : base(settings, connectionFactory) {
            _settings = settings;
        }

        public override IDictionary<string, object> StreamSpecificParameters => new Dictionary<string, object> 
        { ["events"] = _settings.TypesToStream.Select(_ => _.FullName) };

        public override string Sql => @"
SELECT * 
FROM EVENTS 
WHERE EventType in (@events) AND
      GlobalPosition > @GlobalPosition
ORDER BY GlobalPosition";
    }
}
