namespace MagoTrader.Core.Exchange
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
    public class Response
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
    }
}
