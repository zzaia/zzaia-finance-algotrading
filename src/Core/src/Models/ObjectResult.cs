using System;

namespace Zzaia.Finance.Core.Models
{
    public class ObjectResult<T> where T : class
    {
        public bool Succeed { get; set; }
        public T Output { get; set; }
        public ApplicationError Error { get; set; }
        public Exception Exception { get; set; }

    }

    public class ObjectResult
    {
        public bool Succeed { get; set; }
        public bool Output => Succeed;
        public ApplicationError Error { get; set; }
        public Exception Exception { get; set; }
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
        
        public static ObjectResult<T> CreateFailResult<T>(Exception ex) where T : class
        {
            return new ObjectResult<T>
            {
                Output = null,
                Succeed = false,
                Error = ApplicationErrors.InternalServerError,
                Exception = ex
            };
        }

        public static ObjectResult<T> CreateInvalidArgumentResult<T>() where T : class
        {
            return new ObjectResult<T>
            {
                Output = null,
                Succeed = false,
                Error = ApplicationErrors.BadRequestError
            };
        }

        public static ObjectResult CreateSuccessResult()
        {
            return new ObjectResult
            {
                Succeed = true,
            };
        }

        public static ObjectResult CreateFailResult()
        {
            return new ObjectResult
            {
                Succeed = false,
                Error = ApplicationErrors.InternalServerError
            };
        }

        public static ObjectResult CreateInvalidArgumentResult()
        {
            return new ObjectResult
            {
                Succeed = false,
                Error = ApplicationErrors.BadRequestError
            };
        }
    }
}