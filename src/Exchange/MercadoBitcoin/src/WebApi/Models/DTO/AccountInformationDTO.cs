using System.Text.Json.Serialization;

namespace Zzaia.Finance.Exchange.MercadoBitcoin.WebApi.Models.DTO
{
    /// <summary>
    /// Data Transfer Object to be used in json to model serialization
    /// </summary>
    public class AccountInformationDTO
    {
        [JsonPropertyName("balance")]
        public AssetsDTO Assets { get; set; }

        [JsonPropertyName("withdrawal_limits")]
        public WithdrawLimitsDTO WithdrawLimits { get; set; }
    }
}
