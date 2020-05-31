using System;
using System.Collections.Generic;
using System.Text;

namespace MagoTrader.Core.Exchange
{
    public class Response<T>
    {
        public string Message { get; set; }
        public System.Net.HttpStatusCode Code { get; set; }
        public T Item { get; set; }
        public List<T> Values { get; set; }
        public T Value { get; set; }
        public bool IsOK()
        {
            return Code == System.Net.HttpStatusCode.OK;
        }
    }
}
