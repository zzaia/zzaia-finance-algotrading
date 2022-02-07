using Google.Protobuf;
using Google.Protobuf.Reflection;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class Response<T> where T : class
    {
        public Response() { }
        public Response(T output)
        {
            Output = output;
        }
        public Response(ProblemDetails problemDetails)
        {
            ProblemDetails = problemDetails;
        }
        public T Output { get; set; }
        public ProblemDetails ProblemDetails { get; set; }
        public bool Success => Output != null;
    }
    public class Response : IMessage
    {
        public Response() { }
        public Response(bool success)
        {
            Success = success;
        }
        public Response(ProblemDetails problemDetails)
        {
            ProblemDetails = problemDetails;
        }
        public bool Output => Success;
        public ProblemDetails ProblemDetails { get; set; }
        public bool Success { get; set; }

        public MessageDescriptor Descriptor => throw new System.NotImplementedException();

        public int CalculateSize()
        {
            throw new System.NotImplementedException();
        }

        public void MergeFrom(CodedInputStream input)
        {
            throw new System.NotImplementedException();
        }

        public void WriteTo(CodedOutputStream output)
        {
            throw new System.NotImplementedException();
        }
    }
}
