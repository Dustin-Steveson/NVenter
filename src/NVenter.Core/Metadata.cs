using System;

namespace NVenter.Core
{
    public class Metadata
    {
        public Guid Id { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
        public DateTimeOffset Created { get; set; }
        public int StreamPosition { get; set; }
    }
}