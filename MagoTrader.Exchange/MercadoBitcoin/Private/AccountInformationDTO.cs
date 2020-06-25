using System.Text.Json.Serialization;

namespace MagoTrader.Exchange.MercadoBitcoin.Private
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    class AccountInformationDTO
    {
        [JsonPropertyName("balance")]
        public AssetsDTO Assets { get; set; }

        [JsonPropertyName("withdrawal_limits")]
        public WithdrawLimitsDTO WithdrawLimits { get; set; }
    }
}
