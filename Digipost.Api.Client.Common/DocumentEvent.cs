using System;
using System.Collections;
using System.Collections.Generic;
using Digipost.Api.Client.Common.Enums;

namespace Digipost.Api.Client.Common
{
    public class DocumentEvents : IEnumerable<DocumentEvent>
    {
        private readonly IEnumerable<DocumentEvent> _events;

        public DocumentEvents(IEnumerable<DocumentEvent> events)
        {
            _events = events;
        }

        public IEnumerator<DocumentEvent> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class DocumentEvent
    {
        public Guid Guid { get; }

        public DocumentEventType EventType { get; }

        public DateTime Created { get; }

        public DateTime DocumentCreated { get; }

        public EventMetadata EventMetadata { get; set; }

        public DocumentEvent(Guid guid, DocumentEventType eventType, DateTime created, DateTime documentCreated)
        {
            Guid = guid;
            EventType = eventType;
            Created = created;
            DocumentCreated = documentCreated;
        }
    }

    public abstract class EventMetadata
    {
    }

    public class RequestForRegistrationExpiredMetadata : EventMetadata
    {
        /// <summary>
        /// Values of FallbackChannel might be 'none' or 'print'.
        /// </summary>
        public string FallbackChannel { get; }

        public RequestForRegistrationExpiredMetadata(string fallbackChannel)
        {
            FallbackChannel = fallbackChannel;
        }
    }
}
