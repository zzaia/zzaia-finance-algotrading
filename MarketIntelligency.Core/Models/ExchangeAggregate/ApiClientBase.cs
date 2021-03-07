﻿using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    public class ApiClientBase
    {
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
                using var problemStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                var problemDetails = await JsonSerializer.DeserializeAsync<ProblemDetails>(problemStream).ConfigureAwait(false);
                return new Response(problemDetails);
            }
        }
    }
}