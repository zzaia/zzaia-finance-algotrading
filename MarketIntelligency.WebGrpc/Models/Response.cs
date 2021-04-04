using Grpc.Core;

namespace MarketIntelligency.WebGrpc.Models
{
    public class Response<T> where T: class
    {
        public Response() { }
        public Response(T output)
        {
            Output = output;
        }
        public Response(Status status)
        {
            Status = status;
        }
        public T Output { get; set; }
        public Status Status { get; set; }
        public bool Succeed => Output != null;
        public bool HasDetails => Status.Detail != null && string.IsNullOrEmpty(Status.Detail);
    }
    public class Response
    {
        public Response() { }
        public Response(bool success)
        {
            Succeed = success;
        }
        public Response(Status status)
        {
            Status = status;
        }
        public bool Output => Succeed;
        public Status Status { get; set; }
        public bool Succeed { get; set; }
        public bool HasDetails => Status.Detail != null && string.IsNullOrEmpty(Status.Detail);
    }
}