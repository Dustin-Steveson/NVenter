using System;

namespace NVenter.Core
{
    public class MessageContext
    {
        public MessageContext(Guid id, Guid causationId, Guid coorelationId, DateTimeOffset created)
        {
            Id = id;
            CausationId = causationId;
            CoorelationId = coorelationId;
            Created = created;
        }
        public Guid Id { get; }
        public Guid CausationId { get; }
        public Guid CoorelationId { get; }
        public DateTimeOffset Created { get; }
    }
}