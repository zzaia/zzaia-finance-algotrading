using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ApiClientBase
    {
        protected ILogger<object> _logger;
        public async Task<Response<T>> GetResponseAsync<T>(HttpResponseMessage response) where T : class
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var customerFromApi = await JsonSerializer.DeserializeAsync<T>(responseStream).ConfigureAwait(false);
                return new Response<T>(customerFromApi);
            }
            else
            {
                _logger.LogError($"Client Api response returned {response.StatusCode} {response.ReasonPhrase}");
                using var problemStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(problemStream).ConfigureAwait(false);
                return new Response<T>(problemDetails);
            }
        }

        public async Task<Response> GetResponseAsync(HttpResponseMessage response)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));

            if (response.IsSuccessStatusCode)
            {
                return new Response(true);
            }
            else
            {
                _logger.LogError($"Client Api response returned {response.StatusCode} {response.ReasonPhrase}");
                using var problemStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(problemStream).ConfigureAwait(false);
                return new Response(problemDetails);
            }
        }
    }
}
