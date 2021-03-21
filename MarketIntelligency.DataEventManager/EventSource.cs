using MediatR;
using System;

namespace MarketIntelligency.DataEventManager
{
    public class EventSource<T> : INotification where T : class
    {
        public EventSource(T content)
        {
            Content = content;
            OcurredAt = DateTimeOffset.UtcNow;
        }
        public DateTimeOffset OcurredAt { get; set; }
        public DateTimeOffset RecordedAt { get; set; }
        public T Content { get; set; }
    }
}