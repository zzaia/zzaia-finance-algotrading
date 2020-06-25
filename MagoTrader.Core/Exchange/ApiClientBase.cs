using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagoTrader.Core.Exchange
{
    public class ApiClientBase
    {
        private const string _okMessage = "Success";
        private readonly string _notFoundMessage = "No information found";
        private readonly string _unauthorizedMessage = "User not authorized";
        private readonly string _internalServerErrorMessage = "Internal server error";

        public async Task<Response<T>> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var resp = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                T value = JsonSerializer.Deserialize<T>(resp, options);
                return new Response<T> { Code = System.Net.HttpStatusCode.OK, Message = _okMessage, Value = value };
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Response<T> { Code = System.Net.HttpStatusCode.NotFound, Message = _notFoundMessage };
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return new Response<T> { Code = System.Net.HttpStatusCode.Unauthorized, Message = _unauthorizedMessage };
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new Response<T> { Code = System.Net.HttpStatusCode.InternalServerError, Message = _internalServerErrorMessage };
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return new Response<T> { Code = System.Net.HttpStatusCode.BadRequest, Message = $"{httpResponseMessage.ReasonPhrase}" };
            }
            else
            {
                return new Response<T> { Code = httpResponseMessage.StatusCode, Message = $"{httpResponseMessage.ReasonPhrase}" };
            }
        }
    }
}
