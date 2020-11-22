using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MarketIntelligency.Core.Models.ExchangeAggregate
{
    /// <summary>
    /// A machine-readable format for specifying errors in HTTP API responses based on
    /// https://tools.ietf.org/html/rfc7807.
    /// </summary>
    public class ProblemDetails
    {
        public ProblemDetails() { }
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("detail")]
        public string Detail { get; set; }
        [JsonPropertyName("status")]
        public int Status { get; set; }
        [JsonPropertyName("instance")]
        public string Instance { get; set; }
        [JsonExtensionData]
        public IDictionary<string, object> Extensions { get; set; }
        [JsonPropertyName("errors")]
        public IDictionary<string, string[]> Errors { get; set; }
        public bool IsValidationProblem => Errors != null;
    }
}
