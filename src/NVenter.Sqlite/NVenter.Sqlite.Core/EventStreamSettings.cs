using System;
using System.Collections.Generic;

namespace NVenter.Sqlite.Core {
    public class EventStreamSettings {
        public IEnumerable<Type> TypesToStream { get; set; }
    }
}