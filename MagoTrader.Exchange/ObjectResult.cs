namespace MagoTrader.Exchange
{
    public class ObjectResult<T> where T : class
    {
        public bool Succeed { get; set; }
        public T Output { get; set; }
        public ApplicationError Error { get; set; }
    }

    public static class ObjectResultFactory
    {
        public static ObjectResult<T> CreateSuccessResult<T>(T output) where T : class
        {
            return new ObjectResult<T>
            {
                Succeed = true,
                Output = output
            };
        }

        public static ObjectResult<T> CreateFailResult<T>() where T : class
        {
            return new ObjectResult<T>
            {
                Output = null,
                Succeed = false,
                Error = ApplicationErrors.InternalServerError
            };
        }
    }
}
