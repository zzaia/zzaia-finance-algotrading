using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MagoTrader.Exchange.MercadoBitcoin.Public
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class OrderBookDTO
    {
        [JsonPropertyName("asks")]
        public decimal[][] Asks { get; set; }

        [JsonPropertyName("bids")]
        public decimal[][] Bids { get; set; }

        [JsonPropertyName("ticker")]
        public string MainTicker { get; set; }

        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }

    }
}
