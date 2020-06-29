using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MagoTrader.Core.Exchange
{
    public class ApiClientBase
    {
        public async Task<Response<T>> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage) where T : class
        {
            if (httpResponseMessage == null) throw new ArgumentNullException(nameof(httpResponseMessage));

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var customerFromApi = await JsonSerializer.DeserializeAsync<T>(responseStream).ConfigureAwait(false);
                return new Response<T>(customerFromApi);
            }
            else
            {
                using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var errorDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(responseStream).ConfigureAwait(false);
                return new Response<T>(errorDetails);
            }
        }
    }
}
