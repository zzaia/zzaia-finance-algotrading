using Google.Protobuf;
using Google.Protobuf.Reflection;
using MediatR;
using System;

namespace MarketIntelligency.Core.Models
{
    public class EventSource<T> : IMessage, INotification where T : class
    {
        public EventSource() { }
        public EventSource(T content)
        {
            Content = content;
            OcurredAt = DateTimeOffset.UtcNow;
        }
        public EventSource(T content, DateTimeOffset ocurredAt, DateTimeOffset recordedAt)
        {
            Content = content;
            OcurredAt = ocurredAt;
            RecordedAt = recordedAt;
        }
        public DateTimeOffset OcurredAt { get; set; }
        public DateTimeOffset RecordedAt { get; set; }
        public T Content { get; set; }

        public MessageDescriptor Descriptor => throw new NotImplementedException();

        public int CalculateSize()
        {
            throw new NotImplementedException();
        }

        public void MergeFrom(CodedInputStream input)
        {
            throw new NotImplementedException();
        }

        public void WriteTo(CodedOutputStream output)
        {
            throw new NotImplementedException();
        }
    }
}