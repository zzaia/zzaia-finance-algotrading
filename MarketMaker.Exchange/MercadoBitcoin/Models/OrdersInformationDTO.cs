using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace MarketMaker.Exchange.MercadoBitcoin.Models
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class OrdersInformationDTO
    {
        [JsonPropertyName("orders")]
        public IEnumerable<OrderDTO> Orders { get; set; }

    }
}
